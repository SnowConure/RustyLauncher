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
using Launcher.Converters;

namespace Launcher.Pages
{
    /// <summary>
    /// Interaction logic for SignInPage.xaml
    /// </summary>
    public partial class SignInPage : Page
    {
        public SignInPage()
        {
            InitializeComponent();
            ((MainWindow)Application.Current.MainWindow).GetData();
        }

        private void Login_Button(object sender, RoutedEventArgs e)
        {
            // find username
            string[] credentials = ((MainWindow)Application.Current.MainWindow).ReadFile(0);

            for (int i = 0; i < credentials.Length; i++)
            {
                //found username
                if(username.Text == credentials[i] && i% 2 != 0)
                {

                    // compare to password
                    if (password.Password.ToString() == credentials[i-1])
                    {
                        // Corrent Password
                        // Log In
                        ((MainWindow)Application.Current.MainWindow).user = credentials[i];


                        if (Application.Current.MainWindow != null)
                            ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Uri("Pages/PrepareingToLaunchStore.xaml", UriKind.RelativeOrAbsolute));
                        return;
                    }
                }
            }

            loginError.Text = "Invalid Credentials!";
            loginError.Visibility = Visibility.Visible;
        }



        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
           /* AwsUploader downloader = new AwsUploader();
            downloader.UploadStatic();*/
            if (Application.Current.MainWindow != null)
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Uri("Pages/RegisterAccountPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
