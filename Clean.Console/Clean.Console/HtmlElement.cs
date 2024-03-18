using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Clean.Console
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement()
        {
            Name = "";
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
            InnerHtml = "";
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            HtmlElement currentElement;
            while (queue.Count > 0)
            {
                currentElement = queue.Dequeue();
                yield return currentElement;

                if (currentElement != null)
                {
                    if (currentElement.Children == null)
                    {
                        continue;
                    }
                    else
                    {
                        if (currentElement.Children.Count > 0)
                        {
                            foreach (var child in currentElement.Children)
                            {
                                queue.Enqueue(child);
                            }
                        }
                    }
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors(HtmlElement element)
        {
            HtmlElement current = element;
            while (current.Parent != null)
            {
                yield return current.Parent;
                current = current.Parent;
            }
        }
    }
}