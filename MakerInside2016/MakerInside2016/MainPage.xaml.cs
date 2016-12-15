using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

namespace MakerInside2016
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private async void Timer_Tick(object sender, object e)
        {
            timer.Stop();

            HttpResponseMessage response = await SendData(sliderTemperatura.Value);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                listviewLog.Items.Add($"Spedito: {sliderTemperatura.Value}");
        }

        private void sliderTemperatura_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            timer.Start();
        }

        private async Task<HttpResponseMessage> SendData(double valore)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(@"https://dweet.io/");
            HttpResponseMessage response = await client.GetAsync($"dweet/for/MakerInside?temperatura={valore}");
            return response;
        }

    }
}
