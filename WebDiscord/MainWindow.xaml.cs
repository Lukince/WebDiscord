using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp;
using CefSharp.Wpf;

namespace WebDiscord
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight - 13;

            InitBrowser("https://discord.com/app");

            browser.TitleChanged += (sender, e) =>
            {
                var s = (string)e.NewValue;

                if (s != "Discord")
                    s += " - Discord";

                Title = s;
            };

            StateChanged += (sender, e) =>
            {
                if (WindowState == WindowState.Normal)
                {
                    ResizeMode = ResizeMode.CanResize;
                    MainChrome.ResizeBorderThickness = new Thickness(4);
                }
                else if (WindowState == WindowState.Maximized)
                {
                    ResizeMode = ResizeMode.NoResize;
                    MainChrome.ResizeBorderThickness = new Thickness(0);
                }
            };
        }

        ChromiumWebBrowser browser;

        public void InitBrowser(string starturl)
        {
            if (!System.IO.Directory.Exists("cache"))
                System.IO.Directory.CreateDirectory("cache");

            CefSettings set = new CefSettings();
            set.CefCommandLineArgs.Add("disable-usb-keyboard-detect", "1");
            set.CachePath = System.IO.Path.GetFullPath("cache/");
            Cef.Initialize(set);

            browser = new ChromiumWebBrowser();
            browser.Address = starturl;
            Grid.SetRow(browser, 1);

            MainGrid.Children.Add(browser);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                ResizeMode = ResizeMode.CanResize;
                MainChrome.ResizeBorderThickness = new Thickness(4);
                WindowState = WindowState.Normal;
            }
            else
            {
                ResizeMode = ResizeMode.NoResize;
                MainChrome.ResizeBorderThickness = new Thickness(0);
                WindowState = WindowState.Maximized;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
