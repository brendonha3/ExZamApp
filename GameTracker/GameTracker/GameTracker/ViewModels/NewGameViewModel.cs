using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameTracker.Models;
using GameTracker.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GameTracker.ViewModels
{
    [QueryProperty(nameof(GameId), nameof(GameId))]
    public partial class NewGameViewModel : ViewModelBase
    {
        [ObservableProperty]
        private int gameId;

        [ObservableProperty]
        private string name, studio, rating;

        [ObservableProperty]
        private DateTime releaseDate;

        [ObservableProperty]
        private Game game;

        private int ratingNumber;
        public string MinimumDate { get; set; }
        public string MaximumDate { get; set; }


        public NewGameViewModel()
        {
            Title = "Add game";
            MinimumDate = "01/01/1971"; // year of the first commercial arcade video game
            MaximumDate = DateTime.Now.ToString("MM/dd/yyyy");
        }

        [ICommand]
        async Task Save()
        {
            Guard.IsNotNullOrWhiteSpace(name);
            Guard.IsNotNullOrWhiteSpace(rating);
            Guard.IsNotNullOrWhiteSpace(studio);

            int.TryParse(rating, out ratingNumber);

            if (gameId == 0)
                await GameService.AddGame(name, ratingNumber, studio, releaseDate);
            else
            {
                PrepareGameObject(game);
                await GameService.UpdateGame(game);
            }

            await Shell.Current.GoToAsync("..");
        }

        [ICommand]
        async Task OnPageAppearing()
        {
            if (gameId == 0)
                return;

            Game = await GameService.GetGame(gameId);
            Name = Game.Name;
            Rating = Game.Rating.ToString();
            Studio = Game.Studio;
            ReleaseDate = Game.ReleaseDate;
        }

        private void PrepareGameObject(Game game)
        {
            game.Rating = ratingNumber;
            game.Studio = studio;
            game.ReleaseDate = releaseDate;
            game.Name = name;
        }
    }
}
