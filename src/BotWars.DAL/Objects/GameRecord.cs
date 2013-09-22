using System;

namespace BotWars.DAL.Objects
{
    public class GameRecord
    {
        public virtual int Id { get; set; }
        public virtual Game GamePlayed { get; set; }
        public virtual DateTime RecordedDate { get; set; }
        public virtual string Json { get; set; }
    }
}