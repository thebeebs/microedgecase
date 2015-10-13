using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EdgeCasesApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            resultsProgressRing.IsActive = true;

            //Getting URL from user input
            var url = TextBox_URLInput.Text;

            RootObject edgeCase;

            try
            {

                if(url == "")
                {
                    //Getting results from text message using Twilio
                    edgeCase = await EdgeCaseModel.GetResultsTwilio();
                }
                else
                {
                    //Getting results from user inputted URL
                    edgeCase = await EdgeCaseModel.GetResults(url);
                }

            }
            catch(Exception ex)
            {
                // handle exception...
                // for now just rethrow
                throw ex;
            }

            resultsProgressRing.IsActive = false;

            /*
                Due to the inconsitent nature of the resulting data from the API, the text blocks have
                been hardcoded. It is not beneficial to abstract each property into a super class as
                they each have different attributes.
                TODO: 
                Reengineer the UI 
                   - Collapse logic into functions
                   - Standardise data model
                   - Dynamically generate UI
            */

            SolidColorBrush PASS_COLOUR = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 0));
            SolidColorBrush FAIL_COLOR = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0));
            const string PASS_TEXT = "PASS";
            const string FAIL_TEXT = "FAIL";

            var browserDetectionResult = edgeCase.results.browserDetection;
            TextBox_BrowserDetectionTitle.Text = browserDetectionResult.testName;
            TextBox_BrowserDetectionStatus.Text = browserDetectionResult.passed ? PASS_TEXT : FAIL_TEXT;
            TextBox_BrowserDetectionPanel.Background = browserDetectionResult.passed ? PASS_COLOUR : FAIL_COLOR;
            TextBox_BrowserDetectionPanel.Visibility = Visibility.Visible;

            var markupResult = edgeCase.results.markup;
            TextBox_MarkupTitle.Text = markupResult.testName;
            TextBox_MarkupStatus.Text = markupResult.passed ? PASS_TEXT : FAIL_TEXT;
            TextBox_MarkupPanel.Background = markupResult.passed ? PASS_COLOUR : FAIL_COLOR;
            TextBox_MarkupPanel.Visibility = Visibility.Visible;

            var pluginFreeResult = edgeCase.results.pluginfree;
            TextBox_PluginFreeTitle.Text = pluginFreeResult.testName;
            TextBox_PluginFreeStatus.Text = pluginFreeResult.passed ? PASS_TEXT : FAIL_TEXT;
            TextBox_PluginFreePanel.Background = pluginFreeResult.passed ? PASS_COLOUR : FAIL_COLOR;
            TextBox_PluginFreePanel.Visibility = Visibility.Visible;

            var jsLibsResult = edgeCase.results.jslibs;
            TextBox_JSLibTitle.Text = jsLibsResult.testName;
            TextBox_JSLibStatus.Text = jsLibsResult.passed ? PASS_TEXT : FAIL_TEXT;
            TextBox_JSLibPanel.Background = jsLibsResult.passed ? PASS_COLOUR : FAIL_COLOR;
            TextBox_JSLibPanel.Visibility = Visibility.Visible;

            var edgeResult = edgeCase.results.edge;
            TextBox_EdgeTitle.Text = edgeResult.testName;
            TextBox_EdgeStatus.Text = edgeResult.passed ? PASS_TEXT : FAIL_TEXT;
            TextBox_EdgePanel.Background = edgeResult.passed ? PASS_COLOUR : FAIL_COLOR;
            TextBox_EdgePanel.Visibility = Visibility.Visible;

            var cssPrefixes = edgeCase.results.cssprefixes;
            TextBox_CSSPrefixesTitle.Text = cssPrefixes.testName;
            TextBox_CSSPrefixesStatus.Text = cssPrefixes.passed ? PASS_TEXT : FAIL_TEXT;
            TextBox_CSSPrefixesPanel.Background = cssPrefixes.passed ? PASS_COLOUR : FAIL_COLOR;
            TextBox_CSSPrefixesPanel.Visibility = Visibility.Visible;
        }

        private void Button_BrowserDetectionDetails_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_MarkupDetails_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_JSLibDetails_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_CSSPrefixesDetails_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_PluginFreeDetails_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_EdgeDetails_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

