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

        public event EventHandler<ChangePageArgs> PageChanged;

        public void Start()
        {
           
        }

        private void StartGamePlay_Click(object sender, RoutedEventArgs e)
        {
            PageChanged.Invoke(this, new ChangePageArgs(NamePage.GamePlay));
        }

        private void StartMultiplayerGame_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
