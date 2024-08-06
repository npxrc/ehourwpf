using System;
using System.Net;
using System.IO;
using System.Windows;
using System.Net.Http;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Navigation;
using eHoursModernUI;

namespace eHourWPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private readonly string appDataFolder = "eHours";
        private string phpSessionId;
        private string nameOfPerson;
        private string nameOfAcademy;
        private string username;
        private string password;

        private static readonly CookieContainer _cookieContainer = new CookieContainer();
        private static readonly HttpClientHandler _handler = new HttpClientHandler
        {
            CookieContainer = _cookieContainer,
            AllowAutoRedirect = true
        };
        private static readonly HttpClient _client = new HttpClient(_handler);

        public Login()
        {
            InitializeComponent();
            uName.Text = "District Username";
            uName.Foreground = new SolidColorBrush(Colors.Gray);
            try
            {
                var (username, password) = CredentialManager.ReadCredential("eHours");
                if (!string.IsNullOrEmpty(username))
                {
                    uName.Text = username;
                    uName.Foreground= new SolidColorBrush(Colors.White);
                }
                if (!string.IsNullOrEmpty(password))
                {
                    uPass.Password = password;
                }
            }
            catch (Exception ex)
            {
                // Handle the case where no credentials are found
                Console.WriteLine($"No stored credentials found: {ex.Message}");
            }
        }

        private void uName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (uName.Text == "District Username")
            {
                uName.Text = string.Empty;
                uName.Foreground = new SolidColorBrush(Colors.White);
            }
        }
        private void uName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(uName.Text))
            {
                uName.Text = "District Username";
                uName.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private async Task<string> Post()
        {
            var values = new Dictionary<string, string>
            {
                { "uName", username },
                { "uPass", password }
            };

            var content = new FormUrlEncodedContent(values);

            // Set a User-Agent
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            // Use HTTPS
            var response = await _client.PostAsync("https://academyendorsement.olatheschools.com/loginuserstudent.php", content);
            var responseString = await response.Content.ReadAsStringAsync();

            // Access the PHPSESSID cookie
            Uri uri = new Uri("https://academyendorsement.olatheschools.com/");
            var cookies = _cookieContainer.GetCookies(uri);
            phpSessionId = cookies["PHPSESSID"]?.Value;

            return responseString;
        }
        private async Task<string> Get(string url)
        {
            if (string.IsNullOrEmpty(phpSessionId))
            {
                MessageBox.Show("PHPSESSID cookie is not set.");
                return "$$FAIL$$";
            }

            // Ensure the PHPSESSID cookie is set for the domain
            Uri uri = new Uri(url);
            _cookieContainer.Add(uri, new Cookie("PHPSESSID", phpSessionId));

            var response = await _client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        private string ReadFromFile(string filename)
        {
            try
            {
                var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                var dataPath = Path.Combine(localAppDataPath, appDataFolder);

                var filePath = Path.Combine(dataPath, filename);

                if (File.Exists(filePath))
                {
                    var textFromFile = File.ReadAllText(filePath);
                    return textFromFile;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        private void WriteToFile(string filename, string toWrite)
        {
            try
            {
                var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                var dataPath = Path.Combine(localAppDataPath, appDataFolder);

                if (!Directory.Exists(dataPath))
                {
                    Directory.CreateDirectory(dataPath);
                }

                var filePath = Path.Combine(dataPath, filename);

                var textToWrite = toWrite;
                File.WriteAllText(filePath, textToWrite);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private async void login(object sender, RoutedEventArgs evtarg)
        {
            if (uName.Text == "District Username")
            {
                MessageBox.Show("Enter your district username and password.\r\n(e.g. 408coc30)\r\nTry again after that.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            username = uName.Text;
            password = uPass.Password;
            CredentialManager.WriteCredential("eHours", username, password);
            var resp = await Post();
            resp = resp.Replace("\n", "");
            if (resp.Contains("<h2>Welcome to your"))
            {
                nameOfAcademy = resp.Split(new string[] { "<h2>Welcome to your " }, StringSplitOptions.None)[1].Split(new string[] { " Endorsement" }, StringSplitOptions.None)[0];
                nameOfPerson = resp.Split(new string[] { "Tracking, " }, StringSplitOptions.None)[1].Split(new string[] { "</h2>" }, StringSplitOptions.None)[0];
                Console.WriteLine(nameOfPerson);
                Console.WriteLine(nameOfAcademy);

                var getresp = await Get("https://academyendorsement.olatheschools.com/Student/studentEHours.php");

                // Create and show the new form, passing the necessary parameters and reference to this form
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.MainFrame.Navigate(new Home(username, password, phpSessionId, nameOfPerson, nameOfAcademy, getresp, _cookieContainer, _handler, _client));
            }
            else
            {
                MessageBox.Show("Incorrect username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /*private async void login(object sender, RoutedEventArgs e)
        {
            var resp = ReadFromFile("home.html");
            resp = resp.Replace("\n", "");
            if (resp.Contains("<h2>Welcome to your"))
            {
                nameOfAcademy = resp.Split(new string[] { "<h2>Welcome to your " }, StringSplitOptions.None)[1].Split(new string[] { " Endorsement" }, StringSplitOptions.None)[0];
                nameOfPerson = resp.Split(new string[] { "Tracking, " }, StringSplitOptions.None)[1].Split(new string[] { "</h2>" }, StringSplitOptions.None)[0];

                var getresp = ReadFromFile("ehours.html");
                WriteToFile("fuckthis.html", getresp);

                // Create and show the new form, passing the necessary parameters and reference to this form
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.MainFrame.Navigate(new Home(username, password, phpSessionId, nameOfPerson, nameOfAcademy, getresp, _cookieContainer, _handler, _client));
            }
            else
            {
                MessageBox.Show("Incorrect username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }*/
    }
}
