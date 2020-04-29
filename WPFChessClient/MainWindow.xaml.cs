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
using WPFChessClient.Pages;

namespace WPFChessClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public enum NamePage
    {
        GamePlay,
        MainMenu
    }

    public partial class MainWindow : Window
    {
        private Dictionary<NamePage, IPageChanger> Pages;

        public MainWindow()
        {
            InitializeComponent();
            CreatePages();
            ChangePage(this, new ChangePageArgs(NamePage.MainMenu));
        }

        private void CreatePages()
        {
            Pages = new Dictionary<NamePage, IPageChanger>();
            Pages.Add(NamePage.GamePlay, new GamePlayPage());
            Pages.Add(NamePage.MainMenu, new MainMenuPage());

            foreach(KeyValuePair<NamePage, IPageChanger> page in Pages)
            {
                page.Value.PageChanged += ChangePage;
            }
        }

        private void ChangePage(object sender, ChangePageArgs e)
        {
            Pages[e.Name].Start();
            MainFrame.Navigate(Pages[e.Name]);
        }
    }
}
