using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTracker.Models
{
    public class Game
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Name { get; set; }
        public string Studio { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
