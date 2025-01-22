

using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Xml.Linq;
using HtmlSerializer;

DomSerializer domSerializer = new DomSerializer();

var html = await domSerializer.Load("https://learn.microsoft.com/en-us/dotnet/csharp/");
var dom = domSerializer.Serialize(html);

var resultQuery = dom.QuerySelector(Selector.ParseQuery("div p.card-supertitle"), dom);

resultQuery.ToList().ForEach(e => Console.WriteLine(e));




