using GrovePi;
using GrovePi.Sensors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace App1
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly IBuildGroveDevices _deviceFactory = DeviceFactory.Build;

        ILed led ;
        IRotaryAngleSensor rotaryAngleSensor;
        int maxValue;

        DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();

            led = _deviceFactory.Led(Pin.DigitalPin5);
            rotaryAngleSensor = _deviceFactory.RotaryAngleSensor(Pin.AnalogPin2);
            maxValue = 255;
        }

        private void Timer_Tick(object sender, object e)
        {
            try
            {
                var sensorValue = rotaryAngleSensor.SensorValue();
                led.AnalogWrite((byte)(sensorValue/4));
                gVal1.Value = sensorValue;
                gVal2.Value = 1023-sensorValue;
            }
            catch (Exception){ }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
    }
}
