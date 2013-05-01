using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Syndication;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Elaine.Win
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            NewsItems = new ObservableCollection<FeedItem>();
            DataContext = this;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await GetFeedAsync("http://sydsvenskan.se/rss/senastenytt");
        }

        public ObservableCollection<FeedItem> NewsItems { get; set; }

        private async Task GetFeedAsync(string feedUriString)
        {
            // using Windows.Web.Syndication;
            SyndicationClient client = new SyndicationClient();
            Uri feedUri = new Uri(feedUriString);

            try
            {
                SyndicationFeed feed = await client.RetrieveFeedAsync(feedUri);
                foreach (SyndicationItem item in feed.Items)
                {
                    FeedItem feedItem = new FeedItem();
                    feedItem.Title = item.Title.Text;
                    feedItem.PubDate = item.PublishedDate.DateTime;
                    feedItem.Author = (item.Authors != null && item.Authors.Any()) ? item.Authors[0].Name.ToString() : null;

                    //feedItem.Content = item.Summary.Text;
                    var firstOrDefault = item.ElementExtensions.FirstOrDefault(e => e.NodeName == "summary");
                    var content = firstOrDefault != null ? firstOrDefault.NodeValue : string.Empty;
                    content = content.Replace("<br>", Environment.NewLine);
                    content = WebUtility.UrlDecode(content);
                    feedItem.Content = content;

                    NewsItems.Add(feedItem);
                }
            }
            catch (Exception ex)
            {
                // Log Error.
                //TitleText.Text = ex.Message;
                Debug.WriteLine(ex);
            }
        }
    }

    public class FeedItem
    {
        public string Title { get; set; }

        public DateTime PubDate { get; set; }

        public string Author { get; set; }

        public string Content { get; set; }
    }
}