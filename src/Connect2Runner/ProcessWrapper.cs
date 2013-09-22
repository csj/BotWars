using System.Diagnostics;

namespace BotWars.Connect2
{
    public class ProcessWrapper
    {
        public ProcessWrapper(Process process, string botName, string authorName)
        {
            Process = process;
            BotName = botName;
            AuthorName = authorName;
        }

        public Process Process { get; private set; }
        public string BotName { get; private set; }
        public string AuthorName { get; private set; }
    }
}
