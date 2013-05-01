using System;
using System.Diagnostics;
using System.Linq;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
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
            bool couldInit = false;
            try
            {
                await _vm.Init();
                couldInit = _vm.NewsItems.Any();
                busyBorder.Visibility = Visibility.Collapsed;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.ToString());
            }

            if (!couldInit)
            {

                var messageDialog = new MessageDialog("Kunde inte hämta nyheter, vänligen kontrollera att du har nätverksåtkomst och försök igen.");
                await messageDialog.ShowAsync();
                Application.Current.Exit();
            }
        }

    
    }
}