using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public class HtmlTreeBuilder
    {
        public HtmlElement BuildHtmlTree(List<string> htmlLines)
        {
            var root = new HtmlElement { Name = "root" };
            var currentElement = root;
            var htmlHelper = HtmlHelper.Instance;

            foreach (var line in htmlLines)
            {
                var tagName = GetTagName(line);

                if (line == "/html")
                    break;

                if (line.StartsWith("/"))
                {
                    // Closing tag
                    if (currentElement.Parent != null)
                    {
                        currentElement = currentElement.Parent;
                    }
                }
                else if (htmlHelper.Tags.Contains(tagName))
                {
                    var newElement = CreateHtmlElement(line);
                    currentElement.Children.Add(newElement);
                    newElement.Parent = currentElement;

                    if (!htmlHelper.TagsVoid.Contains(tagName))
                        currentElement = newElement;
                }
                else
                {
                    // Text content
                    currentElement.InnerHtml = (currentElement.InnerHtml ?? "") + line;
                }
            }

            return root;
        }

        private string GetTagName(string line)
        {
            var match = Regex.Match(line, "^([a-zA-Z0-9]+)");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }
        private Dictionary<string, string> GetAttributes(string line)
        {
            var attributes = new Dictionary<string, string>();

            var matches = Regex.Matches(line, "([a-zA-Z0-9-]+)\\s*=\\s*\\\"(.*?)\\\"");
            foreach (Match match in matches)
            {
                attributes[match.Groups[1].Value] = match.Groups[2].Value;
            }

            return attributes;
        }     

        private Dictionary<string, string> GetFilteredAttributes(Dictionary<string, string> attributes)
        {
          return  attributes
                .Where(a => a.Key != "id" && a.Key != "class")
                .ToDictionary(a => a.Key, a => a.Value);
        }
        private HtmlElement CreateHtmlElement(string line)
        {
            var tagName = GetTagName(line);
            var attributes = GetAttributes(line);
            var filteredAttributes =GetFilteredAttributes(attributes);

            return new HtmlElement
            {
                Name = tagName,
                Id = attributes.ContainsKey("id") ? attributes["id"] : null,
                Classes = attributes.ContainsKey("class") ? attributes["class"].Split(' ').ToList() : new List<string>(),
                Attributes = filteredAttributes, 
                Children = new List<HtmlElement>()
            };
        }
    }
}
