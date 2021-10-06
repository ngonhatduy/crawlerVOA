using System;
using System.Net.Http;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft;
using Newtonsoft.Json.Linq;

namespace Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            startCrawler();
            Console.ReadLine();
        }

        //Add from master

        public static async Task startCrawler()
        {
            //set url to crawler
            var url = "http://www.manythings.org/voa/scripts/";

            //khởi tạo bất đồng bộ - async, await
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            //Bóc tách theo html
            var uls = htmlDocument.DocumentNode.Descendants("ul").Where(node => node.GetAttributeValue("class", "").Equals("list")).ToList()[1].Descendants("li").ToList();

            //Create output Json
            var array = new JArray();
            foreach (var li in uls)
            {
                var title = "{\"Title\"" + ":" + "\"" + li.Descendants("a").FirstOrDefault().InnerText + "\"" + "}";
                var link = li.Descendants("a").FirstOrDefault().ChildAttributes("href").FirstOrDefault().Value;
                JObject titleJson = JObject.Parse(title);
                titleJson.Add("Link", JToken.FromObject(link));              
                array.Add(titleJson);
            }
            Console.WriteLine(array);
        }
    }
}
