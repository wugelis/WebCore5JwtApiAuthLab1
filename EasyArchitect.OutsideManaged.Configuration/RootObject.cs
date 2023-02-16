namespace EasyArchitect.OutsideManaged.Configuration
{
    public class Rootobject
    {
        public AppSettings AppSettings { get; set; }
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public Connectionstrings ConnectionStrings { get; set; }
    }

    public class AppSettings
    {
        public string Secret { get; set; }
        public int? TimeoutMinutes { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }
        public string MicrosoftHostingLifetime { get; set; }
    }

    public class Connectionstrings
    {
        public string OutsideDbContext { get; set; }
    }

}