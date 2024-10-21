
using System.Reflection;

namespace Server.Helper;

public class ApplicationSettings
{
    private ConfigurationBuilder configurationBuilder;
    private readonly IConfiguration configuration;
    public ApplicationSettings()
    {
        configurationBuilder = new ConfigurationBuilder();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        configurationBuilder.AddJsonFile(path, false);
        configuration = configurationBuilder.Build();
    }
    public object GetConfiguration(string section, string key, object obj)
    {
        PropertyInfo propertyInfo = obj.GetType().GetProperty(key);
        var configValue = configuration[$"{section}:{key}"];

        if (propertyInfo != null)
        {
            propertyInfo.SetValue(obj, configValue, null);
        }
        return obj;
    }
    public string GetConfigurationValue(string section, string key)
    {
        return configuration[$"{section}:{key}"];
    }

}