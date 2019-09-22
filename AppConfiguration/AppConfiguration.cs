using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace AppConfiguration
{
    public class AppConfiguration
    {
        public static IConfiguration _configuration;

        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetConfiguration(string key) => _configuration.GetValue<string>(key);

        public static string GetConfiguration(IConfigurationSection configSection, string key) => configSection.GetValue<string>(key);

        public static IConfigurationSection GetSection(string key) => _configuration.GetSection(key);


        public static async Task<string> GetDbConnStringFromAzureKeyVault()
        {
            var connectionString = string.Empty;

            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            var sqlSecret = await keyVaultClient.GetSecretAsync(DbConnIdentifier).ConfigureAwait(false);
            connectionString = sqlSecret.Value;

            return connectionString;

        }

        public static string DbConnIdentifier
        {
            get
            {
                var result = GetConfiguration("DbConnIdentifier");
                return result;
            }
        }

        public static class AzureKeyVaultUseClientApi
        {
            static IConfigurationSection section = GetSection("AzureKeyVaultUseClientApi");
            public static string GetConnectionSecret()
            {
                return GetConfiguration(section, "YourConnectionSecret");
            }
        }


    }
}
