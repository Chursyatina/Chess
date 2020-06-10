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
    public enum NamePage
    {
        GamePlay,
        MainMenu,
        StartigGamePage,
        LeaderTablePage,
        AboutPage,
        HelpPage,
        UnendedGameStatringPage
    }

    public partial class MainWindow : Window
    {
        private Dictionary<NamePage, IPageChanger> Pages;

        private static MainWindow Instance;

        public MainWindow()
        {
            InitializeComponent();
            CreatePages();
            ChangePage(this, new ChangePageArgs(NamePage.MainMenu));
            RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
            Instance = this;
        }

        public static MainWindow GetInstance()
        {
            return Instance;
        }

        private void CreatePages()
        {
            Pages = new Dictionary<NamePage, IPageChanger>();
            Pages.Add(NamePage.GamePlay, new GamePlayPage());
            Pages.Add(NamePage.MainMenu, new MainMenuPage());
            Pages.Add(NamePage.StartigGamePage, new GameStartingPage());
            Pages.Add(NamePage.LeaderTablePage, new LeaderTablePage());
            Pages.Add(NamePage.AboutPage, new AboutPage());
            Pages.Add(NamePage.HelpPage, new HelpPage());
            Pages.Add(NamePage.UnendedGameStatringPage, new UnendedGameStartingPage());

            foreach(KeyValuePair<NamePage, IPageChanger> page in Pages)
            {
                page.Value.PageChanged += ChangePage;
            }
        }

        private void ChangePage(object sender, IPageArgs e)
        {
            if (e is ChangePageToGameArgs && Pages[e.Name] is GamePlayPage)
            {
                ((GamePlayPage)Pages[e.Name]).SetData(((ChangePageToGameArgs)e).FirstPlayerName, ((ChangePageToGameArgs)e).SecondPlayerName, ((ChangePageToGameArgs)e).GameTime);
            }

            if (e is ChangePageToUnendedGameArgs && Pages[e.Name] is GamePlayPage)
            {
                ((GamePlayPage)Pages[e.Name]).SetUnendedGameData(((ChangePageToUnendedGameArgs)e).FirstPlayerName, ((ChangePageToUnendedGameArgs)e).SecondPlayerName, ((ChangePageToUnendedGameArgs)e).FirstPlayer, ((ChangePageToUnendedGameArgs)e).SecondPlayer, ((ChangePageToUnendedGameArgs)e).Board, ((ChangePageToUnendedGameArgs)e).CurrentPlayer);
            }
            Pages[e.Name].Start();
            MainFrame.Navigate(Pages[e.Name]);
        }
    }
}
