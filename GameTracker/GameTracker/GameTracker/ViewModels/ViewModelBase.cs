using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTracker.ViewModels
{
    public partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private string subtitle = string.Empty;

        [ObservableProperty]
        private string icon = string.Empty;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool isNotBusy = true;

        [ObservableProperty]
        private bool canLoadMore = true;

        [ObservableProperty]
        private string header = string.Empty;

        [ObservableProperty]
        private string footer = string.Empty;

        public ViewModelBase()
        {

        }

    }
}