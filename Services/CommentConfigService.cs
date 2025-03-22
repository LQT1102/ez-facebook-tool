using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using EZ.FACEBOOK.TOOL.Models;

namespace EZ.FACEBOOK.TOOL.Services
{
    public class CommentConfigService
    {
        private const string ConfigFilePath = "./data/comment_config.json";

        public async Task SaveConfigAsync(CommentConfig config)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigFilePath));
            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(config, jsonOptions);
            await File.WriteAllTextAsync(ConfigFilePath, jsonString);
        }

        public async Task<CommentConfig> LoadConfigAsync()
        {
            if (!File.Exists(ConfigFilePath))
            {
                return new CommentConfig();
            }

            var jsonString = await File.ReadAllTextAsync(ConfigFilePath);
            return JsonSerializer.Deserialize<CommentConfig>(jsonString) ?? new CommentConfig();
        }
    }
} 