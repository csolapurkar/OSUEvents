using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace OSUEvents
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class GroupsPage : OSUEvents.Common.LayoutAwarePage
    {
        public GroupsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property provides the grouped collection of items to be displayed.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            EventsDataSource _feedDataSource = App.EventDataSource;
            CategoryDataSource _categoryDataSource = App.CategoryDataSource;

            if (_feedDataSource.EventsList.Count == 0)
            {
                await _feedDataSource.getEventsAsync();
            }

            if (_categoryDataSource.Categories.Count == 0)
            {
                await _categoryDataSource.getCategoriesAsync();
            }

            this.DataContext = (_feedDataSource.EventsList).First();

            this.DefaultViewModel["Groups"] = e.Parameter;
        }
    }
}
