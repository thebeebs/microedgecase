using System;
using System.Collections.Generic;
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

            var url = TextBox_URLInput.Text;
            RootObject edgeCase = await EdgeCaseModel.GetResults(url);

            resultsProgressRing.IsActive = false;

            /*
                Due to the inconsitent nature of the resulting data from the API, the text blocks have
                been hardcoded. It is not beneficial to abstract each property into a super class as
                they each have different attributes.
                TODO: 
                Reengineer the UI 
                    - Change IsEnabled control template background.
                    - Separate textbox into multiple indepenent components (Name, Status and Expand/Collapse button)
            */
            SolidColorBrush PASS_COLOUR = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 0));
            SolidColorBrush FAIL_COLOR = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0));

            var browserDetectionResult = edgeCase.results.browserDetection;
            TextBox_BrowserDetection.Text = "Browser Detection  \t\t " + StringifyResult(browserDetectionResult.passed);
            TextBox_BrowserDetection.Background = browserDetectionResult.passed ? PASS_COLOUR : FAIL_COLOR;

            var markupResult = edgeCase.results.browserDetection;
            TextBox_Markup.Text = "Markup  \t\t  " + StringifyResult(markupResult.passed);
            TextBox_Markup.Background = markupResult.passed ? PASS_COLOUR : FAIL_COLOR;

            var pluginFreeResult = edgeCase.results.browserDetection;
            TextBox_PluginFree.Text = "Plugin Free \t\t  " + StringifyResult(pluginFreeResult.passed);
            TextBox_PluginFree.Background = pluginFreeResult.passed ? PASS_COLOUR : FAIL_COLOR;

            var jsLibsResult = edgeCase.results.browserDetection;
            TextBox_JSLibs.Text = "JS Libs \t\t  " + StringifyResult(jsLibsResult.passed);
            TextBox_JSLibs.Background = jsLibsResult.passed ? PASS_COLOUR : FAIL_COLOR;

            var edgeResult = edgeCase.results.browserDetection;
            TextBox_Edge.Text = "Edge \t\t  " + StringifyResult(edgeResult.passed);
            TextBox_Edge.Background = edgeResult.passed ? PASS_COLOUR : FAIL_COLOR;

            var cssPrefixes = edgeCase.results.browserDetection;
            TextBox_CSSPrefixes.Text = "CSS Prefixes \t\t  " + StringifyResult(cssPrefixes.passed);
            TextBox_CSSPrefixes.Background = cssPrefixes.passed ? PASS_COLOUR : FAIL_COLOR;
        }

        private string StringifyResult(bool result)
        {
            var output = result ? "PASS": "FAIL";
            return output;
        }
    }
}
