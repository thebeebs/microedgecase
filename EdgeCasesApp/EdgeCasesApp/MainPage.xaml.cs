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

        private async void button_Click(object sender, RoutedEventArgs e)
        {

            var url = textBox.Text;
            RootObject edgeCase = await EdgeCaseModel.GetResults(url);

            textResult_browserDetection.Text = "Browser Detection  \t  " + BoolToString(edgeCase.results.browserDetection.passed);
            textResult_markup.Text = "Markup  \t  " + BoolToString(edgeCase.results.markup.passed);
            textResult_pluginFree.Text = "Plugin Free \t  " + BoolToString(edgeCase.results.pluginfree.passed);
            textResult_jsLibs.Text = "JS Libs \t  " + BoolToString(edgeCase.results.jslibs.passed);
            textResult_edge.Text = "Edge \t  " + BoolToString(edgeCase.results.edge.passed);
            textResult_css.Text = "CSS Prefxies \t  " + BoolToString(edgeCase.results.cssprefixes.passed);

        }

        public string BoolToString(bool boolean)
        {
            string result;

            if (boolean == true)
            {
                result = "PASS";
            }
            else
            {
                result = "FAIL";
            }

            return result;
        }
    }
}
