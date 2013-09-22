using System;

namespace BotWars.DAL.Objects
{
    public class Bot
    {
        public virtual int Id { get; set; }
        public virtual Author Author { get; set; }
        public virtual string Name { get; set; }
        public virtual Game GamePlayed { get; set; }
        public virtual double Rating { get; set; }
        public virtual string ExecutableName { get; set; }
    }
}