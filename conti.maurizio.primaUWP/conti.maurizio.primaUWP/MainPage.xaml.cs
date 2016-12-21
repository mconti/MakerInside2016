using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace conti.maurizio.primaUWP
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
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            timer.Stop();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://dweet.io");
            client.GetStringAsync($"dweet/for/itts5h?slider={sliderValore.Value}");

            webView.Navigate(new Uri("https://freeboard.io/board/Zo0rcA"));
        }

        private void sliderValore_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            timer.Start();
        }
    }
}
