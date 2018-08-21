using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryFileReader
{
    public class BookEntry
    {
        public string FileID { get; set; }
        public string AuthorName { get; set; }
        public string BookTitle { get; set; }
        public Int32 QuantityInStock { get; set; }
        public Double Price { get; set; }
    }
}
