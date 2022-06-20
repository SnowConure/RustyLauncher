using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.IO;
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
using Launcher.CustomControls;


namespace Launcher.Pages
{

    /// <summary>
    /// Interaction logic for StorePage.xaml
    /// </summary>
    public partial class StorePage : Page
    {


        public static StorePage Instance;

        public int focusedGame;
        public GameState focusedGameState;
        public LibraryItemCard focusedLibraryItemCard;

        public StorePage()
        {
            InitializeComponent();
            Instance = this;

            Username.Text = ((MainWindow)Application.Current.MainWindow).user;

            if (((MainWindow)Application.Current.MainWindow).user == "Admin")
            {
                AdminGrid.Visibility = Visibility.Visible;
                AdminConsole.Text = MainWindow.Decrypt(((MainWindow)Application.Current.MainWindow).ReadFullFile());
            }

            Game1.Status = CheckVersion(1, Game1);
            Game2.Status = CheckVersion(2, Game2);
            Game3.Status = CheckVersion(3, Game3);
            Game4.Status = CheckVersion(4, Game4);
            Game5.Status = CheckVersion(5, Game5);
        }

        public string CheckVersion(int GameID, LibraryItemCard _gameCard)
        {
            string[] gameStats = ((MainWindow)Application.Current.MainWindow).ReadFile(GameID);
            string[] localStats = ((MainWindow)Application.Current.MainWindow).ReadLocalFile();

            if (localStats[GameID] == "NotInst")
            {
                focusedGameState = GameState.NotInstalled;
                _gameCard.Status = "Not Installed";
                UninstallButton.Visibility = Visibility.Collapsed;

                return "Install";
            }
            else if (gameStats[1] == localStats[0])
            {
                focusedGameState = GameState.UpToDate;
                _gameCard.Status = "Version: " + gameStats[1];
                UninstallButton.Visibility = Visibility.Visible;
                return "Launch";
            }
            else
            {
                focusedGameState = GameState.UpdateAvaliable;
                _gameCard.Status = "Update Avaliable";
                UninstallButton.Visibility = Visibility.Visible;

                return "Update";
            }

        }

        public void FocusOnGame(int gameID, LibraryItemCard _gameCard)
        {
            focusedLibraryItemCard = _gameCard;

            GameFocusBar.Visibility = Visibility.Hidden;

            FocusOnGame(gameID);
        }

        public void FocusOnGame(int gameID)
        {
            focusedGame = gameID;
            LaunchButton.Visibility = Visibility.Visible;
            CheckVersion(gameID, focusedLibraryItemCard);

            GameFocusBar.Visibility = Visibility.Visible;

            LaunchButton.Content = CheckVersion(gameID, focusedLibraryItemCard);

            

            string[] gameVars = ((MainWindow)Application.Current.MainWindow).ReadFile(gameID);

            GameDescription.Text = gameVars[4];

            GameTitle.Text = gameVars[0];

            PreviewScreen.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Screenshot" + gameID + ".png"));
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            if (focusedGameState == GameState.UpdateAvaliable)
            {
                ((MainWindow)Application.Current.MainWindow).DownLoad(focusedGame);
                DownloadProgress.Visibility = Visibility.Visible;
                LaunchButton.Content = "";

            }
            else if (focusedGameState == GameState.NotInstalled)
            {
                ((MainWindow)Application.Current.MainWindow).InstallNewGame(focusedGame);
                DownloadProgress.Visibility = Visibility.Visible;
                LaunchButton.Content = "";
            }
            else
            {
                ((MainWindow)Application.Current.MainWindow).LaunchGame(focusedGame);
            }
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).UpdateStaticAdmin(AdminConsole.Text);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow != null)
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Uri("Pages/SignInPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void UninstallButton_Click(object sender, RoutedEventArgs e)
        {
            Directory.Delete(Path.Combine(((MainWindow)Application.Current.MainWindow).rootPath, "Games", ((MainWindow)Application.Current.MainWindow).ReadFile(focusedGame)[0]), true);
            ((MainWindow)Application.Current.MainWindow).UpdateLocalFileVariable(focusedGame, "NotInst");
            DownloadProgress.Visibility = Visibility.Collapsed;
            UninstallButton.Visibility = Visibility.Collapsed;

            LaunchButton.Content = "Install";

        }
    }
}
