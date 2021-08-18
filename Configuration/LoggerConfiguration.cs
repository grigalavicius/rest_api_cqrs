namespace Configuration
{
    public class LoggerConfiguration: ConfigurationBase
    {
        public bool WipeLogBeforeStart { get; set; }
        public string Directory { get; set; }
    }
}