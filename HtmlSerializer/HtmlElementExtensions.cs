using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    static class HtmlElementExtensions
    {
      
        public static bool IsMatch(HtmlElement element, Selector selector)
        {
            if (!string.IsNullOrEmpty(selector.TagName) && selector.TagName != element.Name)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(selector.Id) && selector.Id != element.Id)
            {
                return false;
            }
            if (selector.Classes != null && selector.Classes.Any() &&
                (element.Classes == null || !selector.Classes.All(c => element.Classes.Contains(c))))
            {
                return false;
            }
            return true;

        }
        public static HashSet<HtmlElement> QuerySelector(this HtmlElement htmlElement, Selector selector, HtmlElement dom)
        {
            var result=new HashSet<HtmlElement>();
            QuerySelectorRecursive(selector, htmlElement, result);
            return result;
        }
        private static void QuerySelectorRecursive(Selector selector, HtmlElement element, HashSet<HtmlElement> result)
        {
            if (selector == null) return;

            var matchingChildren = element.Descendants().Where(child => IsMatch(child, selector)).ToList();

            if (!selector.Children.Any())
            {
                result.UnionWith(matchingChildren);
                return;
            }

            foreach (var child in matchingChildren)
                QuerySelectorRecursive(selector.Children.First(), child, result);
        }
    }
}
