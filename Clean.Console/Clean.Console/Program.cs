using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Clean.Console;
using HtmlSerializer;

class Program
{
    static async Task Main(string[] args)
    {
        HtmlElement rootElement = null;

        var html = await Load("https://yourweddingcountdown.com/0f887");

        var cleanHtml = new Regex("\\s").Replace(html, " ");
        var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0).ToList();
        Console.WriteLine(htmlLines);
        TreeBuilder builder = new TreeBuilder();
        HtmlElement root;
        root = TreeBuilder.BuildTree(htmlLines);
        Selector selectorTree = Selector.Create("br.clear");
        var res = root.GetElementsBySelector(selectorTree);
        var result = new HashSet<HtmlElement>(res);
        foreach (var item in result)
        {
            Console.WriteLine($" name: {item.Name}");
            if (item.Attributes.Count > 0)
            {
                Console.WriteLine("attributes list: ");
                foreach (var attribute in item.Attributes)
                {
                    Console.WriteLine($"{attribute} ,");
                }
            }
            if (item.Classes.Count > 0)
            {
                Console.WriteLine("classes list: ");
                foreach (var class_ in item.Classes)
                {
                    Console.WriteLine(class_);
                }
            }
            if (item.Id != null)
            {
                Console.WriteLine($"id : {item.Id}");
            }
            Console.WriteLine($"parent: {item.Parent.Name}");
            Console.WriteLine();
        }
    }

    static async Task<string> Load(string url)
    {
        HttpClient client = new HttpClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        return html;
    }

    static void PrintTree(HtmlElement root, string indent)
    {
        Console.WriteLine($"root {root.Name}");
        if (root != null)
        {
            PrintNodeDetails(root, indent);

            foreach (var child in root.Children)
            {
                PrintTree(child, indent);
            }
        }
    }

    static void PrintNodeDetails(HtmlElement element, string indent)
    {
        Console.WriteLine($"{indent}Tag name: {element.Name}");
        Console.WriteLine($"{indent}Id: {element.Id ?? "None"}");
        Console.WriteLine($"{indent}Classes: {string.Join(", ", element.Classes)}");
        Console.WriteLine($"{indent}Attributes: {string.Join(", ", element.Attributes)}");
        Console.WriteLine($"{indent}Inner HTML: {element.InnerHtml ?? "None"}");
        Console.WriteLine($"{indent}Parent: {element.Parent?.Name ?? "None"}");

        if (element.Children.Count > 0)
        {
            Console.WriteLine($"{indent}Children:");
            foreach (var child in element.Children)
            {
                Console.WriteLine($"{indent}  {child.Name}");
            }
        }
        Console.WriteLine();
    }
}