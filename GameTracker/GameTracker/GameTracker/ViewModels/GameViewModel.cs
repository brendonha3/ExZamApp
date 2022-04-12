using GameTracker.Models;
using GameTracker.Services;
using GameTracker.Views;
using GameTracker.Resources;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Xamarin.Forms;
using System;
using System.Threading.Tasks;

namespace GameTracker.ViewModels
{
    public partial class GameViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Game selectedGame;

        public ObservableRangeCollection<Game> Games { get; set; }

        public GameViewModel()
        {
            Title = "Games";

            Games = new ObservableRangeCollection<Game>();
        }

        [ICommand]
        async Task Selected(Game game)
        {
            Guard.IsNotNull(game);

            var route = $"{nameof(GameDetailPage)}?GameId={game.Id}";
            await Shell.Current.GoToAsync(route);
        }

        [ICommand]
        async Task Load()
        {
            Games.Clear();

            var games = await GameService.GetGame();

            Games.AddRange(games);
        }

        [ICommand]
        async Task Add()
        {
            var route = $"{nameof(NewGamePage)}";
            await Shell.Current.GoToAsync(route);
        }

        [ICommand]
        async Task Remove(Game game)
        {
            Guard.IsNotNull(game);

            await GameService.RemoveGame(game.Id);
            await Load();
        }

        [ICommand]
        async Task Refresh()
        {
            IsBusy = true;

            await Load();

            IsBusy = false;
        }
    }
}
