using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using Launcher.Converters;
using Launcher.CustomControls;
using Launcher.Pages;
using Squirrel;
using System.Threading.Tasks;

namespace Launcher
{
    public enum LauncherStatus
    {
        ready,
        downloading
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string rootPath;
        public string gameFile;
        public string localFile;
        public string user;
        public LauncherStatus launcherStatus = LauncherStatus.ready;

        public UpdateManager updateManager;

        int newGameID;
        string[] newGame;

        static readonly HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            rootPath = Directory.GetCurrentDirectory();

            if (!File.Exists("Local.txt"))
            {

                File.Create("Local.txt").Dispose();

                //This will save some text to a file in the same folder as your project exe file
                using (StreamWriter sw = File.CreateText("Local.txt"))
                {
                    sw.Write("NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;NotInst;");
                }

            }
            if (!File.Exists("Static.txt"))
            {
                File.Create(rootPath + "Static.txt").Dispose();

                //This will save some text to a file in the same folder as your project exe file
                using (StreamWriter sw = File.CreateText("Static.txt"))
                {
                    sw.Write("889864,Admin,1,q1,;Far Locked,0.1,https://drive.google.com/uc?export=download&id=1borpGcX4khtCuL21IcabFOZVGBlTsb0E&confirm=y,Far_Locked.exe,The game the one and only is currently working on,Temp,Temp,Temp,Temp,LAUNCHER UPDATE,1.0,;Project Shelter Remake,1.0,https://drive.google.com/uc?export=download&id=1BEphfH-y9kv9xSqSVOlDpqH6ZuTEvpao&confirm=y,Project Shelter Remake.exe,A Super short demo for a remake project for our first game,;Kowalski Out Walking,1.0,https://drive.google.com/uc?export=download&id=14k4QwQzlFqURyZUJglfSVH9zxJA5bGzU&confirm=y,GameJam2022.exe,A short puzzle game made for Mini Game Jam 2022,;Original Shelter,1.0,https://drive.google.com/uc?export=download&id=1qZqcQKtUU8IJxxLcMmfMJWvAp9GzZApq&confirm=y,ProjectShelter.exe,Original Build of Project: Shelter out first game ever made,;OneTryMurderMystery,1.0,https://drive.google.com/uc?export=download&id=1xtPWe17u6LB-X1_c0n_ssxizKxaXIMo6&confirm=y,One try murder mystery.exe,Short Interactive novel made in 2 days,;Rapid Smash,0.5,https://drive.google.com/uc?export=download&id=1cdPNS2cBXJZ8Qnm1IlCDRP726T0xcvUe&confirm=y,Rapid Rabid Lethal Smash.exe,A Smash like game with characters from different games idk bro. shit is not working for shit but you know ankan kan springa runt");
                }

            }

            localFile = ReadFullLocalFile();

           /* AwsUploader downloader = new AwsUploader();
            downloader.DownloadStatic();*/



            // first time setup games folder
            if(!Directory.Exists(rootPath + "/Games"))
                Directory.CreateDirectory(rootPath + "/Games");
           

        }

        #region Squirrel

        public async void MainWindow_Loaded()
        {
            //if(!Debugger.IsAttached)
            updateManager = await UpdateManager.GitHubUpdateManager(@"https://github.com/SnowConure/RustyLauncher");

            //  updateManager.UpdateApp();

            // updateManager.CurrentlyInstalledVersion();

            CheckForUpdates();
        }

        private async void CheckForUpdates()
        {
            //Check for update
            var updateInfo = await updateManager.CheckForUpdate();

            if (updateInfo.ReleasesToApply.Count > 0)
            {
                MainFrame.Navigate(new Uri("Pages/UpdateLauncherPage.xaml", UriKind.RelativeOrAbsolute));
            } else
            {
                MainFrame.Navigate(new Uri("Pages/SignInPage.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        public async void UpdateLauncher()
        {
            await updateManager.UpdateApp();

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        #endregion

        #region web download
        public void InstallNewGame(int gameID) 
        {
            newGame = ReadFile(gameID);
            // create folder
            Directory.CreateDirectory(rootPath + "/Games" + "/" + newGame[0]);

            DownLoad(gameID);
        }

        public void DownLoad(int gameID)
        {
            launcherStatus = LauncherStatus.downloading;

            newGameID = gameID;
            //get game vars from server
            newGame = ReadFile(newGameID);

            //UpdateStaticAdmin();
            try
            {
               //  new Thread(() =>
               //  {
                      WebClient webClient = new WebClient();
                    // if first time installing game

                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadGameCompletedCallback);
                    webClient.DownloadFileAsync(new Uri(newGame[2]), Path.Combine(rootPath, "Games", newGame[0], newGame[0] + ".zip"));
                //  }).Start();
            }
            catch (Exception ex)
            {
            }

            
        }

        public void DownloadGameCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            ZipFile.ExtractToDirectory(Path.Combine(rootPath, "Games", newGame[0], newGame[0] + ".zip"),
                Path.Combine(rootPath, "Games", newGame[0]),
                true);
            File.Delete(Path.Combine(rootPath, "Games", newGame[0], newGame[0] + ".zip"));


            //update local file to correct version
            UpdateLocalFileVariable(newGameID, newGame[1]);

            launcherStatus = LauncherStatus.ready;

            StorePage.Instance.DownloadProgress.Visibility = Visibility.Collapsed;
            StorePage.Instance.FocusOnGame(newGameID);
            StorePage.Instance.focusedGameState = GameState.UpToDate;
            StorePage.Instance.LaunchButton.Content = "Launch";

        }

        public void LaunchGame(int gameID)
        {
            string path = Path.Combine(rootPath, "Games", ReadFile(gameID)[0]);
            if (File.Exists(Path.Combine(path, ReadFile(gameID)[3])) && launcherStatus == LauncherStatus.ready)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(path, ReadFile(gameID)[3]));
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = path;
                Process.Start(startInfo);

                //Close();
            }
        }

        #endregion

        #region File Management
        public void GetData()
        {
            
            gameFile = Decrypt(ReadFullFile());

        }

        public void UpdateGameFileVariable(int _game, int _varIndex, string _newVar)
        {
            string[] gameList = gameFile.Split(";");

            string finalString = "";

            for (int i = 0; i < gameList.Length; i++)
            {
                string[] gameVars = gameList[i].Split(",");
                for (int j = 0; j < gameVars.Length; j++)
                {
                    //At the right variable
                    if (i == _game && j == _varIndex)
                    {
                        gameVars[j] = _newVar;
                    }

                    //stitch Together again
                    finalString += gameVars[j] + ",";
                }
                finalString += ";";
            }

            gameFile = finalString;
            SaveFile();
        }

        public void UpdateStaticAdmin(string text)
        {
            gameFile = text;

            SaveFile();

            /*AwsUploader uploader = new AwsUploader();
            uploader.UploadStatic();*/
        }

        public void UpdateLocalFileVariable(int gameID, string _newVersion)
        {
            string[] localList = localFile.Split(";");

            string finalString = "";

            for (int i = 0; i < localList.Length; i++)
            {
                    //At the right variable
                    if (i == gameID)
                    {
                        localList[i] = _newVersion;
                    }

                    finalString += localList[i] + ";";
            }

            localFile = finalString;
            SaveLocalFile();
        }

        public void SaveFile()
        {

            //This will save some text to a file in the same folder as your project exe file
            using (StreamWriter sw = File.CreateText("Static.txt"))
            {
                sw.Write(Encrypt(gameFile));
            }
        }

        public void SaveLocalFile()
        {

            //This will save some text to a file in the same folder as your project exe file
            using (StreamWriter sw = File.CreateText("Local.txt"))
            {
                sw.Write(localFile);
            }
        }
            
        public string[] ReadLocalFile()
        {
            string fileContents;
            using (StreamReader sr = File.OpenText("Local.txt"))
            {
                fileContents = sr.ReadToEnd();
            }

            string[] gameInfo = fileContents.Split(";");

            return gameInfo;
        }

        private string ReadFullLocalFile()
        {

            string fileContents;
            using (StreamReader sr = File.OpenText("Local.txt"))
            {
                fileContents = sr.ReadToEnd();
            }
            return fileContents;
        }

        public string ReadFullFile()
        {

         

            string fileContents;
            using (StreamReader sr = File.OpenText("Static.txt"))
            {
                fileContents = sr.ReadToEnd();
            }
            return fileContents;
        }

        // ((MainWindow)Application.Current.MainWindow).ReadFile();
        public string[] ReadFile(int _gameIndex)
        {
            string[] gameList = gameFile.Split(";");

            string[] gameVars = gameList[_gameIndex].Split(",");

            return gameVars;
        }

        // Function to return the modified String
        public static string Encrypt(string text)
        {
            return text;

            var textBytes = System.Text.Encoding.ASCII.GetBytes(text);
            return System.Convert.ToBase64String(textBytes);
        }

        public static string Decrypt(string text)
        {
            return text;
            var hex = System.Convert.FromBase64String(text);
            return System.Text.Encoding.ASCII.GetString(hex);
        }

        #endregion
        public void BringForward()
        {
            // Brings this app back to the foreground.
            this.Activate();
        }
    }

}
