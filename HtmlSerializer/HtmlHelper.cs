using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public class HtmlHelper
    {
        private static HtmlHelper _instance = new HtmlHelper();

        public static HtmlHelper Instance => _instance;
        public List<string> Tags { get;private set; }
        public List<string> TagsVoid { get;private set; }
        private HtmlHelper()
        {
            Tags = LoadTags("JsonFiles/HtmlTags.json");
            TagsVoid = LoadTags("JsonFiles/HtmlVoidTags.json");
        }

        private static List<string> LoadTags(string filePath)
        {
            try
            {
                var content = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<string>>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading file '{filePath}': {ex.Message}");
                return new List<string>();
            }
        }
        


    }
}
