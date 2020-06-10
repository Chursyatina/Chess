using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для UnendedGameStartingPage.xaml
    /// </summary>
    public partial class UnendedGameStartingPage : Page, IPageChanger
    {

        private string SelectedFile;
        private ListBox listBox;

        public UnendedGameStartingPage()
        {
            InitializeComponent();
        }

        public event EventHandler<IPageArgs> PageChanged;

        public void Start()
        {
            
        }

        private void ResumeMatch_Click(object sender, RoutedEventArgs e)
        {
            Saver saver = new Saver();
            ChangePageToUnendedGameArgs args = saver.DowloadUnendedGame(SelectedFile);
            args.Name = NamePage.GamePlay;
            PageChanged.Invoke(this, args);
        }

        private void Files_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedValue == null) return;
            SelectedFile = (string)listBox.SelectedValue;
        }

        private void FillList()
        {
            Saver saver = new Saver();
            List<string> names = saver.GetUnendedGamesList();

            Files.ItemsSource = names;

            listBox.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PageChanged.Invoke(this, new ChangePageArgs(NamePage.MainMenu));
        }

        private void Files_Loaded(object sender, RoutedEventArgs e)
        {
            listBox = sender as ListBox;
            FillList();
        }
    }
}
