using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotWars.Connect2
{
    public abstract class Runner
    {
        private readonly List<ProcessWrapper> _playerWrappers;
        protected readonly List<Process> _players;

        protected Runner(IEnumerable<ProcessWrapper> playerWrappers)
        {
            _playerWrappers = playerWrappers.ToList();
            _players = playerWrappers.Select(p => p.Process).ToList();
        }

        public void Run()
        {
            var json = Play();

            // insert Players element
            json["Connect2"].First.AddBeforeSelf(
                new JProperty("Players",
                              new JArray(_playerWrappers.Select(p => JObject.FromObject(new
                                  {
                                      Name = p.BotName,
                                      Author = p.AuthorName
                                  })))));

            Console.WriteLine(json.ToString(Formatting.None));
        }

        protected abstract JObject Play();
    }
}