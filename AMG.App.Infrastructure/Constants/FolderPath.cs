using System.IO;

namespace AMG.App.Infrastructure.Constants
{
    public class FolderPath
    {
        public static string StaticFilePath = Path.Combine(new string[] { Directory.GetCurrentDirectory(), "wwwroot" });
        public const string ImagePath = "Images";
        public const string TemplatePath = "Template";
    }
}