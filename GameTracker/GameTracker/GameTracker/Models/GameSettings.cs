using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GameTracker.Models
{
    public class GameSettings
    {
        [PrimaryKey]
        public long GameId { get; set; }
        public bool? IsFavorite { get; set; }
        public bool? IsWishlist { get; set; }
        public bool? IsOwned { get; set; }
        public DateTime? LastFavorite { get; set; }
        public DateTime? LastWishlist { get; set; }
        public DateTime? LastOwned { get; set; }
    }
}
