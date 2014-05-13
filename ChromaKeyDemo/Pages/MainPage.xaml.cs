/**
 * Copyright (c) 2013-2014 Microsoft Mobile. All rights reserved.
 * 
 * Nokia and Nokia Connecting People are registered trademarks of Nokia Corporation. 
 * Other product and company names mentioned herein may be trademarks
 * or trade names of their respective owners.
 * 
 * See the license text file for license information.
 */

using ChromaKeyDemo.Resources;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Nokia.Graphics.Imaging;
using System;
using System.Collections.Generic;
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
        private WriteableBitmap _bitmap = null;
        private DispatcherTimer _timer = null;
        private Color _color = new Color();
        private CameraPreviewImageSource _source = null;
        private WriteableBitmapRenderer _renderer = null;
        private FilterEffect _effect = null;
        private IList<IFilter> _filters = null;
        private ChromaKeyFilter _chromaKeyFilter = null;
        private RotationFilter _rotationFilter = null;
        private bool _initialized = false;
        private bool _rendering = false;

        public MainPage()
        {
            InitializeComponent();

            var aboutMenuItem = new ApplicationBarMenuItem()
            {
                Text = AppResources.MainPage_AboutMenuItem_Text
            };

            aboutMenuItem.Click += AboutMenuItem_Click;

            ApplicationBar.MenuItems.Add(aboutMenuItem);

            Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewfinderCanvasArea.Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, ViewfinderCanvasArea.ActualWidth, ViewfinderCanvasArea.ActualHeight)
            };

            FilteredImageArea.Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, FilteredImageArea.ActualWidth, FilteredImageArea.ActualHeight)
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Initialize();

            BackgroundMediaElement.Source = new Uri("/Assets/Video/oceantrip-small.mp4", UriKind.Relative);

            if (BackgroundMediaElement.Visibility == Visibility.Visible)
            {
                BackgroundMediaElement.Position = TimeSpan.Zero;
                BackgroundMediaElement.Play();
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

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

            var rotation = App.Camera.SensorLocation == CameraSensorLocation.Back ?
                App.Camera.SensorRotationInDegrees : - App.Camera.SensorRotationInDegrees;

            ViewfinderBrush.SetSource(App.Camera);
            ViewfinderBrushTransform.Rotation = rotation;

            // Setup image processing pipeline

            _source = new CameraPreviewImageSource(App.Camera);
            _rotationFilter = new RotationFilter(rotation);
            _chromaKeyFilter = new ChromaKeyFilter();

            _filters = new List<IFilter> {_rotationFilter, _chromaKeyFilter};

            _effect = new FilterEffect(_source) {Filters = _filters};

            _bitmap = new WriteableBitmap((int)App.Camera.PreviewResolution.Width, (int)App.Camera.PreviewResolution.Height);
            _renderer = new WriteableBitmapRenderer(_effect, _bitmap, OutputOption.Stretch);

            FilteredImage.Source = _bitmap;

            _color = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);

            ColorBorder.Background = new SolidColorBrush(_color);

            // Start rendering timer

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };
            _timer.Tick += DispatcherTimer_Tick;
            _timer.Start();

            _initialized = true;
        }

        private void Uninitialize()
        {
            _initialized = false;

            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= DispatcherTimer_Tick;
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
            if (_initialized && !_rendering)
            {
                _rendering = true;

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

                _rendering = false;
            }
        }

        private async void ViewfinderCanvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var point = e.GetPosition(ViewfinderCanvas);
            var bitmap = new WriteableBitmap((int)ViewfinderCanvas.ActualWidth, (int)ViewfinderCanvas.ActualHeight);

            using (var source = new CameraPreviewImageSource(App.Camera))
            using (var effect = new FilterEffect(source))
            using (var renderer = new WriteableBitmapRenderer(effect, bitmap, OutputOption.Stretch))
            {
                effect.Filters = new List<IFilter>()
                {
                    new RotationFilter(_rotationFilter.RotationAngle)
                };

                await renderer.RenderAsync();

                var picked = bitmap.Pixels[((int)point.Y) * bitmap.PixelWidth + ((int)point.X)];

                _color = new Color
                {
                    A = 0xFF,
                    R = (byte)((picked & 0x00FF0000) >> 16),
                    G = (byte)((picked & 0x0000FF00) >> 8),
                    B = (byte)(picked & 0x000000FF)
                };
            }

            ColorBorder.Background = new SolidColorBrush(_color);
        }

        private void FilteredImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (BackgroundMediaElement.Visibility == Visibility.Collapsed)
            {
                BackgroundMediaElement.Position = TimeSpan.Zero;
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
            if (_initialized)
            {
                BackgroundMediaElement.Position = TimeSpan.Zero;
                BackgroundMediaElement.Play();
            }
        }
    }
}