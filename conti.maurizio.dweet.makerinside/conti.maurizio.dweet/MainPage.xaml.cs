using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
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

using GrovePi.Sensors;
using GrovePi;

// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace conti.maurizio.dweet
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        public MainPage()
        {
            this.InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(300);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            var grove = GrovePi.DeviceFactory.Build;
            IDHTTemperatureAndHumiditySensor sensore = grove.DHTTemperatureAndHumiditySensor(Pin.DigitalPin3, DHTModel.Dht22);
            sensore.Measure();
            // qui puoi leggere umidita e temperatura;
            // ...

            IRotaryAngleSensor rotaryAngleSensor = grove.RotaryAngleSensor(Pin.AnalogPin2);
            mioGauge.Value = rotaryAngleSensor.SensorValue();
        }

        private async void mioSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            // https://dweet.io/dweet/for/MakerInside?red=0&green=000&blue=255


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.dweet.io");
            await client.GetAsync($"/dweet/for/MakerInside?red={mioSlider.Value}&green=000&blue=0");

            HttpResponseMessage response = await client.GetAsync($"/get/latest/dweet/for/MakerInside");
            // https://dweet.io/get/latest/dweet/for/MakerInside
            if ( response.IsSuccessStatusCode )
            {
                RootObject ms = await response.Content.ReadAsAsync<RootObject>();
                try
                {
                    int rosso = ms.with[0].content.red;
                    //mioGauge.Value = rosso;
                }
                catch{ }
          }
        }
    }


    public class Content
    {
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }
    }

    public class With
    {
        public string thing { get; set; }
        public string created { get; set; }
        public Content content { get; set; }
    }

    public class RootObject
    {
        public string @this { get; set; }
        public string by { get; set; }
        public string the { get; set; }
        public List<With> with { get; set; }
    }


}
