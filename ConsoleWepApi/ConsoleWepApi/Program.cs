using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
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
        private static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            var menu = "0";
            do { 
                Console.WriteLine("1> Добавить автора.");
                Console.WriteLine("2> Список авторов.");
                Console.WriteLine("3> Удалить автора");
                Console.WriteLine("0> Выход.");
                Console.Write(">");
                menu = Console.ReadLine();

                switch (menu) {
                    case "1":
                        Console.Clear();
                        AuthorModel newAuthor = new AuthorModel();
                        Console.Write("Имя: ");
                        newAuthor.FirstName = Console.ReadLine();
                        Console.Write("Фамилия: ");
                        newAuthor.LastName = Console.ReadLine();
                        CreateAuthor(newAuthor);
                        break;
                    case "2":
                        Console.Clear();
                        LoadContent();
                        Console.ReadKey();
                        break;
                    case "3":
                        Console.Clear();
                        LoadContent();
                        Console.Write("\n\nnВведите Id Автора из списка:");
                        var id = 0;
                        int.TryParse(Console.ReadLine(),out id);
                        DeleteAuthor(id);
                        break;
                    default:
                        break;
                        
                }
                Console.Clear();
            } while (!menu.Equals("0"));


        }


        static async void LoadContent()
        {
            var response = await client.GetAsync("http://localhost:60719/api/AuthorApi");

            List<AuthorModel> authors = JsonConvert.DeserializeObject<List<AuthorModel>>(response.Content.ReadAsStringAsync().Result);
            foreach (var item in authors)
            {
                Console.WriteLine(item.Id + "\t" + item.FirstName + "\t" + item.LastName);
            }
          
        }

        static async void CreateAuthor(AuthorModel obj)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            await client.PostAsync("http://localhost:60719/api/AuthorApi", content);
        }

        static async void DeleteAuthor(int id)
        {
          await client.DeleteAsync("http://localhost:60719/api/AuthorApi/"+id);
        }
    }
}
