using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace Elaine.Win
{
    public class MainPageVm : INotifyPropertyChanged
    {
        public MainPageVm()
        {
            NewsItems = new ObservableCollection<FeedItem>();
            //NewsFeeds = new ObservableCollection<NewsFeed>();
            //NewsFeeds.Add(new NewsFeed { Uri = "http://sydsvenskan.se/rss/senastenytt", Title = "Senaste nytt" });
            //NewsFeeds.Add(new NewsFeed { Uri = "http://sydsvenskan.se/rss/sport", Title = "Sport" });
            //NewsFeeds.Add(new NewsFeed { Uri = "http://sydsvenskan.se/rss/malmo", Title = "Malmö" });
            //NewsFeeds.Add(new NewsFeed { Uri = "http://sydsvenskan.se/rss/lund", Title = "Lund" });
            //NewsFeeds.Add(new NewsFeed { Uri = "http://sydsvenskan.se/rss/skane", Title = "Skåne" });
            //NewsFeeds.Add(new NewsFeed { Uri = "http://sydsvenskan.se/rss/opinion", Title = "Opinion" });
        }

        //public ObservableCollection<NewsFeed> NewsFeeds { get; set; }

        public ObservableCollection<FeedItem> NewsItems { get; set; }

        public async Task Init(NewsFeed feed)
        {
            await GetFeedAsync(feed.Uri);
        }

        private async Task GetFeedAsync(string feedUriString)
        {
            // using Windows.Web.Syndication;
            var client = new SyndicationClient();
            var feedUri = new Uri(feedUriString);

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

                    content = content.Replace("&#160;", " ");

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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}