/*
 * Copyright © 2013 Nokia Corporation. All rights reserved.
 * Nokia and Nokia Connecting People are registered trademarks of Nokia Corporation. 
 * Other product and company names mentioned herein may be trademarks
 * or trade names of their respective owners. 
 * See LICENSE.TXT for license information.
 */

using ChromaKeyDemo.Resources;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Nokia.Graphics.Imaging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using Windows.Phone.Media.Capture;

namespace ChromaKeyDemo.Pages
{
    public partial class MainPage : PhoneApplicationPage
    {
        private static int RESOLUTION_WIDTH = 640;
        private static int RESOLUTION_HEIGHT = 480;
        private static CameraSensorLocation SENSOR_LOCATION = CameraSensorLocation.Back;

        private WriteableBitmap _bitmap = null;
        private DispatcherTimer _timer = null;
        private Color _color = new Color();
        private Semaphore _semaphore = new Semaphore(1, 1);
        private bool _initialized = false;

        private CameraPreviewImageSource _source = null;
        private WriteableBitmapRenderer _renderer = null;
        private FilterEffect _effect = null;
        private IList<IFilter> _filters = null;
        private ChromaKeyFilter _chromaKeyFilter = null;
        private RotationFilter _rotationFilter = null;

        public MainPage()
        {
            InitializeComponent();

            var aboutMenuItem = new ApplicationBarMenuItem()
            {
                Text = AppResources.MainPage_AboutMenuItem_Text
            };

            aboutMenuItem.Click += AboutMenuItem_Click;

            ApplicationBar.MenuItems.Add(aboutMenuItem);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Initialize();

            if (BackgroundMediaElement.Visibility == Visibility.Visible)
            {
                BackgroundMediaElement.Play();
            }

            _timer.Start();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (BackgroundMediaElement.Visibility == Visibility.Visible)
            {
                BackgroundMediaElement.Stop();
            }

            Uninitialize();
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/AboutPage.xaml", UriKind.Relative));
        }

        private void Initialize()
        {
            // Initialize camera

            var rotation = SENSOR_LOCATION == CameraSensorLocation.Back ? App.Camera.SensorRotationInDegrees : - App.Camera.SensorRotationInDegrees;

            ViewfinderBrush.SetSource(App.Camera);
            ViewfinderBrushTransform.Rotation = rotation;

            // Setup image processing pipeline

            _source = new CameraPreviewImageSource(App.Camera);

            _rotationFilter = new RotationFilter(rotation);

            _chromaKeyFilter = new ChromaKeyFilter();

            _filters = new List<IFilter>();
            _filters.Add(_rotationFilter);
            _filters.Add(_chromaKeyFilter);

            _effect = new FilterEffect(_source);
            _effect.Filters = _filters;

            _bitmap = new WriteableBitmap(RESOLUTION_WIDTH, RESOLUTION_HEIGHT);

            _renderer = new WriteableBitmapRenderer(_effect, _bitmap, OutputOption.Stretch);

            FilteredImage.Source = _bitmap;

            _color = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);

            ColorBorder.Background = new SolidColorBrush(_color);

            // Start rendering timer

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };
            _timer.Tick += DispatcherTimer_Tick;

            _initialized = true;
        }

        private void Uninitialize()
        {
            _initialized = false;

            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }

            if (_renderer != null)
            {
                _renderer.Dispose();
                _renderer = null;
            }

            if (_effect != null)
            {
                _effect.Dispose();
                _effect = null;
            }

            _filters = null;
            _chromaKeyFilter = null;
            _rotationFilter = null;
            _bitmap = null;

            if (_source != null)
            {
                _source.Dispose();
                _source = null;
            }
        }

        private async void DispatcherTimer_Tick(object sender, System.EventArgs e)
        {
            _timer.Stop();

            await RenderFilteredImage();

            if (_initialized)
            {
                _timer.Start();
            }
        }

        private async Task RenderFilteredImage()
        {
            if (_initialized)
            {
                try
                {
                    _chromaKeyFilter.Color = Windows.UI.Color.FromArgb(_color.A, _color.R, _color.G, _color.B);
                    _chromaKeyFilter.ColorDistance = DistanceSlider.Value;

                    await _renderer.RenderAsync();

                    _bitmap.Invalidate();
                }
                catch (Exception)
                {
                }
            }
        }

        private async void ViewfinderCanvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var point = e.GetPosition(ViewfinderCanvas);

            _color = await PickColorFromImageAsync(ViewfinderCanvas, point);

            ColorBorder.Background = new SolidColorBrush(_color);
        }

        private async Task<Color> PickColorFromImageAsync(FrameworkElement element, Point point)
        {
            var bitmap = new WriteableBitmap((int)element.ActualWidth, (int)element.ActualHeight);

            using (var source = new CameraPreviewImageSource(App.Camera))
            using (var effect = new FilterEffect(source))
            using (var renderer = new WriteableBitmapRenderer(effect, bitmap, OutputOption.Stretch))
            {
                effect.Filters = new List<IFilter>()
                {
                    new RotationFilter(_rotationFilter.RotationAngle)
                };

                await renderer.RenderAsync();

                //System.Diagnostics.Debug.WriteLine("Bitmap({0}, {1}) - Point({2}, {3})", bitmap.PixelWidth, bitmap.PixelHeight, point.X, point.Y);

                var picked = bitmap.Pixels[((int)point.Y) * bitmap.PixelWidth + ((int)point.X)];

                var color = new Color
                {
                    A = 0xFF,
                    R = (byte)((picked & 0x00FF0000) >> 16),
                    G = (byte)((picked & 0x0000FF00) >> 8),
                    B = (byte)(picked & 0x000000FF)
                };

                return color;
            }
        }

        private async void FilteredImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (BackgroundMediaElement.Visibility == Visibility.Collapsed)
            {
                BackgroundMediaElement.Play();
                BackgroundMediaElement.Visibility = Visibility.Visible;
            }
            else
            {
                BackgroundMediaElement.Stop();
                BackgroundMediaElement.Visibility = Visibility.Collapsed;
            }
        }

        private void BackgroundMediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundMediaElement.Play();
        }
    }
}