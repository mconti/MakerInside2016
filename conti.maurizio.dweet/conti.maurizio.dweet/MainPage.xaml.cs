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
using Newtonsoft.Json;

// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace conti.maurizio.dweet
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        bool aggiorna = false;
        
        public MainPage()
        {
            this.InitializeComponent();

            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private async void Timer_Tick(object sender, object e)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://dweet.io");
                client.Timeout = TimeSpan.FromMilliseconds(2000);
                HttpResponseMessage response;

                if (aggiorna)
                {
                    response = await client.GetAsync($"/dweet/for/itts?temperatura={sliderTemperatura.Value}");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var ro = await response.Content.ReadAsAsync<dWeetRequest>();
                        listviewLog.Items.Add($"ho spedito {sliderTemperatura.Value}");
                    }
                }
                else
                {
                    response = await client.GetAsync($"/get/latest/dweet/for/itts");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var ro = await response.Content.ReadAsAsync<dWeetResponse>();
                        var valore = ro.with[0].content.temperatura;
                        listviewLog.Items.Add($"ho ricevuto {valore}");
                        gaugeTemperatura.Value = valore;
                        sliderTemperatura.Value = valore;
                    }
                }
                aggiorna = false;
            }
            catch (Exception err)
            {

            }
        }

        private void sliderTemperatura_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            aggiorna = true;
            timer.Start();
            //gaugeTemperatura.Value = sliderTemperatura.Value;
        }
    }

    public class Content
    {
        public int temperatura { get; set; }
    }

    public class With
    {
        public string thing { get; set; }
        public string created { get; set; }
        public Content content { get; set; }
        public string transaction { get; set; }
    }

    public class dWeetRequest
    {
        public string @this { get; set; }
        public string by { get; set; }
        public string the { get; set; }
        public With with { get; set; }
    }
    public class dWeetResponse
    {
        public string @this { get; set; }
        public string by { get; set; }
        public string the { get; set; }
        public List<With> with { get; set; }
    }
}
