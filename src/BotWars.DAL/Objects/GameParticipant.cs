namespace BotWars.DAL.Objects
{
    public class GameParticipant
    {
        public virtual int Id { get; set; }
        public virtual Bot Bot { get; set; }
        public virtual GameRecord GameRecord { get; set; }
        public virtual double NewRating { get; set; }
        public virtual double Result { get; set; }
    }
}