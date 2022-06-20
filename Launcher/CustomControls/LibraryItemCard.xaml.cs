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
using Launcher.Pages;

namespace Launcher.CustomControls
{
    public enum GameState
    {
        NotInstalled,
        UpdateAvaliable,
        UpToDate
    }

    /// <summary>
    /// Interaction logic for LibraryItemCard.xaml
    /// </summary>
    public partial class LibraryItemCard : UserControl
    {
        public LibraryItemCard()
        {
            InitializeComponent();
        }

        public int GameID
        {
            get { return (int)GetValue(GameIDProperty); }
            set { SetValue(GameIDProperty, value); }
        }

        public static readonly DependencyProperty GameIDProperty =
            DependencyProperty.Register("GameID", typeof(int), typeof(LibraryItemCard));

        public void UpdateVersion()
        {

            StorePage.Instance.FocusOnGame(GameID, this);
        }
        

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(LibraryItemCard));



        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(LibraryItemCard));

        public string Status
        {
            get => (string)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(LibraryItemCard));

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StorePage.Instance.FocusOnGame(GameID, this);
        }
    }
}
