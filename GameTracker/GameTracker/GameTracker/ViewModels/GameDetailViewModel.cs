using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameTracker.Helpers;
using GameTracker.Models;
using GameTracker.Services;
using IGDB.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace GameTracker.ViewModels
{
    [QueryProperty(nameof(GameId), nameof(GameId))]
    public partial class GameDetailViewModel : ViewModelBase
    {
        [ObservableProperty]
        private int _gameId;

        [ObservableProperty]
        private Game? _game;

        [ObservableProperty]
        private ObservableCollection<Screenshot> _screenshots;

        [ObservableProperty]
        private GameDescription _description;

        [ObservableProperty]
        private GameSettings _settings;


        public GameDetailViewModel()
        {
            Title = "Game details";

            Screenshots = new ObservableCollection<Screenshot>();         
        }

        /// <summary>
        /// Loads a Game[] from API based on passed gameId param from previous page
        /// and moves and formats properties from the selected game into classes that are xaml friendly.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task Load()
        {
            var gameResult = await GameService.GetGame(_gameId);

            Game = GameHelper.FormatGameFields(gameResult.FirstOrDefault());

            Game.Screenshots.Values.ForEach(ss => Screenshots.Add(ss));

            Screenshots.ForEach(ss => ss.Url = GameHelper.FormatUrl(ss.Url));

            Description = GameHelper.FormatDescription(Game);

            Settings = await GameSettingsService.GetGameSettings(_gameId) 
                       ?? new GameSettings() { GameId = _gameId };
        }

        [RelayCommand]
        public async Task UpdateOwned()
        {
            Settings = await GameSettingsService.UpdateOwnedSetting(_gameId);
        }

        [RelayCommand]
        public async Task UpdateFavorite()
        {
            Settings = await GameSettingsService.UpdateFavoriteSetting(_gameId);
        }

        [RelayCommand]
        public async Task UpdateWish()
        {
            Settings = await GameSettingsService.UpdateWishSetting(_gameId);
        }
    }
}
