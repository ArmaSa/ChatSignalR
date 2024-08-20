using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ApplicationSettings
    {
        public string AllowedHosts { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public Logging Logging { get; set; }
    }
    public class ConnectionStrings
    {
        public string SqlServer { get; set; }
    }
    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }
    public class LogLevel
    {
        public string Default { get; set; }
        public string Hangfire { get; set; }
    }
}