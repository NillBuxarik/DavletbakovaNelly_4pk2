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
    public sealed partial class BlankPage1 : Page
    {
        public BlankPage1()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
                textBlock1.Text = e.Parameter.ToString();
        }


        private void Forwardmain_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), "переход из первого окна");
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPage2), "переход из первого окна");
        }

        private void Forwardtree_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPage3), "переход из первого окна"); 
        }
    }
}
