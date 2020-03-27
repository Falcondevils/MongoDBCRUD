using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBCRUD.Models
{
    public class BookInput
    {
        public string Name { get; set; }

        public string Price { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }
    }
}
