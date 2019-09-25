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
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static List<UserModel> users;
        private static List<BookModel> books;
        private static List<AuthorBook> userBooks;
        static void Main(string[] args)
        {
           
            var menu = "0";
            do
            {
                Task load = LoadProgram();
                load.Wait();
                Console.WriteLine("1> Выдать книгу.");
                Console.WriteLine("2> Вернуть книгу.");
                Console.WriteLine("3> Список должников");
                Console.WriteLine("0> Выход.");
                Console.Write(">");
                menu = Console.ReadLine();

                switch (menu)
                {
                    case "1":
                        Console.Clear();
                        AuthorBook authorBook = new AuthorBook();
                        PrintBooks();
                        Console.WriteLine();
                        Console.Write("Введите номер книги (0 - выход): ");
                        int answer = 0;
                        int.TryParse(Console.ReadLine(), out answer);
                        if (answer == 0)
                            break;
                        authorBook.BooksId = answer;
                        Console.Clear();
                        PrintUsers();
                        Console.WriteLine();
                        Console.Write("Введите номер пользователя (0 - выход): ");
                        int answer2 = 0;
                        int.TryParse(Console.ReadLine(), out answer2);
                        if (answer2 == 0)
                            break;
                        authorBook.UserId = answer2;
                        authorBook.AuthorId = books.Find(i => i.Id == authorBook.BooksId).AuthorId;
                        authorBook.AuthorName = books.Find(i => i.Id == authorBook.BooksId).AuthorName;
                        authorBook.UserName = users.Find(i => i.Id == authorBook.UserId).Name;
                        authorBook.BooksName = books.Find(i => i.Id == authorBook.BooksId).Title;
                        Console.Clear();
                        Console.Write("Введите количество дней аренды (0 - выход): ");
                        int answer3 = 0;
                        int.TryParse(Console.ReadLine(), out answer3);
                        if (answer3 == 0)
                            break;
                        authorBook.DateOrder = DateTime.Now.AddDays(answer3);
                        CreateUserBook(authorBook);
                        Console.Clear();
                        Console.WriteLine("Заказ успешно оформлен!");
                        Console.ReadKey();
                        break;
                    case "2":
                        Console.Clear();
                        PrintUsersBooks();
                        Console.WriteLine();
                        Console.Write("Введите номер заказа (0 - выход): ");
                        int answer4 = 0;
                        int.TryParse(Console.ReadLine(), out answer4);
                        if (answer4 == 0)
                            break;
                        DeleteUserBook(answer4);
                        Console.Clear();
                        Console.WriteLine("Заказ успешно удален!");
                        Console.ReadKey();
                        break;
                    case "3":
                        Console.Clear();
                        PrintUsersBooksDebtor();
                        Console.ReadKey();
                        break;
                    default:
                        break;

                }
                Console.Clear();
            } while (!menu.Equals("0"));


        }

        static async Task<bool> LoadProgram()
        {
            var response = await client.GetAsync("http://localhost:60719/api/UserApi");
            users = JsonConvert.DeserializeObject<List<UserModel>>(response.Content.ReadAsStringAsync().Result);
            response = await client.GetAsync("http://localhost:60719/api/BookApi");
            books = JsonConvert.DeserializeObject<List<BookModel>>(response.Content.ReadAsStringAsync().Result);
            response = await client.GetAsync("http://localhost:60719/api/UserBooksApi");
            userBooks = JsonConvert.DeserializeObject<List<AuthorBook>>(response.Content.ReadAsStringAsync().Result);
            return true;
        }

        static void PrintBooks()
        {
            var formatString = string.Format("{{0, -{0}}}", 10);
            Console.Write(formatString, "№");
            Console.Write(formatString, "Title");
            Console.Write(formatString, "Author");
            Console.Write(formatString, "Genre");
            Console.Write(formatString, "Pages");
            Console.Write(formatString, "Price");
            Console.WriteLine();
            Console.WriteLine();
            foreach (var s in books)
            {
                Console.Write(formatString, s.Id);
                Console.Write(formatString, s.Title);
                Console.Write(formatString, s.AuthorName);
                Console.Write(formatString, s.GenreName);
                Console.Write(formatString, s.Pages);
                Console.Write(formatString, s.Price);
                Console.WriteLine();
            }         
        }

        static void PrintUsers()
        {
            var formatString = string.Format("{{0, -{0}}}", 10);
            Console.Write(formatString, "№");
            Console.Write(formatString, "Name");
            Console.Write(formatString, "Email");
            Console.WriteLine();
            Console.WriteLine();
            foreach (var s in users)
            {
                Console.Write(formatString, s.Id);
                Console.Write(formatString, s.Name);
                Console.Write(formatString, s.Email);
                Console.WriteLine();
            }
        }

        static void PrintUsersBooks()
        {
            var formatString = string.Format("{{0, -{0}}}", 10);
            Console.Write(formatString, "№");
            Console.Write(formatString, "Title");
            Console.Write(formatString, "Author");
            Console.Write(formatString, "User");
            Console.Write(formatString, "Date return");
            Console.WriteLine();
            Console.WriteLine();
            foreach (var s in userBooks)
            {
                Console.Write(formatString, s.Id);
                Console.Write(formatString, s.BooksName);
                Console.Write(formatString, s.AuthorName);
                Console.Write(formatString, s.UserName);
                Console.Write(formatString, s.DateOrder.ToShortDateString());
                Console.WriteLine();
            }
        }

        static void PrintUsersBooksDebtor()
        {
            var formatString = string.Format("{{0, -{0}}}", 10);
            Console.Write(formatString, "№");
            Console.Write(formatString, "Title");
            Console.Write(formatString, "Author");
            Console.Write(formatString, "User");
            Console.Write(formatString, "Date return");
            Console.WriteLine();
            Console.WriteLine();
            foreach (var s in userBooks.Where(i=>i.DateOrder<DateTime.Now))
            {
                Console.Write(formatString, s.Id);
                Console.Write(formatString, s.BooksName);
                Console.Write(formatString, s.AuthorName);
                Console.Write(formatString, s.UserName);
                Console.Write(formatString, s.DateOrder.ToShortDateString());
                Console.WriteLine();
            }
        }

        static async void CreateUserBook(AuthorBook obj)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            await client.PostAsync("http://localhost:60719/api/UserBooksApi", content);
        }

        static async void DeleteUserBook(int id)
        {
          await client.DeleteAsync("http://localhost:60719/api/UserBooksApi/" + id);
        }
    }
}
