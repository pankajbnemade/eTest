namespace ERP.Models.Extension
{
    /// <summary>
    /// application configuration app settings.
    /// </summary>
    public class AppSettings
    {
        public string ErplanConnString { get; set; }
        public Enviroment Enviroment { get; set; }
    }

    /// <summary>
    /// enviroment.
    /// </summary>
    public class Enviroment
    {
        public AppEnviroment AppEnviroment { get; set; }
    }

    /// <summary>
    /// application enviroment.
    /// </summary>
    public class AppEnviroment
    {
        public string EnvType { get; set; }
    }
}
