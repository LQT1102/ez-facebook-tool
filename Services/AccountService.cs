using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EZ.FACEBOOK.TOOL.Models;

namespace EZ.FACEBOOK.TOOL.Services
{
    public class AccountService
    {
        private const string ConfigFilePath = "./data/accounts.json";

        public async Task SaveAccountsAsync(List<Account> accounts)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigFilePath));
            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(accounts, jsonOptions);
            await File.WriteAllTextAsync(ConfigFilePath, jsonString);
        }

        public async Task<List<Account>> LoadAccountsAsync()
        {
            if (!File.Exists(ConfigFilePath))
            {
                return new List<Account>();
            }

            var jsonString = await File.ReadAllTextAsync(ConfigFilePath);
            return JsonSerializer.Deserialize<List<Account>>(jsonString) ?? new List<Account>();
        }

        public List<Account> ParseAccountsFromText(string text)
        {
            return text.Split('\n')
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line =>
                {
                    var parts = line.Split('|');
                    return new Account
                    {
                        Username = parts.Length > 0 ? parts[0].Trim() : "",
                        Password = parts.Length > 1 ? parts[1].Trim() : "",
                        Status = "Sẵn sàng"
                    };
                })
                .ToList();
        }
    }
} 