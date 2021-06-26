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

namespace Azumi_Browser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string Tab1URL = "";
        public static string Tab2URL = "";

        List<string> WebPages;
        int Current = 0;

        int tabCount = 0;
        TabItem CurrentTabItem = null;
        ChromiumWebBrowser currentBrowserShowing = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WebPages = new List<string>();
            GoHome();
        }

        void GoHome()
        {
            AddressBar.Text = "www.google.com";
            currentBrowserShowing.Address = "www.Google.com";
            WebPages.Add("www.google.com");
        }

        void LoadWebPages(string Link, bool addToList = true)
        {
            AddressBar.Text = Link;
            currentBrowserShowing.Address = Link;

            MenuItem items = new MenuItem();
            items.Click += MenuClicked;
            items.Header = Link;

            items.Width = 184;
            Menu.Items.Add(items);

            if (addToList)
            {
                Current++;
                WebPages.Add(Link);
            }
        }

        private void Search()
        {
            if (currentBrowserShowing != null && AddressBar.Text != string.Empty)
            {
                currentBrowserShowing.Address = "https://www.google.com/search?q=" + AddressBar.Text;
                if (AddressBar.Text.Contains("www."))
                {
                    currentBrowserShowing.Load(AddressBar.Text);
                }
            }
        }

        void ToggleWebPages(string Option)
        {
            if (Option == "→")
            {
                if ((WebPages.Count - Current - 1) != 0)
                {
                    Current++;
                    LoadWebPages(WebPages[Current], false);
                }
            }

            else
            {
                if ((WebPages.Count + Current - 1) >= WebPages.Count)
                {
                    Current--;
                    LoadWebPages(WebPages[Current], false);
                }
            }
        }

        private void Button_Click(object Sender, RoutedEventArgs e)
        {
            Button btn = (Button)Sender;
            ToggleWebPages(btn.Content.ToString());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LoadWebPages(WebPages[Current]);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            LoadWebPages(WebPages[0]);
        }

        private void MenuClicked(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            LoadWebPages(item.Header.ToString());
        }

        private void AddressBar_KeyDown(object sender, KeyEventArgs e)
        {            
                if (e.Key == Key.Enter)
                {
                    Search();
                }
        }

        public void AddressBar_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= AddressBar_GotFocus;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (WebPages.Count != 0)
            {
                Menu.PlacementTarget = hBTN;
                Menu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                Menu.HorizontalOffset = -155;
                Menu.IsOpen = true;
            }

        }

        private void Button_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void AddTab_Click(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = new TabItem();
            ChromiumWebBrowser browser = new ChromiumWebBrowser();
            browser.Name = "Browser_" + tabCount;
            tabControl.Items.Add(tabItem);
            tabItem.Name = "tab_" + tabCount;
            tabCount++;
            tabItem.Content = browser;
            browser.Address = "https://www.google.com";

            tabItem.Header = "Tab";
            tabControl.SelectedItem = tabItem;
            CurrentTabItem = tabItem;



            currentBrowserShowing = browser;
            browser.Loaded += FinishedLoadingWebPage;


        }

        private void FinishedLoadingWebPage(object sender, RoutedEventArgs e)
        {
            var sndr = sender as ChromiumWebBrowser;

            if (CurrentTabItem != null)
            {
                string RemoveHttp = sndr.Address.Replace("http://www.", "");
                string host = RemoveHttp.Replace("https://www.", "");
                CurrentTabItem.Header = host;
            }

        }

        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedItem != null)
            {
                CurrentTabItem = tabControl.SelectedItem as TabItem;
            }

            if (CurrentTabItem != null)
            {
                currentBrowserShowing = CurrentTabItem.Content as ChromiumWebBrowser;
            }
            AddressBar.Text = currentBrowserShowing.Address;
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            tabControl.Items.Remove(CurrentTabItem);
            if (tabCount < 1)
            {
                Close();
            }
        }
    }
}
