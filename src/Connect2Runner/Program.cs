using System.Diagnostics;

namespace BotWars.Connect2
{
    public class Program
    {
        static void Main(string[] args)
        {
            //args = new[]
            //    {
            //        "AlwaysPlay4.exe", "Always Play 4", "csj",
            //        "PlayRandom.exe", "Random", "csj"
            //    };

            var process1 = Process.Start(new ProcessStartInfo
                {
                    FileName = args[0],
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                });
            
            var process2 = Process.Start(new ProcessStartInfo
                {
                    FileName = args[3],
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                });

            var wrapper1 = new ProcessWrapper(process1, args[1], args[2]);
            var wrapper2 = new ProcessWrapper(process2, args[4], args[5]);

            new Connect2Runner(new[] {wrapper1, wrapper2}).Run();

            if (!process1.HasExited) process1.Kill();
            if (!process2.HasExited) process2.Kill();
        }
    }

}