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
using WPFChessClient.Saving;

namespace WPFChessClient.Pages
{
    /// <summary>
    /// Логика взаимодействия для LeaderTablePage.xaml
    /// </summary>
    public partial class LeaderTablePage : Page, IPageChanger
    {

        private static int SortPlayersByWins(LeaderTablePlayer x, LeaderTablePlayer y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal.
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater.
                    return 1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return -1;
                }
                else
                {
                    // ...and y is not null, compare the
                    // lengths of the two strings.
                    //
                    if (x.Wins > y.Wins) return -1;
                    else
                    {
                        if (y.Wins > x.Wins) return 1;
                        else
                        {
                            if (x.Wins == y.Wins)
                            {
                                if (x.Games > y.Games) return 1;
                                else return -1;
                            }
                            else return -1;
                        }
                    }
                }
            }
        }


        private Saver Saver;
        private List<LeaderTablePlayer> Leaders;
        public LeaderTablePage()
        {
            InitializeComponent();
            FillLeaderTable();
        }

        public event EventHandler<IPageArgs> PageChanged;

        public void Start()
        {

        }

        private void FillLeaderTable()
        {
            Saver = new Saver();
            Leaders = Saver.GetLeaderTable();

            Leaders.Sort(SortPlayersByWins);
            
            for (int i = 0; i < Leaders.Count; i++)
            {
                if (i == 0)
                {
                    NameFirst.Text = Leaders[i].Name;
                    GamesFirst.Text = Leaders[i].Games.ToString();
                    WinsFirst.Text = Leaders[i].Wins.ToString();
                }
                if (i == 1)
                {
                    NameSecond.Text = Leaders[i].Name;
                    GemesSecond.Text = Leaders[i].Games.ToString();
                    WinsSecond.Text = Leaders[i].Wins.ToString();
                }
                if (i == 2)
                {
                    NameThird.Text = Leaders[i].Name;
                    GemesThird.Text = Leaders[i].Games.ToString();
                    WinsThird.Text = Leaders[i].Wins.ToString();
                }
                if (i == 3)
                {
                    NameFourth.Text = Leaders[i].Name;
                    GamesFourth.Text = Leaders[i].Games.ToString();
                    WinsFourth.Text = Leaders[i].Wins.ToString();
                }
                if (i == 4)
                {
                    NameFifth.Text = Leaders[i].Name;
                    GemesFifth.Text = Leaders[i].Games.ToString();
                    WinsFifth.Text = Leaders[i].Wins.ToString();
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            PageChanged.Invoke(this, new ChangePageArgs(NamePage.MainMenu));
        }
    }
}
