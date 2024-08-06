using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HtmlAgilityPack;
using System.Net;
using System.Net.Http;
using System.Web;

namespace eHourWPF
{
    public partial class Home : Page
    {
        private string username;
        private string password;
        private string phpSessionId;
        private string nameOfPerson;
        private string nameOfAcademy;
        private string getresp;
        private CookieContainer _cookieContainer;
        private HttpClientHandler _handler;
        private HttpClient _client;

        private HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        private List<EHourRequest> eHourRequests = new List<EHourRequest>();

        public Home(string username, string password, string phpSessionId, string nameOfPerson, string nameOfAcademy, string getresp, CookieContainer _cookieContainer, HttpClientHandler _handler, HttpClient _client)
        {
            InitializeComponent();
            this.username = username;
            this.password = password;
            this.phpSessionId = phpSessionId;
            this.nameOfPerson = nameOfPerson;
            this.nameOfAcademy = nameOfAcademy;
            this.getresp = getresp;

            this._cookieContainer = _cookieContainer;
            this._handler = _handler;
            this._client = _client;

            doc.LoadHtml(getresp);

            this.Loaded += Home_Loaded;

            studentName.Content = nameOfPerson;
            studentAcademy.Content = nameOfAcademy;
        }

        private void Home_Loaded(object sender, RoutedEventArgs e)
        {
            ParseEHourRequests();
            CreateLayout();
        }

        private void ParseEHourRequests()
        {
            var rows = doc.DocumentNode.SelectNodes("//tr[@class='entry']");

            if (rows != null)
            {
                foreach (var row in rows)
                {
                    var buttonNode = row.SelectSingleNode(".//button[@name='ehours_request_descr']");
                    var value = buttonNode.GetAttributeValue("value", string.Empty);
                    var description = HttpUtility.HtmlDecode(buttonNode.InnerText.Trim());

                    var tdNodes = row.SelectNodes(".//td");
                    if (tdNodes != null && tdNodes.Count >= 3)
                    {
                        var hours = tdNodes[1].InnerText.Trim();
                        var date = tdNodes[2].InnerText.Trim();

                        eHourRequests.Add(new EHourRequest
                        {
                            Value = value,
                            Description = description,
                            Hours = hours,
                            Date = date
                        });
                    }
                }
            }
        }

        private void CreateLayout()
        {
            foreach (var request in eHourRequests)
            {
                Button requestButton = new Button
                {
                    Content = $"{request.Description}\nHours: {request.Hours}\nDate: {request.Date}",
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    FontFamily = new FontFamily("SF Pro Display"),
                    FontSize = 13,
                    Width = buttonContainer.ActualWidth - 40,
                    Height = 80,
                    Margin = new Thickness(20, 5, 20, 5),
                    Tag = request.Value,
                    Foreground = Brushes.White
                };

                requestButton.Click += RequestButton_Click;
                buttonContainer.Children.Add(requestButton);
            }
        }

        private void RequestButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            string value = (string)clickedButton.Tag;
            // TODO: Implement RequestViewer for WPF
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            RequestViewer requestViewer = new RequestViewer(value, phpSessionId, clickedButton.Content.ToString(), _cookieContainer, _handler, _client);
            mainWindow.MainFrame.Navigate(requestViewer);
        }

        private void CreateRequest(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Creating eHour Requests is not supported yet.\r\nWe (I) know it's inconvenient, but I also don't know how to code C#.\r\nCheck back in a week or two!", ":(", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public class EHourRequest
    {
        public string Value
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public string Hours
        {
            get; set;
        }
        public string Date
        {
            get; set;
        }
    }
}