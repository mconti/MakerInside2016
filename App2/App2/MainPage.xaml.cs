using GrovePi;
using GrovePi.I2CDevices;
using GrovePi.Sensors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace App2
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly IBuildGroveDevices _deviceFactory = DeviceFactory.Build;

        ILed led;
        IRotaryAngleSensor rotaryAngleSensor;
        IRgbLcdDisplay display;
        IDHTTemperatureAndHumiditySensor humidity;

        DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();

            led = _deviceFactory.Led(Pin.DigitalPin5);
            rotaryAngleSensor = _deviceFactory.RotaryAngleSensor(Pin.AnalogPin2);
            display = _deviceFactory.RgbLcdDisplay();
            display.SetBacklightRgb(255, 0, 0);

            humidity = _deviceFactory.DHTTemperatureAndHumiditySensor(Pin.DigitalPin3, DHTModel.Dht22);
        }

        private void Timer_Tick(object sender, object e)
        {
            try
            {
                var sliderValue = rotaryAngleSensor.SensorValue();
                gVal1.Value = sliderValue;

                var normalizedSensorvalue = (byte)(sliderValue / 4);
                led.AnalogWrite(normalizedSensorvalue);

                var humidityValue = humidity.Humidity;
                var temperatureValue = humidity.TemperatureInCelsius;

                gVal2.Value = temperatureValue;
                gVal3.Value = humidityValue;

                //string msg = "Temp:" + humidityValue.ToString() + " Hum:" + temperatureValue.ToString();
                string msg = string.Format("T{0:N1} H{1:N1}%\n{2:N1}", temperatureValue, humidityValue, normalizedSensorvalue);
                display.SetText(msg);
                display.SetBacklightRgb(normalizedSensorvalue, 100, normalizedSensorvalue);

                humidity.Measure();
                Task.Delay(10);
            }
            catch (Exception err) { Debug.WriteLine(err); }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(150);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
    }
}
