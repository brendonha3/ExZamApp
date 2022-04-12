using CommunityToolkit.Diagnostics;
using GameTracker.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GameTracker.Services
{
    public static class GameService
    {
        static SQLiteAsyncConnection db;

        static async Task Init()
        {
            if (db != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<Game>();
        }

        public static async Task AddGame(string name, int rating, string studio, DateTime releaseDate)
        {
            await Init();

            var game = new Game
            {
                Name = name,
                Rating = rating,
                Studio = studio,
                ReleaseDate = releaseDate,
                TimeStamp = DateTime.Now,
            };

            await db.InsertAsync(game);
        }

        public static async Task RemoveGame(int id)
        {
            await Init();

            await db.DeleteAsync<Game>(id);
        }

        public static async Task<IEnumerable<Game>> GetGame()
        {
            await Init();

            var games = await db.Table<Game>().ToListAsync();

            return games;
        }

        public static async Task<Game> GetGame(int id)
        {
            await Init();

            var game = await db.Table<Game>()
                .FirstOrDefaultAsync(c => c.Id == id);

            return game;
        }

        public static async Task UpdateGame(Game game)
        {
            await Init();

            await db.UpdateAsync(game);
        }
    }
}
