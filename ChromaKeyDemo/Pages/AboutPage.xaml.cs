/*
 * Copyright © 2013 Nokia Corporation. All rights reserved.
 * Nokia and Nokia Connecting People are registered trademarks of Nokia Corporation. 
 * Other product and company names mentioned herein may be trademarks
 * or trade names of their respective owners. 
 * See LICENSE.TXT for license information.
 */

using ChromaKeyDemo.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;

namespace ChromaKeyDemo.Pages
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();

            // Application version number

            var version = XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;

            var versionRun = new Run()
            {
                Text = String.Format(AppResources.AboutPage_VersionRun_Text, version) + "\n"
            };

            VersionParagraph.Inlines.Add(versionRun);

            // Application about text

            var aboutRun = new Run()
            {
                Text = AppResources.AboutPage_AboutRun_Text + "\n"
            };

            AboutParagraph.Inlines.Add(aboutRun);

            // Application guide text

            var guideRun = new Run()
            {
                Text = AppResources.AboutPage_GuideRun_Text + "\n"
            };

            GuideParagraph.Inlines.Add(guideRun);

            // Link to project website

            var projectRunText = AppResources.AboutPage_ProjectRun_Text;
            var projectRunTextSpans = projectRunText.Split(new string[] { "{0}" }, StringSplitOptions.None);

            var projectRunSpan1 = new Run();
            projectRunSpan1.Text = projectRunTextSpans[0];

            var projectRunSpan2 = new Run();
            projectRunSpan2.Text = projectRunTextSpans[1] + "\n";

            var projectsLink = new Hyperlink();
            projectsLink.Inlines.Add(AppResources.AboutPage_Hyperlink_Project_Url);
            projectsLink.Click += ProjectsLink_Click;

            ProjectParagraph.Inlines.Add(projectRunSpan1);
            ProjectParagraph.Inlines.Add(projectsLink);
            ProjectParagraph.Inlines.Add(projectRunSpan2);

            // Legal text

            // The video oceantrip-small.mp4 used in this application is a short clip from {0}.
            // The original video is copyright of the "zero-project" ({1}) and licensed under the
            // Creative Commons Attribution 3.0 Unported (CC BY 3.0) license ({2}).

            var legalRunText = AppResources.AboutPage_LegalRun_Text;
            var legalRunTextSpans = legalRunText.Split(new string[] { "{0}", "{1}", "{2}" }, StringSplitOptions.None);

            var legalRunSpan1 = new Run();
            legalRunSpan1.Text = legalRunTextSpans[0];

            var legalRunSpan2 = new Run();
            legalRunSpan2.Text = legalRunTextSpans[1] + "\n";

            var legalRunSpan3 = new Run();
            legalRunSpan3.Text = legalRunTextSpans[2] + "\n";

            var legalRunSpan4 = new Run();
            legalRunSpan4.Text = legalRunTextSpans[3] + "\n";

            var videoSourceLink = new Hyperlink();
            videoSourceLink.Inlines.Add(AppResources.AboutPage_Hyperlink_VideoSource_Text);
            videoSourceLink.Click += VideoSourceLink_Click;

            var zeroProjectLink = new Hyperlink();
            zeroProjectLink.Inlines.Add(AppResources.AboutPage_Hyperlink_ZeroProject_Text);
            zeroProjectLink.Click += ZeroProjectLink_Click;

            var creativeCommonsLink = new Hyperlink();
            creativeCommonsLink.Inlines.Add(AppResources.AboutPage_Hyperlink_CCBY30_Text);
            creativeCommonsLink.Click += CreativeCommonsLink_Click;

            LegalParagraph.Inlines.Add(legalRunSpan1);
            LegalParagraph.Inlines.Add(videoSourceLink);
            LegalParagraph.Inlines.Add(legalRunSpan2);
            LegalParagraph.Inlines.Add(zeroProjectLink);
            LegalParagraph.Inlines.Add(legalRunSpan3);
            LegalParagraph.Inlines.Add(creativeCommonsLink);
            LegalParagraph.Inlines.Add(legalRunSpan4);
        }

        private void ProjectsLink_Click(object sender, RoutedEventArgs e)
        {
            var webBrowserTask = new WebBrowserTask()
            {
                Uri = new Uri(AppResources.AboutPage_Hyperlink_Project_Url, UriKind.Absolute)
            };

            webBrowserTask.Show();
        }

        private void VideoSourceLink_Click(object sender, RoutedEventArgs e)
        {
            var webBrowserTask = new WebBrowserTask()
            {
                Uri = new Uri(AppResources.AboutPage_Hyperlink_VideoSource_Url, UriKind.Absolute)
            };

            webBrowserTask.Show();
        }

        private void ZeroProjectLink_Click(object sender, RoutedEventArgs e)
        {
            var webBrowserTask = new WebBrowserTask()
            {
                Uri = new Uri(AppResources.AboutPage_Hyperlink_ZeroProject_Url, UriKind.Absolute)
            };

            webBrowserTask.Show();
        }

        private void CreativeCommonsLink_Click(object sender, RoutedEventArgs e)
        {
            var webBrowserTask = new WebBrowserTask()
            {
                Uri = new Uri(AppResources.AboutPage_Hyperlink_CCBY30_Url, UriKind.Absolute)
            };

            webBrowserTask.Show();
        }
    }
}