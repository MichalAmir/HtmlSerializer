using Clean.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Clean.Console
{
    public class TreeBuilder
    {
        public static HtmlElement BuildTree(List<string> htmlLines)
        {
            var htmlHelper = HtmlHelper.Instance;
            HtmlElement root = null;
            HtmlElement current = root;
            HtmlElement prev = null;
            var openTagsStack = new Stack<HtmlElement>();

            foreach (string line in htmlLines)
            {
                if (line.StartsWith("!DOCTYPE"))
                    continue;

                if (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line))
                    continue;

                var firstWord = line.Split(' ')[0];

                if (firstWord == "/html")
                    break;

                if (firstWord.StartsWith("/"))
                {
                    current = openTagsStack.Pop().Parent;
                }
                else
                {
                    var newElement = new HtmlElement();
                    newElement.Name = firstWord;

                    var attributeList = new Regex("([^\\s]+)=\"(.+?)\"").Matches(line);
                    foreach (Match attribute in attributeList)
                    {
                        newElement.Attributes.Add(attribute.Value);
                        string attributeName = attribute.Groups[1].Value;
                        string attributeValue = attribute.Groups[2].Value;

                        if (attributeName.ToLower() == "class")
                        {
                            var listClass = attributeValue.Split(' ').ToList();
                            foreach (string listItem in listClass)
                            {
                                newElement.Classes.Add(listItem);
                            }
                        }
                        else if (attributeName.ToLower() == "id")
                        {
                            var id = attributeValue;
                            newElement.Id = id;
                        }
                    }

                    if (htmlHelper.AllTags.Contains(firstWord) && !htmlHelper.SelfClosingTags.Contains(firstWord))
                    {
                        prev = current;
                        current = newElement;
                        if (root == null)
                        {
                            root = current;
                            root.Parent = null;
                            openTagsStack.Push(current);
                        }
                        else
                        {
                            // Push open tags onto the stack
                            if (openTagsStack.Count == 0 || openTagsStack.Peek() != current)
                            {
                                openTagsStack.Push(current);
                                prev.Children.Add(newElement);
                                newElement.Parent = prev;
                            }
                            else
                            {
                                current.Children.Add(newElement);
                                newElement.Parent = current;
                                current = newElement;
                            }
                        }

                    }

                    else
                    {
                        if (htmlHelper.SelfClosingTags.Contains(firstWord))
                        {
                            // Self-closing tag
                            current.Children.Add(newElement);
                            newElement.Parent = current;
                        }
                        else
                        {
                            current.InnerHtml += line;
                        }
                    }

                }
            }
            return root;
        }
    }
}
