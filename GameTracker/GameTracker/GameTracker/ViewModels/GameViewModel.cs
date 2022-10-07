using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameTracker.Helpers;
using GameTracker.Models;
using GameTracker.Services;
using GameTracker.Views;
using IGDB.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace GameTracker.ViewModels
{
    public partial class GameViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Game? _selectedGame;

        [ObservableProperty]
        private string _searchTerm, _emptyViewText;
        
        [ObservableProperty]
        private bool _isOwnedGame, _isWishlistGame, _isFavoriteGame;
 
        private bool _isBackNav;
        private string _savedGameButton;

        public ObservableRangeCollection<Game> Games { get; set; }

        public GameViewModel()
        {
            Title = "Games API";

            Games = new ObservableRangeCollection<Game>();
        }

        /// <summary>
        /// Passes selected game to GameDetailPage
        /// and enables BackNav, which disables Load() temporarily
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        [RelayCommand]
        async Task Selected(Game game)
        {
            Guard.IsNotNull(game);

            var route = $"{nameof(GameDetailPage)}?GameId={game.Id}";
            _isBackNav = true;
            await Shell.Current.GoToAsync(route);
        }

        /// <summary>
        /// Search command
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task Search()
        {
            await Load();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        [RelayCommand]
        async Task SavedGamesButton(string button)
        {
            SetButtonBool(button);
                
            if (!IsFavoriteGame && !IsOwnedGame && !IsWishlistGame)
            {
                await Load();
                return;
            }

            Game[] games = new Game[0];
            ObservableRangeCollection<GameSettings> gameSettings = new();
            List<long> gameIds = new();

            Games.Clear();

            EmptyViewText = "Loading...";

            gameSettings = await GameSettingsService.GetGameSettings(button);

            gameSettings = SortSettingByDate(gameSettings, button);

            gameSettings.ForEach(game => gameIds.Add(game.GameId));

            games = await GameService.GetSavedGames(gameIds.ToArray());

            games.ForEach(game => GameHelper.FormatGameFields(game));

            Games.AddRange(games);

            EmptyViewText = "No games";
        }

        /// <summary>
        /// Command that is run when the end of the CollectionView is reached.
        /// Guard clause disables this method if any saved game button is active.
        /// If search term is null, more top20 will return. Else, more search results.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task ThresholdReached()
        {
            if (IsFavoriteGame || IsOwnedGame || IsWishlistGame)
                return;

            Game[] games = new Game[0];

            if (_searchTerm == null)
                games = await GameService
                    .GetTopRatedGamesThreshold((int)Games.LastOrDefault().TotalRatingCount!);
            else
                games = await GameService
                    .GetSearchedGamesThreshold(_searchTerm, 
                    (int)Games.LastOrDefault().TotalRatingCount!);

            games.ForEach(game => GameHelper.FormatGameFields(game));

            Games.AddRange(games);
        }

        /// <summary>
        /// Disabled by BackNav
        /// Loads new games into collectionview, depending on if search is null or not
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task Load()
        {
            if (IsFavoriteGame || IsOwnedGame || IsWishlistGame)
                await SavedGamesButton(_savedGameButton);

            if (_isBackNav)
            {
                _isBackNav = false;
                return;
            }

            Games.Clear();

            EmptyViewText = "Loading...";

            Game[] games = new Game[0];

            if (_searchTerm == null)
                games = await GameService.GetTopRatedGames();
            else
                games = await GameService.GetSearchedGames(_searchTerm);

            games.ForEach(game => GameHelper.FormatGameFields(game));

            Games.AddRange(games);

            EmptyViewText = "No games";
        }

        /// <summary>
        /// Helper function to change saved buttons on button press
        /// </summary>
        /// <param name="button"></param>
        private void SetButtonBool(string button)
        {
            switch(button)
            {
                case "wishlist":
                    if (IsWishlistGame)
                    {
                        IsWishlistGame = false;
                        break;
                    }
                    IsWishlistGame = true;
                    IsOwnedGame = false;
                    IsFavoriteGame = false;
                    break;
                case "owned":
                    if (IsOwnedGame)
                    {
                        IsOwnedGame = false;
                        break;
                    }
                    IsOwnedGame = true;
                    IsWishlistGame = false;
                    IsFavoriteGame = false;
                    break;
                case "favorite":
                    if (IsFavoriteGame)
                    {
                        IsFavoriteGame = false;
                        break;
                    }
                    IsFavoriteGame = true;
                    IsWishlistGame = false;
                    IsOwnedGame = false;
                    break;
                default:
                    break;
            }

            _savedGameButton = button;
        }

        /// <summary>
        /// Helper function to sort saved games based on which saved game button is active
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        private ObservableRangeCollection<GameSettings> SortSettingByDate
                (ObservableRangeCollection<GameSettings> settings, string button)
        {
            ObservableRangeCollection<GameSettings> sortedCollection = new();

            switch (button)
            {
                case "wishlist":
                    sortedCollection.AddRange(settings.OrderBy(e => e.LastWishlist));
                    break;
                case "owned":
                    sortedCollection.AddRange(settings.OrderBy(e => e.LastOwned));
                    break;
                case "favorite":
                    sortedCollection.AddRange(settings.OrderBy(e => e.LastFavorite));
                    break;
                default:
                    return new ObservableRangeCollection<GameSettings>();
            }

            return sortedCollection;
        }
    }
}
