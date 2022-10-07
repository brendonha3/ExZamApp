using GameTracker.Helpers;
using IGDB;
using IGDB.Models;
using System.Threading.Tasks;

namespace GameTracker.Services
{
    public static class GameService
    {
        private static IGDBClient? _client;

        /// <summary>
        /// Init to ensure that the httpclient is always active
        /// </summary>
        private static void Init()
        {
            if (_client != null)
                return;

            _client = new IGDBClient(
                "aa8j5sctlvcfel9yepmouol48roqhx",
                "83niixjlw4du8plaktbi8ovvndvjw2"
            );
        }

        /// <summary>
        /// Queries top20 most popular games
        /// </summary>
        /// <returns>Game[]</returns>
        public static async Task<Game[]> GetTopRatedGames()
        {
            Init();

            return await _client!.QueryAsync<Game>(IGDBClient.Endpoints.Games,
                query: @"fields *, 
                        collection.name,
                        cover.url,
                        genres.name,
                        platforms.name,
                        release_dates.date,
                        screenshots.url;
                        sort total_rating_count desc;                           
                        where total_rating != null & 
                        total_rating_count > 10 &
                        version_parent = null;
                        limit 20;");
        }

        /// <summary>
        /// Queries an additional top20 popular games
        /// Starting off on the last game on the existing list
        /// </summary>
        /// <param name="lastGameCount"></param>
        /// <returns></returns>
        public static async Task<Game[]> GetTopRatedGamesThreshold(int lastGameCount)
        {
            Init();

            return await _client!.QueryAsync<Game>(IGDBClient.Endpoints.Games,
                query: @"fields *, 
                        collection.name,
                        cover.url,
                        genres.name,
                        platforms.name,
                        release_dates.date,
                        screenshots.url;
                        limit 20;
                        sort total_rating_count desc;                           
                        where total_rating != null & 
                        total_rating_count > 10 &
                        version_parent = null & " +
                        "total_rating_count < " + lastGameCount + ";");
        }

        /// <summary>
        /// Queries a single game
        /// </summary>
        /// <param name="id">GameID</param>
        /// <returns>Game[]</returns>
        public static async Task<Game[]> GetGame(long id)
        {
            Init();

            return await _client!.QueryAsync<Game>(IGDBClient.Endpoints.Games,
                query: @"fields *, 
                        collection.name,
                        cover.url,
                        genres.name,
                        platforms.name,
                        release_dates.date,
                        screenshots.url;
                        sort aggregated_rating desc;                           
                        where id = " + id.ToString() + ";");
        }

        /// <summary>
        /// Queries API based on searchterm
        /// Total_rating_count and version_parent are to keep out unknown games
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns>Game[]</returns>
        public static async Task<Game[]> GetSearchedGames(string searchTerm)
        {
            Init();

            return await _client!.QueryAsync<Game>(IGDBClient.Endpoints.Games,
                 query: @"fields *, 
                        collection.name,
                        cover.url,
                        genres.name,
                        platforms.name,
                        release_dates.date,
                        screenshots.url;
                        sort total_rating_count desc;
                        limit 20;" +
                        "where name ~ *\""+ searchTerm + "\"* & " +
                        "total_rating_count > 5 & " +
                        "version_parent = null;");
        }

        /// <summary>
        /// Queries an additional 20 games
        /// Starting off on the last game on the existing list
        /// and matching on the name
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="lastGameScore"></param>
        /// <returns></returns>
        public static async Task<Game[]> GetSearchedGamesThreshold(string searchTerm, int lastGameScore)
        {
            Init();

            return await _client!.QueryAsync<Game>(IGDBClient.Endpoints.Games,
                 query: @"fields *, 
                        collection.name,
                        cover.url,
                        genres.name,
                        platforms.name,
                        release_dates.date,
                        screenshots.url;
                        sort total_rating_count desc;
                        limit 20;" +
                        "where name ~ *\"" + searchTerm + "\"* & " +
                        "total_rating_count < " + lastGameScore + "& " +
                        "total_rating_count > 2 & " +
                        "version_parent = null;");
        }

        /// <summary>
        /// Queries based on a sorted list of gameIDs tied to games
        /// that have been saved
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>Game[]</returns>
        public static async Task<Game[]> GetSavedGames(long[] ids)
        {
            Init();

            if (ids.Length == 0)
               return new Game[0];

            var idString = GameHelper.FormatNumberArray(ids);

            return await _client!.QueryAsync<Game>(IGDBClient.Endpoints.Games,
                 query: @"fields *, 
                        collection.name,
                        cover.url,
                        genres.name,
                        platforms.name,
                        release_dates.date,
                        screenshots.url;
                        where id = (" + idString + ");");
        }
    }
}
