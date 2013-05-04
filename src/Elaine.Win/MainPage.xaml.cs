using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Elaine.Win
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        private MainPageVm _vm;

        public MainPage()
        {
            this.InitializeComponent();
            _vm = new MainPageVm();
            DataContext = _vm;

            SettingsPane.GetForCurrentView().CommandsRequested += MainPage_CommandsRequested;
        }

        void MainPage_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsHelper.AddSettingsCommands(args);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //var feed = _vm.NewsFeeds.First();
            await TryInit(new NewsFeed{Uri = "http://www.sydsvenskan.se/rss.xml"});
        }

        private async Task TryInit(NewsFeed feed)
        {
            bool couldInit = false;
            try
            {
                await _vm.Init(feed);
                couldInit = _vm.NewsItems.Any();
                busyBorder.Visibility = Visibility.Collapsed;
                AnimateSwipeText.Begin();
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.ToString());
            }

            if (!couldInit)
            {
                var messageDialog =
                    new MessageDialog(
                        "Kunde inte hämta nyheter, vänligen kontrollera att du har nätverksåtkomst och försök igen.");
                await messageDialog.ShowAsync();
                Application.Current.Exit();
            }
        }

        //private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    var item = e.ClickedItem;
        //    if (item == null) return;

        //    await TryInit(item as NewsFeed);
        //}

    }
}