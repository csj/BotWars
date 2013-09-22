using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace BotWars.Connect2
{
    public class Connect2Runner  : Runner
    {
        private readonly int[,] _field = new int[8,9];  // all 0; with padding
        private readonly List<int> _moves = new List<int>();

        public Connect2Runner(IEnumerable<ProcessWrapper> playerWrappers) : base(playerWrappers)
        {
        }

        protected override JObject Play()
        {
            var turn = 0;
            
            while (true)
            {
                var move = GetMove(turn);
                if (move == null)
                    return Result(1 - turn);

                if (!ExecuteTurn(turn, move.Value))
                    return Result(1 - turn);

                _moves.Add(move.Value);
                EchoMove(1 - turn, move.Value);

                if (GameIsOver(turn))
                    return Result(turn);

                turn = 1 - turn;
            }
        }

        private bool GameIsOver(int turn)
        {
            for (int i = 1; i <= 6; i++)
            {
                for (int j = 1; j <= 7; j++)
                {
                    if (_field[i, j] != turn + 1) continue;
                    
                    // check 8 directions
                    if (_field[i - 1, j - 1] == turn + 1) return true;
                    if (_field[i - 1, j + 0] == turn + 1) return true;
                    if (_field[i - 1, j + 1] == turn + 1) return true;
                    if (_field[i + 0, j - 1] == turn + 1) return true;
                    //if (_field[i + 0, j + 0] == turn + 1) return true;
                    if (_field[i + 0, j + 1] == turn + 1) return true;
                    if (_field[i + 1, j - 1] == turn + 1) return true;
                    if (_field[i + 1, j + 0] == turn + 1) return true;
                    if (_field[i + 1, j + 1] == turn + 1) return true;
                }
            }

            return false;
        }

        private void EchoMove(int opponent, int move)
        {
            _players[opponent].StandardInput.WriteLine("Opponent played " + move);
        }

        private JObject Result(int winner)
        {
            return new JObject(
                new JProperty("Connect2",
                              new JObject(
                                  new JProperty("Moves", new JArray(_moves)),
                                  new JProperty("Results", new JArray(winner == 0 ? new[] {1, 0} : new[] {0, 1}))
                                  )
                    )
                );
        }

        /// <returns>true if player made a valid move</returns>
        private bool ExecuteTurn(int turn, int move)
        {
            if (move < 1 || move > 7)
            {
                _players[turn].StandardInput.WriteLine("# Input not between 1-7; aborting");
                return false;
            }

            for (int i = 1; i <= 6; i++)
            {
                if (_field[i, move] == 0)
                {
                    _field[i, move] = turn + 1; // 1 or 2
                    return true;
                }
            }

            _players[turn].StandardInput.WriteLine("# Column " + move + " is full; aborting");
            return false;
        }

        private int? GetMove(int turn)
        {
            _players[turn].StandardInput.WriteLine("Move");
            var line = _players[turn].StandardOutput.ReadLine();
            int result;
            if (int.TryParse(line, out result)) return result;
            
            _players[turn].StandardInput.WriteLine("# Input not accepted: " + line + "; aborting");
            return null;
        }
    }
}