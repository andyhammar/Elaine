﻿namespace Elaine.Win
{
    public class MainPageVmDesign : MainPageVm
    {
        public MainPageVmDesign()
        {
            NewsItems.Add(new FeedItem()
                {
                    Content = "Long story about something that happened in Malmö. sadfadsf asdfasdf asdfasdf.",
                    ImageUri = "http://www.sydsvenskan.se/ScaledImages/704x396/Images2/2013/5/1/hippe.jpg?h=7780a611e6809e17477e78b38e72a9d2&amp;fill=False&amp;cut=False",
                    Title = "Title of event"
                });


//            Senaste nytt
//http://sydsvenskan.se/rss/senastenytt

//Sport
//http://sydsvenskan.se/rss/sport

//Malmö
//http://sydsvenskan.se/rss/malmo

//Lund
//http://sydsvenskan.se/rss/lund

//Skåne
//http://sydsvenskan.se/rss/skane

//Opinion
//http://sydsvenskan.se/rss/opinion
        }
    }
}