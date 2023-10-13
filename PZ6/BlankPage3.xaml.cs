using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace PZ6
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class BlankPage3 : Page
    {
        public BlankPage3()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
                textBlock1.Text = e.Parameter.ToString();
        }

        private void Forward_Clickmain(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), "переход из третьего окна");
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPage1), "переход из третьего окна");
        }

        private void Forwardtwo_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPage2), "переход из третьего окна");
        }
    }
}
