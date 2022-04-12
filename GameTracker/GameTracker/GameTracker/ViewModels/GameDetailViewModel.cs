using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameTracker.Models;
using GameTracker.Services;
using GameTracker.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GameTracker.ViewModels
{
    [QueryProperty(nameof(GameId), nameof(GameId))]
    public partial class GameDetailViewModel : ViewModelBase
    {
        [ObservableProperty]
        private int gameId;

        [ObservableProperty]
        private Game game;

        public GameDetailViewModel()
        {
            Title = "Game details";
        }

        [ICommand]
        async Task OnPageAppearing()
        {
            await LoadGame();
        }

        [ICommand]
        async Task Delete()
        {
            await GameService.RemoveGame(gameId);
            await Shell.Current.Navigation.PopAsync();
        }

        [ICommand]
        async Task Edit()
        {
            await Shell.Current.GoToAsync($"{nameof(NewGamePage)}?GameId={gameId}");
        }

        [ICommand]
        async Task LoadGame()
        {
            Game = await GameService.GetGame(gameId);
        }
    }
}
