using System;
using System.IO;
using System.Threading.Tasks;

namespace Common.Logging
{
    public static class EfficiencyLogger
    {
        private const string LogLocation =
            @"H:\Departments\Business Services\IT\Tools\Support\Stats";

        public static void Log(
            string tool,
            string projectNumber = "",
            int filesProcessed = 0,
            int itemsProcessed = 0,
            int customStat1 = 0,
            int customStat2 = 0,
            int customStat3 = 0,
            int customStat4 = 0,
            int customStat5 = 0)
        {
            if (string.IsNullOrWhiteSpace(tool))
                return;

            string line = string.Join("|",
                DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                tool,
                Environment.UserName,
                projectNumber,
                filesProcessed,
                itemsProcessed,
                customStat1,
                customStat2,
                customStat3,
                customStat4,
                customStat5,
                Environment.MachineName);

            string fileName =
                $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss-fff}" +
                $"_LOG_{Guid.NewGuid():N}.txt";

            string fullPath = Path.Combine(LogLocation, fileName);

            _ = Task.Run(() =>
            {
                try
                {
                    File.WriteAllText(fullPath, line);
                }
                catch
                {
                    // Never allow logging failure to affect the add-in.
                }
            });
        }
    }
}