using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBCRUD.Models;
using RestSharp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MongoDBCRUD
{
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;
        private readonly MongoClient client;
        private readonly IMongoDatabase database;

        public BookController(ILogger<BookController> logger, IBookstoreDatabaseSettings settings)
        {
            _logger = logger;
            client = new MongoClient(settings.ConnectionString);
            database = client.GetDatabase(settings.DatabaseName);
        }
        // GET: api/<controller>

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("CreateBookCollection")]
        public string CreateBookCollection()
        {
            string msg = "collection exists";
            if (!CollectionExists("Books"))
            {
                database.CreateCollection("Books");
                msg = "Books collection created";
            }
            return msg;

        }

        [HttpGet("GetAllBooks")]
        public string GetAllBooks()
        {
            var books = database.GetCollection<Book>("Books").Find<Book>(x => true);
            return books.ToList<Book>().ToJson();
        }

        // GET api/<controller>/5
        [HttpGet("GetBookByName/{name}")]
        public string GetBookByName(string name)
        {
            string result = string.Empty;
            var filter = Builders<BsonDocument>.Filter.Eq("Name", name);
            var books = database.GetCollection<BsonDocument>("Books");
            var doc = books.Find(filter).FirstOrDefault();
            if (doc == null)
            {
                result = "No books found with this name";
            }
            else
            {
                result = doc.ToJson();
            }
            return result;
        }

        // POST api/<controller>
        [HttpPost("SaveBookDetails")]
        public Book SaveBookDetails(Book book)
        {
            if(book != null && book.Name!=null)
            database.GetCollection<Book>("Books").InsertOne(book);
            return book;
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
