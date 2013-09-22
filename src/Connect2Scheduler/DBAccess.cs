using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotWars.Common;
using BotWars.DAL;
using BotWars.DAL.Objects;
using NHibernate;
using NHibernate.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Connect2Scheduler
{
    public class DBAccess
    {
        public static void CreateDatabase()
        {
            Config.CreateDatabase();
        }

        public static IEnumerable<Player> GetConnect2Players()
        {
            using (var session = Config.GetSessionFactory().OpenSession())
            {
                return (from b in session.Query<Bot>()
                        where b.GamePlayed.Name == "Connect 2"
                        select new Player()
                            {
                                AuthorName = b.Author.Name,
                                BotName = b.Name,
                                ProcessPath = string.Format("../../../../Environment/Users/{0}/{1}", b.Author.FolderName, b.ExecutableName),
                                Rating = b.Rating
                            }).ToList();
            }
        }

        public static void RecordConnect2Result(Player player1, Player player2, decimal result1, decimal result2, double scoreChange1, double scoreChange2, JObject json)
        {
            using (var session = Config.GetSessionFactory().OpenSession())
            {
                var connect2Game = session.Query<Game>().Single(g => g.Name == "Connect 2");

                var gameRecord = new GameRecord
                    {
                        GamePlayed = connect2Game,
                        RecordedDate = DateTime.Now,
                        Json = json.ToString(Formatting.None)
                    };

                session.Save(gameRecord);

                AddParticipant(session, gameRecord, player1, result1, scoreChange1);
                AddParticipant(session, gameRecord, player2, result2, scoreChange2);
            }
        }

        private static void AddParticipant(ISession session, GameRecord gameRecord, Player player, decimal result,
                                           double scoreChange)
        {
            var bot = session.Query<Bot>().Single(b => b.Author.Name == player.AuthorName && b.Name == player.BotName);
            bot.Rating += scoreChange;

            var participant = new GameParticipant
                {
                    Bot = bot,
                    GameRecord = gameRecord,
                    NewRating = bot.Rating,
                    Result = (double) result
                };
            
            session.Save(participant);
            session.SaveOrUpdate(bot); 
            session.Flush();
        }
    }
}