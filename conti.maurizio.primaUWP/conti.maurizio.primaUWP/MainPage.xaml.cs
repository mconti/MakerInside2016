using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace conti.maurizio.primaUWP
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string _webViewUrl = "https://freeboard.io/board/Zo0rcA";

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async Task SetSliderValue(double value)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://dweet.io");
            await client.GetStringAsync($"dweet/for/itts5h?slider={sliderValore.Value}");
        }

        private async void sliderValore_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(200));
            await this.SetSliderValue(e.NewValue);
        }

        /// <summary>
        /// Al caricamento completo della pagina
        /// </summary>
        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            webView.Navigate(new Uri(_webViewUrl));
        }
    }
}
