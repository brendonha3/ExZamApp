using GameTracker.Views;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GameTracker.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        LoginViewModel()
        {
            Title = "Login";
        }

        [ICommand]
        async Task Login()
        {
            var route = $"{nameof(GamePage)}";
            await Shell.Current.GoToAsync(route);
        }
    }
}
