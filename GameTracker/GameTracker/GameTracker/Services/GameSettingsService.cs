using GameTracker.Models;
using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace GameTracker.Services
{
    public static class GameSettingsService
    {
        static SQLiteAsyncConnection? db;

        static async Task Init()
        {
            if (db != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<GameSettings>();
        }

        public static async Task AddGameSettings(GameSettings settings)
        {
            await Init();

            await db!.InsertAsync(settings);
        }

        public static async Task<ObservableRangeCollection<GameSettings>> GetGameSettings(string button)
        {
            await Init();

            var gameSettings = new ObservableRangeCollection<GameSettings>();

            if (button == "owned")
                gameSettings.AddRange(await db!.Table<GameSettings>()
                                               .Where(e => e.IsOwned == true)
                                               .ToListAsync());
            else if (button == "wishlist")
                gameSettings.AddRange(await db!.Table<GameSettings>()
                                               .Where(e => e.IsWishlist == true)
                                               .ToListAsync());
            else
                gameSettings.AddRange(await db!.Table<GameSettings>()
                                               .Where(e => e.IsFavorite == true)
                                               .ToListAsync());

            return gameSettings;
        }

        public static async Task<GameSettings> GetGameSettings(long id)
        {
            await Init();

            var gameSettings = await db!.Table<GameSettings>()
                .FirstOrDefaultAsync(c => c.GameId == id);

            return gameSettings;
        }

        public static async Task<GameSettings> UpdateGameSettings(GameSettings settings)
        {
            await Init();

            await db!.UpdateAsync(settings);

            return settings;
        }

        public static async Task<GameSettings> UpdateWishSetting(long id)
        {
            var settings = await GetGameSettings(id);

            if (settings == null)
            {
                settings = new()
                {
                    GameId = id,
                    IsWishlist = true,
                    LastWishlist = DateTime.Now
                }; 

                await AddGameSettings(settings);
                return settings;
            }

            if (settings.IsWishlist == true)
                settings.IsWishlist = false;
            else
            {
                settings.IsWishlist = true;
                settings.LastWishlist = DateTime.Now;
            }

            return await UpdateGameSettings(settings);
        }

        public static async Task<GameSettings> UpdateFavoriteSetting(long id)
        {
            var settings = await GetGameSettings(id);

            if (settings == null)
            {
                settings = new()
                {
                    GameId = id,
                    IsFavorite = true,
                    LastFavorite = DateTime.Now
                };

                await AddGameSettings(settings);
                return settings;
            }

            if (settings.IsFavorite == true)
                settings.IsFavorite = false;
            else
            {
                settings.IsFavorite = true;
                settings.LastFavorite = DateTime.Now;
            }

            return await UpdateGameSettings(settings);
        }

        public static async Task<GameSettings> UpdateOwnedSetting(long id)
        {
            var settings = await GetGameSettings(id);

            if (settings == null)
            {
                settings = new()
                {
                    GameId = id,
                    IsOwned = true,
                    LastOwned = DateTime.Now
                };

                await AddGameSettings(settings);
                return settings;
            }

            if (settings.IsOwned == true)
                settings.IsOwned = false;
            else
            {
                settings.IsOwned = true;
                settings.LastOwned = DateTime.Now;
            }

            return await UpdateGameSettings(settings);
        }
    }
}

