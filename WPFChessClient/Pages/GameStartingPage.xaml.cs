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
using WPFChessClient.Logic;
using WPFChessClient.Saving;
using static WPFChessClient.Pages.GamePlayPage;

namespace WPFChessClient.Pages
{
    /// <summary>
    /// Логика взаимодействия для GameStartingPage.xaml
    /// </summary>
    public partial class GameStartingPage : Page, IPageChanger
    {
        private string FirstPlayerName;
        private string SecondPlayerName;

        private int Time;


        public GameStartingPage()
        {
            InitializeComponent();
        }

        public event EventHandler<IPageArgs> PageChanged;

        public void Start()
        {
            
        }

        private void FirstPlayerNameTexBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox firstNameBox = (TextBox)sender;
            FirstPlayerName = firstNameBox.Text;
        }

        private void SecondPlayerNameTexBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox secondNameBox = (TextBox)sender;
            SecondPlayerName = secondNameBox.Text;
        }

        private void TimeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox box = (TextBox)sender;
            int time;
            if (!int.TryParse(box.Text + e.Text, out time) || time > 30 || time < 1)
                e.Handled = true;
            Time = time;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PageChanged.Invoke(this, new ChangePageToGameArgs(NamePage.GamePlay, FirstPlayerName, SecondPlayerName, Time));
            FirstPlayerNameTexBox.Clear();
            SecondPlayerNameTexBox.Clear();
            TimeTextBox.Clear();
        }

        private void TimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SecondPlayerNameTexBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void FirstPlayerNameTexBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
