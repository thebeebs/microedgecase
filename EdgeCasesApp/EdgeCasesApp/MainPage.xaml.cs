﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Devices.Gpio;
using Windows.UI.Core;
using System.Net.Http;
using Windows.Storage;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;
using System.Threading.Tasks;

namespace EdgeCasesApp
{

    public sealed partial class MainPage : Page
    {

        private const int BUTTON_PIN = 5;
        private GpioPin buttonPin;

        private SerialDevice serialPort;
        private DataWriter dataWriter;

        private bool lowLevel = false;

        IStream connection;
        RemoteDevice arduino;

        public MainPage()
        {
            this.InitializeComponent();
            lowLevel = Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.Devices.DevicesLowLevelContract", 1);
            if (lowLevel)
            {
                InitGPIO();
                InitSerialCommsAsync();
            }
        }

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            if (gpio == null)
            {
                GpioStatus.Text = "There is no GPIO controller on this device.";
                return;
            }

            buttonPin = gpio.OpenPin(BUTTON_PIN);

            // Check if input pull-up resistors are supported
            if (buttonPin.IsDriveModeSupported(GpioPinDriveMode.InputPullUp))
                buttonPin.SetDriveMode(GpioPinDriveMode.InputPullUp);
            else
                buttonPin.SetDriveMode(GpioPinDriveMode.Input);

            // Set a debounce timeout to filter out switch bounce noise from a button press
            buttonPin.DebounceTimeout = TimeSpan.FromMilliseconds(50);

            // Register for the ValueChanged event so our buttonPin_ValueChanged 
            // function is called when the button is pressed
            buttonPin.ValueChanged += buttonPin_ValueChanged;

            GpioStatus.Text = "GPIO pins initialized correctly.";
        }

        private async void InitSerialCommsAsync()
        {
            string aqs = SerialDevice.GetDeviceSelector();
            var devices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(aqs, null);

            if(devices.Count > 0)
            {
                string id = devices[0].Id;
                serialPort = await SerialDevice.FromIdAsync(id);

                serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.BaudRate = 9600;
                serialPort.Parity = SerialParity.None;
                serialPort.StopBits = SerialStopBitCount.One;
                serialPort.DataBits = 8;

                dataWriter = new DataWriter(serialPort.OutputStream);

                /* FOR TEST PURPOSES ONLY - REMOVE IN PROD
                   n.b, keep all messages under 25 chars and
                   understand that all messages sent within a
                   certain time frame will be concatenated and
                   also must adhere to the 25 char limit. The
                   limit stops auto wrapping which can cause
                   an infinite loop.
                */
                WriteToSerial("Test message #1");
                //WriteToSerial("Test message #2");
                //WriteToSerial("Test message #3");

            }
            else
            {
                throw new Exception("No devices found");
            }
        }

        private async void WriteToSerial(string message)
        {
            dataWriter.WriteString(message);
            // Launch an async task to complete the write operation
            await dataWriter.StoreAsync();
        }

        private void CloseSerialComms()
        {
            if(serialPort != null)
            {
                serialPort.Dispose();
            }

            if(dataWriter != null)
            {
                dataWriter.DetachStream();
                dataWriter = null;
            }
        }

        private async void GetResultsIoT()
        {
            resultsProgressRing.IsActive = true;

            try
            {
                PlayMusic();

                RootObject edgeCase;
                edgeCase = await EdgeCaseModel.GetResultsTwilio();
                DisplayResults(edgeCase);

            }
            catch (NullReferenceException nullEx)
            {
                GpioStatus.Text = "No website URL found";
                System.Diagnostics.Debug.Write(nullEx);
            }
            catch (HttpRequestException httpEx)
            {
                GpioStatus.Text = "No internet";
                System.Diagnostics.Debug.Write(httpEx);
            }        

            resultsProgressRing.IsActive = false;
            GpioStatus.Text = "Found it!";
        }   

        private async void PlayMusic()
        {
            //MediaElement mysong = new MediaElement();
            //Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            //Windows.Storage.StorageFile file = await folder.GetFileAsync("starwars.wav");
            //var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            //mysong.SetSource(stream, file.ContentType);
            //mysong.Play();

            MediaElement PlayMusic = new MediaElement();
            PlayMusic.AudioCategory = Windows.UI.Xaml.Media.AudioCategory.Media;

            StorageFolder Folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            Folder = await Folder.GetFolderAsync("Assets");
            StorageFile sf = await Folder.GetFileAsync("starwars.wav");
            PlayMusic.SetSource(await sf.OpenAsync(FileAccessMode.Read), sf.ContentType);
            PlayMusic.Play();
        }

        private void DisplayResults(RootObject edgeCase)
        {

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

            if(lowLevel)
            {
                // Compile message to write over serial
                //var msgToSend = "";
                //WriteToSerialAsync(msgToSend);
            }

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

        //RPi / IoT device button press to get edge case results (pressing physical button)
        private void buttonPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs e)
        {
            var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { GetResultsIoT(); });
        }

        //Mobile/table/PC device button press to get edge case results (pressing the 'GO' button in the UI)
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            resultsProgressRing.IsActive = true;
            PlayMusic();

            //Getting URL from user input
            var url = TextBox_URLInput.Text;

            try
            {
                RootObject edgeCase;

                //Getting results from user inputted URL
                edgeCase = await EdgeCaseModel.GetResults(url);

                DisplayResults(edgeCase);
            }
            catch (NullReferenceException nullEx)
            {
                GpioStatus.Text = "No website URL found";
                System.Diagnostics.Debug.Write(nullEx);
            }
            catch (HttpRequestException httpEx)
            {
                GpioStatus.Text = "No internet";
                System.Diagnostics.Debug.Write(httpEx);
            }

            resultsProgressRing.IsActive = false;
            GpioStatus.Text = "Found it!";
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

