using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBCRUD.Models;
using Newtonsoft.Json;
using RestSharp;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MongoDBCRUD
{
    public class Book1Controller : Controller
    {
        private readonly ILogger<Book1Controller> _logger;
        private readonly MongoClient client;
        private readonly IMongoDatabase database;

        public Book1Controller(ILogger<Book1Controller> logger, IBookstoreDatabaseSettings settings)
        {
            _logger = logger;
            client = new MongoClient(settings.ConnectionString);
            database = client.GetDatabase(settings.DatabaseName);
        }

        
    }
}
