using Microsoft.Extensions.Configuration;

namespace Configuration
{
    public static class ConfigurationSectionExtensions
    {
        public static TResult Initialize<TResult>(this IConfigurationSection configurationSection) where TResult : ConfigurationBase
        {
            TResult configuration = configurationSection.Get<TResult>();
            ConfigurationValidator.TryValidateObject(configuration);

            return configuration;
        }
    }
}