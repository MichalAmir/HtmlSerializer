﻿using System;
using System.IO;
using System.Text.Json;

namespace Clean.Console
{
    public class HtmlHelper
    {
        public string[] AllTags { get; set; }
        public string[] SelfClosingTags { get; set; }


        private readonly static HtmlHelper instanc_ = new HtmlHelper();
        public static HtmlHelper Instance => instanc_;

        private HtmlHelper()
        {

            var htmlTagsJson = File.ReadAllText("HtmlTags.json");
            AllTags = JsonSerializer.Deserialize<string[]>(htmlTagsJson);

            var selfClosingTagsJson = File.ReadAllText("HtmlVoidTags.json");
            SelfClosingTags = JsonSerializer.Deserialize<string[]>(selfClosingTagsJson);
        }

    }

}
