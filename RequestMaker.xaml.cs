using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace eHourWPF
{
    public partial class RequestMaker : Page
    {
        private List<string> selectedImages = new List<string>();
        private string appDataFolder = "eHours";
        private string phpSessionId;
        private CookieContainer _cookieContainer;
        private HttpClientHandler _handler;
        private HttpClient _client;
        private HtmlAgilityPack.HtmlDocument finalreq;

        public RequestMaker(string phpSessionId, CookieContainer cookieContainer, HttpClientHandler handler, HttpClient client)
        {
            InitializeComponent();

            this.phpSessionId = phpSessionId;
            this._cookieContainer = cookieContainer;
            this._handler = handler;
            this._client = client;
        }

        private void SelectImages_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*"
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                selectedImages.AddRange(openFileDialog.FileNames);
                MessageBox.Show("Selected Images: " + string.Join(", ", openFileDialog.FileNames));
            }
        }

        private async Task PostRequestAsync(string title, string date, string hours, string desc)
        {
            Console.WriteLine(title);
            Console.WriteLine(date);
            Console.WriteLine(hours);
            Console.WriteLine(desc);
            try
            {
                string formattedDate = DateTime.Parse(date).ToString("yyyy-MM-dd");

                // Prepare the content for the POST request
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(title), "title");
                content.Add(new StringContent(formattedDate), "activityDate");
                content.Add(new StringContent(hours), "hours");
                content.Add(new StringContent(desc), "description");

                // Add images
                foreach (var imagePath in selectedImages)
                {
                    var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
                    imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                    content.Add(imageContent, "img[]", Path.GetFileName(imagePath));
                    Console.WriteLine(imagePath);
                }

                // Ensure the PHPSESSID cookie is set for the domain
                Uri uri = new Uri("https://academyendorsement.olatheschools.com/");
                _cookieContainer.Add(uri, new Cookie("PHPSESSID", phpSessionId));

                // Set User-Agent if not already set
                if (!_client.DefaultRequestHeaders.Contains("User-Agent"))
                {
                    _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                }

                MessageBoxResult result = MessageBox.Show("100% sure?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Send the POST request
                    var response = await _client.PostAsync("https://academyendorsement.olatheschools.com/Student/makeRequest.php", content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    // Handle response (optional)
                    MessageBoxResult maybeitposted = MessageBox.Show("It might've posted. Idk how to check from a C# app.\r\nGo check yourself (click OK to open, cancel to not)", "check urself", MessageBoxButton.OKCancel);
                    if (maybeitposted == MessageBoxResult.OK)
                    {
                        System.Diagnostics.Process.Start("https://academyendorsement.olatheschools.com/signinStudent.php");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                WriteToFile("anerror.txt", ex.Message);
            }
        }

        private void ClearForm_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to clear the form?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                TitleTextBox.Text = string.Empty;
                ActivityDatePicker.SelectedDate = null;
                HoursTextBox.Text = string.Empty;
                DescriptionTextBox.Text = string.Empty;
                selectedImages.Clear();
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

        private void SaveDraft_Click(object sender, RoutedEventArgs e)
        {
            // Logic to save the draft (you can implement this based on your requirements)
            MessageBox.Show("Draft Saved\r\n(IT IS NOT SAVED THIS IS SOMETHING CHATGPT WROTE PLEASE DO NOT CLOSE");
        }

        private void HoursTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Allow only numeric input
            e.Handled = !IsTextNumeric(e.Text);
        }

        private bool IsTextNumeric(string text)
        {
            Regex regex = new Regex("[^0-9]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to submit {TitleTextBox.Text} for {HoursTextBox.Text} eHours?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                await PostRequestAsync(TitleTextBox.Text, ActivityDatePicker.Text, HoursTextBox.Text, DescriptionTextBox.Text);
            }
        }
    }
}
