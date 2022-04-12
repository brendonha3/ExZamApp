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
    public partial class GamePage : ContentPage
    {
        public GamePage()
        {
            InitializeComponent();
        }

        // nulling selecteditem in collectionview required to select the same item multiple times
        void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((CollectionView)sender).SelectedItem = null;
        }

        // toolbar items must be re-enabled or they disable when clicked
        private void addItem_Clicked(object sender, EventArgs e)
        {
            addItem.IsEnabled = true;
        }
    }
}