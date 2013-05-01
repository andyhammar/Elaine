using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace Elaine.Win
{
    public class MainPageVm
    {
        public MainPageVm()
        {
            NewsItems = new ObservableCollection<FeedItem>();
        }
        public ObservableCollection<FeedItem> NewsItems { get; set; }

        public async Task Init()
        {
            await GetFeedAsync("http://sydsvenskan.se/rss/senastenytt");
        }

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

                    try
                    {
                        string imageUri = item.NodeValue.Split(new[] { "img src=\"" }, 2, StringSplitOptions.None)[1];
                        imageUri = imageUri.Split('"')[0];
                        feedItem.ImageUri = imageUri;
                    }
                    catch { }

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
}