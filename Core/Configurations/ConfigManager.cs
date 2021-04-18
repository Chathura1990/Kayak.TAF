using Core.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.IO;

namespace Core.Configurations
{
    public static class ConfigManager
    {
        private static readonly IConfigurationRoot ConfigurationRoot = GetConfiguration();
        public static readonly BrowserConfiguration Browser = BindConfiguration<BrowserConfiguration>();
        public static readonly WaitConfiguration Wait = BindConfiguration<WaitConfiguration>();

        /// <summary>
        /// Get property from appsettings.json.
        /// For 2-level properties, like that:
        /// "Browser" : { "StartUrl": "some.url" }
        /// the following key should be used:
        /// GetValue("Browser:StartUrl").
        /// </summary>
        public static string GetValue(string key)
        {
            return ConfigurationRoot[key];
        }

        /// <summary>
        /// Bind section of json to configuration object.
        /// </summary>
        /// <typeparam name="T">class of the object, that should be created.</typeparam>
        public static T BindConfiguration<T>() where T : IConfiguration
        {
            T config = Activator.CreateInstance<T>();
            ConfigurationRoot.GetSection(config.JsonSectionName).Bind(config);
            return config;
        }

        /// <summary>
        /// Method declares the order of configuration resources, that should be used to extract configuration data.
        /// If the same configuration key is specified in several resources, than value is overriden in the last specified resource.
        /// Method can be updated to read data from console, azure and so on.
        /// <see cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.1"/>
        /// </summary>
        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
#if DEV
            .AddJsonFile("appsettings.DEV.json", true, true)
#endif
            .Build();
        }
    }
}