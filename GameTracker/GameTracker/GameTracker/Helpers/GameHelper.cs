using GameTracker.Models;
using IGDB.Models;

namespace GameTracker.Helpers
{
    public static class GameHelper
    {
        /// <summary>
        /// Function casts fields in a Game object into correct format for display in xaml
        /// </summary>
        /// <param name="game">IGDB.Game</param>
        /// <returns></returns>
        public static Game FormatGameFields(Game game)
        {
            if (game.Cover != null)
                game.Cover.Value.Url = FormatUrl(game.Cover.Value.Url) ?? "";

            if (game.AggregatedRating != null)
                game.AggregatedRating = (int?)game.AggregatedRating ?? 0;

            if (game.Rating != null)
                game.Rating = (int?)game.Rating ?? 0;

            if (game.TotalRating != null)
                game.TotalRating = (int?)game.Rating ?? 0;

            return game;
        }

        /// <summary>
        /// Urls from API are missing https: and come as a thumbnail, 
        /// so this adds https and changes to 720p to the return string
        /// </summary>
        /// <param name="url">string</param>
        /// <returns>formatted string</returns>
        public static string FormatUrl(string url)
        {
            return "https:" + url.Replace("t_thumb", "t_720p");
        }

        /// <summary>
        /// Converts Genre[] and Platform[] in a IGDB.Game object
        /// to a string for proper display in xaml
        /// </summary>
        /// <param name="game">IGDB.Game</param>
        /// <returns>new GameDescription object</returns>
        public static GameDescription FormatDescription(Game game)
        {
            var genres = "";
            var platforms = "";
            bool isFirstPass = true;

            if (game.Genres != null)
            {
                foreach (var genre in game.Genres.Values)
                {
                    if (isFirstPass)
                    {
                        genres = genre.Name ?? "N/A";
                        isFirstPass = false;
                    }
                    else
                        genres += ", " + genre.Name;
                }
            }

            isFirstPass = true;

            if (game.Platforms != null)
            {
                foreach (var platform in game.Platforms.Values)
                {
                    if (isFirstPass)
                    {
                        platforms = platform.Name ?? "N/A";
                        isFirstPass = false;
                    }
                    else
                        platforms += ", " + platform.Name;
                }
            }
            
            return new GameDescription()
            {
                Genres = genres,
                Platforms = platforms
            };
        }

        /// <summary>
        /// Converts a long[] into a formatted string for 
        /// use in querying API for saved Game[]
        /// </summary>
        /// <param name="longs"></param>
        /// <returns>formatted string</returns>
        public static string FormatNumberArray(long[] longs)
        {
            var longString = "";
            bool isFirstPass = true;

            foreach (var id in longs)
            {
                if (isFirstPass)
                {
                    longString = id.ToString();
                    isFirstPass = false;
                }
                else
                    longString += ", " + id.ToString();
            }

            return longString;
        }
    }
}
