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
    public partial class RegisterAccountPage : Page
    {
        public RegisterAccountPage()
        {
            InitializeComponent();

            PasswordError.Visibility = Visibility.Hidden;
            UsernameError.Visibility = Visibility.Hidden;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            //check that fields have values
            if(UsernameField.Text == "")
            {
                UsernameError.Text = "You Need A Username";
                UsernameError.Visibility = Visibility.Visible;
                return;
            }

            //check that fields have values
            if (PasswordField.Password == "")
            {
                PasswordError.Text = "You Need A Password";
                PasswordError.Visibility = Visibility.Visible;
                return;
            }

            if (UsernameField.Text.Contains(',') || UsernameField.Text.Contains(';')
                || PasswordField.Password.Contains(',') || PasswordField.Password.Contains(';'))
            {
                UsernameError.Text = "Username And Password Cant Contain ',' or ';'";
                UsernameError.Visibility = Visibility.Visible;
                return;
            }

            // check password match
            if (PasswordField.Password != PasswordField2.Password)
            {
                PasswordError.Text = "Passwords Dont Match";

                PasswordError.Visibility = Visibility.Visible;
                return;
            }

            // find if username already exists
            if (((MainWindow)Application.Current.MainWindow).gameFile.Contains(UsernameField.Text))
            {
                UsernameError.Text = "Username Already Taken";
                UsernameError.Visibility = Visibility.Visible;
                return;
            }

            ((MainWindow)Application.Current.MainWindow).gameFile = PasswordField.Password.ToString() +
                "," +
                UsernameField.Text +
                "," +
                ((MainWindow)Application.Current.MainWindow).gameFile;

            ((MainWindow)Application.Current.MainWindow).SaveFile();

            AwsUploader uploader = new AwsUploader();
            uploader.UploadStatic();

            if (Application.Current.MainWindow != null)
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Uri("Pages/SignInPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow != null)
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new Uri("Pages/SignInPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
