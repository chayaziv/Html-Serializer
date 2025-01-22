using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new();
        public List<Selector> Children { get; set; } = new();
        public Selector Parent { get; set; }   
        public static Selector ParseQuery(string query)
        {
            string[] parts = query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Selector root = new Selector();
            Selector current = root;
            HtmlHelper htmlHelper = HtmlHelper.Instance;
            foreach (string part in parts)
            {
                Selector newSelector = new Selector();
                string[] components = Regex.Split(part, @"(?=[.#])").Where(x => !string.IsNullOrEmpty(x)).ToArray();
                foreach (string component in components)
                {                 
                    if (component.StartsWith("#"))
                    {
                        newSelector.Id = component[1..];
                    }
                    else if (component.StartsWith("."))
                    {
                        newSelector.Classes.Add(component[1..]);
                    }
                    else
                    {
                        if (htmlHelper.Tags.Contains(component))
                        {
                            newSelector.TagName = component;
                        }
                        else
                        {
                            Console.WriteLine($"Invalid tag name: {component}");
                        }
                    }
                }
                newSelector.Parent = current;
                current.Children.Add(newSelector);
                current = newSelector;
            }

            return root.Children.FirstOrDefault();
        }

    }
}
