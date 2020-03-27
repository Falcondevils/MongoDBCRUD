using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDBCRUD.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoDBCRUD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MongoClient client;
        private readonly IMongoDatabase database;

        public HomeController(ILogger<HomeController> logger, IBookstoreDatabaseSettings settings)
        {
            _logger = logger;
            client = new MongoClient(settings.ConnectionString);
            database = client.GetDatabase(settings.DatabaseName);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public bool GetUser()
        {
            
            string url = "mongodb://localhost:27017/test";
            var client = new MongoClient(url);
            var database = client.GetDatabase("test");
            //database.GetCollection("Users");

            return true;
        }


        [HttpGet]
        public string CreateBookCollection()
        {
            string msg = "collection exists";
            if (!CollectionExists("Users"))
            {
                database.CreateCollection("Books");
                msg = "Books collection created";
            }
            return msg;

        }

        [HttpGet("GetAllBooks")]
        public string GetAllBooks()
        {
            return database.GetCollection<Book>("Books").Find<Book>(x => x.Name == x.Name).ToJson();
        }

        // GET api/<controller>/5
        [HttpGet("GetBookById/{id}")]
        public string GetBookById(string id)
        {
            return database.GetCollection<Book>("Books").Find<Book>(x => x.Id == id).ToJson();
        }

        // POST api/<controller>
        [HttpPost("SaveBookDetails")]
        public Book SaveBookDetails(Book book)
        {
            database.GetCollection<Book>("Books").InsertOne(book);
            return book;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("DeleteBook/{id}")]
        public bool DeleteBook(string id)
        {
            database.GetCollection<Book>("Books").DeleteOne(book => book.Id == id);
            return true;
        }

        public bool CollectionExists(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var options = new ListCollectionNamesOptions { Filter = filter };

            return database.ListCollectionNames(options).Any();
        }
    }
}
