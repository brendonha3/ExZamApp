using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameDetailPage : ContentPage
    {
        public GameDetailPage()
        {
            InitializeComponent();
        }

        private void deleteItem_Clicked(object sender, EventArgs e)
        {
            deleteItem.IsEnabled = true;
        }

        private void editItem_Clicked(object sender, EventArgs e)
        {
            editItem.IsEnabled = true;
        }
    }
}