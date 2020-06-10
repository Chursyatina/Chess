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
using WPFChessClient.EventArgsClasses;
using WPFChessClient.Interfaces;

namespace WPFChessClient.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page, IPageChanger
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        private MainWindow MainWindow;

        public event EventHandler<IPageArgs> PageChanged;

        public void Start()
        {
           
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow = MainWindow.GetInstance();
            MainWindow.Close();
        }

        private void StartOnePlayerGame_Click(object sender, RoutedEventArgs e)
        {
            PageChanged.Invoke(this, new ChangePageArgs(NamePage.StartigGamePage));
        }

        private void LeaderTable_Click(object sender, RoutedEventArgs e)
        {
            PageChanged.Invoke(this, new ChangePageArgs(NamePage.LeaderTablePage));
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            PageChanged.Invoke(this, new ChangePageArgs(NamePage.AboutPage));
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            PageChanged.Invoke(this, new ChangePageArgs(NamePage.HelpPage));
        }

        private void DownloadMatch_Click(object sender, RoutedEventArgs e)
        {
            PageChanged.Invoke(this, new ChangePageArgs(NamePage.UnendedGameStatringPage));
        }
    }
}
