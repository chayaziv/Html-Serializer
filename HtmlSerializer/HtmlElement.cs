using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HtmlSerializer
{

    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new();
        public List<string> Classes { get; set; } = new();
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new();
        public override string ToString()
        {
            var attributes = new List<string>
            {
                Id is not null ? $"id={Id}" : null,
                Classes.Any() ? $"class=\"{string.Join(" ", Classes)}\"" : null
            }.Where(attr => attr != null);

            var openingTag = $"<{Name} {string.Join(" ", attributes).Trim()}>";
            var closingTag = InnerHtml != null ? $"{InnerHtml}</{Name}>" : "</>";
            return $"{openingTag}{closingTag}";
        }

        private IEnumerable<HtmlElement> GetChildren()
        {
            foreach (var child in Children)
            {
                yield return child;
            }
        }
        public List<HtmlElement> Descendants()
        {
            List<HtmlElement> result = new List<HtmlElement>();
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);  // Start with the current element (this)

            while (queue.Count > 0)
            {
                var currentElement = queue.Dequeue(); // Get the element at the front of the queue
                result.Add(currentElement); // Add it to the result list

                var children = currentElement.GetChildren();
                foreach (var child in children)
                {
                    queue.Enqueue(child);
                }
            }
            return result; 
        }    
        public List<HtmlElement> Ancestors()
        {
            List<HtmlElement> result = new List<HtmlElement>();
            result.Add(this);
            var currentElement = this.Parent;

            while (currentElement != null)
            {
                result.Add(currentElement); 
                currentElement = currentElement.Parent; 
            }

            return result;
        }

       
      
    }
}

