using System.Collections.Generic;

namespace EZ.FACEBOOK.TOOL.Models
{
    public class CommentConfig
    {
        public string CommentText { get; set; } = string.Empty;
        public List<string> GroupUrls { get; set; } = new List<string>();
        public int DelaySeconds { get; set; } = 5;
        public List<string> ImagePaths { get; set; } = new List<string>();
    }
} 