using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWepApi
{
    public class AuthorModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:60719/api/AuthorApi");
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    string result = rd.ReadToEnd();
                    List<AuthorModel> authors = JsonConvert.DeserializeObject<List<AuthorModel>>(result);
                    foreach (var item in authors)
                    {
                        Console.WriteLine(item.Id + "\t" + item.FirstName + "\t" + item.LastName);
                    }
                }
            }
        }
    }
}
