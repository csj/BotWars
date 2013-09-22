using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using BotWars.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Connect2Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var pool = DBAccess.GetConnect2Players();

                var player1 = pool.TakeRandom();
                Player player2;
                do player2 = pool.TakeRandom(); while (player2 == player1);

                Play(player1, player2);
                Play(player2, player1);  // let's ensure it's fair
            }
        }




        private static void Play(Player player1, Player player2)
        {

            var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                        {
                            FileName = "Connect2.exe",
                            Arguments = string.Join(" ", new[]
                                {
                                    player1.ProcessPath, "\"" + player1.BotName + "\"", "\"" + player1.AuthorName + "\"",
                                    player2.ProcessPath, "\"" + player2.BotName + "\"", "\"" + player2.AuthorName + "\"",
                                }),
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            RedirectStandardOutput = true
                        }
                };
            process.Start();

            var processOutput = process.StandardOutput.ReadLine();
            while (!process.HasExited) Thread.Sleep(10);
            
            var json = JObject.Parse(processOutput);
            
            var results = json["Connect2"]["Results"] as JArray;
            UpdateScores(player1, player2, results, json);
        }

        private static void UpdateScores(Player player1, Player player2, JArray results, JObject json)
        {
            var startScore1 = player1.Rating;
            var startScore2 = player2.Rating;

            var result1 = decimal.Parse(results[0].ToString());
            var result2 = decimal.Parse(results[1].ToString());

            var expectationDensity1 = Math.Pow(2, startScore1 / 200);
            var expectationDensity2 = Math.Pow(2, startScore2 / 200);
            var totalDensity = expectationDensity1 + expectationDensity2;

            var k = 32;

            var scoreChange1 = k*((double) result1 - (expectationDensity1/totalDensity));
            var scoreChange2 = k*((double) result2 - (expectationDensity2/totalDensity));

            player1.Rating = startScore1 + scoreChange1;
            player2.Rating = startScore2 + scoreChange2;

            Console.WriteLine("{0} vs {1}: {2} wins!  \n  New scores: {0} = {3}; {1} = {4}",
                              player1.BotName, player2.BotName, result1 == 1 ? player1.BotName : player2.BotName,
                              player1.Rating, player2.Rating);

            DBAccess.RecordConnect2Result(player1, player2, result1, result2, scoreChange1, scoreChange2, json);
        }
    }
}
