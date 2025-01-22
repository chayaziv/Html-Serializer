using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public class DomSerializer
    {
        private static readonly HttpClient _httpClient = new();

        public async Task<string> Load(string url) =>
            await _httpClient.GetStringAsync(url);

        public HtmlElement Serialize(string html)
        {
            var htmlLines = Regex.Split(html, "<(.*?)>")
                                 .Select(line => line.Trim())
                                 .Where(line => !string.IsNullOrWhiteSpace(line))
                                 .ToList();

            return new HtmlTreeBuilder().BuildHtmlTree(htmlLines);
        }

    }
}
