using GameTracker.ViewModels;
using GameTracker.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GameTracker
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(GameDetailPage), typeof(GameDetailPage));
        }
    }
}
