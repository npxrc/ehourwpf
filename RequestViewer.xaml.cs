using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using HtmlAgilityPack;

namespace eHourWPF
{
    public partial class RequestViewer : Page
    {
        private string appDataFolder = "eHours";
        private bool canDelete = false;
        private string id;
        private string phpSessionId;
        private string eventName;
        private CookieContainer _cookieContainer;
        private HttpClientHandler _handler;
        private HttpClient _client;

        private HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

        public RequestViewer(string id, string phpSessionId, string eventName, CookieContainer cookieContainer, HttpClientHandler handler, HttpClient client)
        {
            InitializeComponent();

            this.id = id;
            this.phpSessionId = phpSessionId;
            this.eventName = eventName.Split('\n')[0];
            this._cookieContainer = cookieContainer;
            this._handler = handler;
            this._client = client;

            this.Loaded += RequestViewer_Loaded;
            this.Title = $"Request Viewer - {this.eventName}";
        }

        private async void RequestViewer_Loaded(object sender, RoutedEventArgs e)
        {
            await PostAsync();
        }

        private async Task PostAsync()
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "ehours_request_descr", id }
                };

                var content = new FormUrlEncodedContent(values);

                Uri uri = new Uri("https://academyendorsement.olatheschools.com/");
                _cookieContainer.Add(uri, new Cookie("PHPSESSID", phpSessionId));

                if (!_client.DefaultRequestHeaders.Contains("User-Agent"))
                {
                    _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                }

                var response = await _client.PostAsync("https://academyendorsement.olatheschools.com/Student/eHourDescription.php", content);
                var responseString = await response.Content.ReadAsStringAsync();

                WriteToFile("gettingtheevent.txt", responseString);

                doc.LoadHtml(responseString);
                deleteRequestButton.Visibility = Visibility.Collapsed;
                if (doc.DocumentNode.SelectSingleNode("//*[@id='Delete']").InnerHtml.Length > 1)
                {
                    deleteRequestButton.Visibility = Visibility.Visible;
                    Console.WriteLine(doc.DocumentNode.SelectSingleNode("//*[@id='Delete']").InnerHtml);
                    canDelete = true;
                }

                var whiteTextNodes = doc.DocumentNode.SelectNodes("//*[@class='whitetext']");
                if (whiteTextNodes != null && whiteTextNodes.Count >= 2)
                {
                    string reqdHrs = HttpUtility.HtmlDecode(whiteTextNodes[0].InnerText);
                    string dateSubtd = HttpUtility.HtmlDecode(whiteTextNodes[1].InnerText);
                    string desc = HttpUtility.HtmlDecode(doc.DocumentNode.SelectSingleNode("//textarea[@id='description']")?.InnerText);

                    UpdateUIWithData(reqdHrs, dateSubtd, desc);
                }
                else
                {
                    Console.WriteLine("Could not find expected elements in the response.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                WriteToFile("error.txt", ex.Message);
            }
        }

        private void UpdateUIWithData(string reqdHrs, string dateSubtd, string desc)
        {
            eventNameLabel.Text = this.eventName;
            reqdHrsLabel.Text = reqdHrs;
            dateSubtdLabel.Text = dateSubtd;
            descriptionBox.Text = desc;
        }

        private void WriteToFile(string filename, string toWrite)
        {
            try
            {
                string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string dataPath = Path.Combine(localAppDataPath, appDataFolder);

                if (!Directory.Exists(dataPath))
                {
                    Directory.CreateDirectory(dataPath);
                }

                string filePath = Path.Combine(dataPath, filename);
                File.WriteAllText(filePath, toWrite);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private async void DeleteReq(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var values = new Dictionary<string, string>
                    {
                        { "del", id }
                    };

                    var content = new FormUrlEncodedContent(values);

                    Uri uri = new Uri("https://academyendorsement.olatheschools.com/");
                    _cookieContainer.Add(uri, new Cookie("PHPSESSID", phpSessionId));

                    if (!_client.DefaultRequestHeaders.Contains("User-Agent"))
                    {
                        _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                    }

                    var response = await _client.PostAsync("https://academyendorsement.olatheschools.com/deleteRequest.php", content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    WriteToFile("delreq.txt", responseString);

                    // Since this is a Page, we need to navigate back or close the window
                    if (Window.GetWindow(this) is Window parentWindow)
                    {
                        parentWindow.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    WriteToFile("error.txt", ex.Message);
                }
            }
        }
    }
}