using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryFileReader
{
    
    public class BinaryReadWriteClass
    {
        public void LogWriter(BookEntry fileAlreadyStored, BookEntry duplicateFile)
        {
            var fileName = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "Log");
            fileName = fileName + "\\log.txt";
            if (!File.Exists(fileName))
            {
                using (StreamWriter userWriter = new StreamWriter(fileName))
                {
                    userWriter.WriteLine(DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToLongTimeString());
                    userWriter.WriteLine(fileAlreadyStored.FileID + " is a duplicate of file: " + duplicateFile.FileID);
                    userWriter.WriteLine();
                }
            }
            else
            {
                using (StreamWriter userWriter = File.AppendText(fileName))
                {
                    userWriter.WriteLine(DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString());
                    userWriter.WriteLine(fileAlreadyStored.FileID + " is a duplicate of file: " + duplicateFile.FileID);
                    userWriter.WriteLine();
                }
            }


        }
        public void WriteBinary(BookEntry bookEntry)
        {
            try
            {
                var fileName = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "BinaryFiles");
                var files = Directory.GetFiles(fileName).Length;
                fileName = fileName + "\\file00" + (files + 1) + ".dat";

                var readFile = fileName.Split('\\');
                var fileID = readFile.Last();

                using (BinaryWriter binWriter =
                    new BinaryWriter(File.Open(fileName, FileMode.Create)))
                {
                    bookEntry.FileID = fileID;
                    binWriter.Write(bookEntry.FileID);
                    binWriter.Write(bookEntry.AuthorName);
                    binWriter.Write(bookEntry.BookTitle);
                    binWriter.Write(bookEntry.QuantityInStock);
                    binWriter.Write(bookEntry.Price);
                }
                Console.WriteLine();
            }
            catch (IOException ioexp)
            {
                Console.WriteLine("Error: {0}", ioexp.Message);
            }
        }

        public void ReadBinary()
        {
            try
            {
                var folderPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "BinaryFiles");

                List<BookEntry> bookList = new List<BookEntry>();
                var files = Directory.GetFiles(folderPath).Length;
                var allFiles = Directory.GetFiles(folderPath);

                if (files > 0)
                {
                    foreach (var filePath in allFiles)
                    {
                        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                        {
                            BookEntry bookEntry = new BookEntry();
                            bookEntry.FileID = reader.ReadString();
                            bookEntry.AuthorName = reader.ReadString();
                            bookEntry.BookTitle = reader.ReadString();
                            bookEntry.QuantityInStock = reader.ReadInt32();
                            bookEntry.Price = reader.ReadDouble();

                            bookList.Add(bookEntry);

                            var readFile = filePath.Split('\\');
                            var listFile = readFile.Last();

                            var findBoook = bookList.FirstOrDefault(file => file.BookTitle == bookEntry.BookTitle && file.FileID.Equals(bookEntry.FileID) == false);

                            if (findBoook != null)
                            {
                                LogWriter(findBoook, bookEntry);
                            }
                        }
                    }
                }
            }
            catch (IOException ioexp)
            {
                Console.WriteLine("Error: {0}", ioexp.Message);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string enterMoreBooks = "Y";
            do
            {
                BookEntry bookEntry = new BookEntry();
                Console.WriteLine("--------------------------------------------------------------------");
                Console.WriteLine("----------------------BINARY FILES: BOOKSTORE-----------------------");
                Console.WriteLine("--------------------------------------------------------------------\n\n");
                Console.WriteLine("Please enter data to the file...\n\n");

                //Author
                Console.WriteLine("Please enter author name: ");
                bookEntry.AuthorName = Console.ReadLine();

                //Book title
                Console.WriteLine("Please book title: ");
                bookEntry.BookTitle = Console.ReadLine();

                //Quantity
                Console.WriteLine("Please enter the number of books in recieved: ");
                string quantity = Console.ReadLine();
                Int32 value;
                while (!Int32.TryParse(quantity, out value))
                {
                    Console.WriteLine("Please type in a valid number!! ");
                    quantity = Console.ReadLine();
                }
                bookEntry.QuantityInStock = Convert.ToInt32(quantity);


                //Price
                Console.WriteLine("Please enter price: ");
                Double priceCheck;
                string bookPriceRead = Console.ReadLine();
                while (!Double.TryParse(bookPriceRead, out priceCheck))
                {
                    Console.WriteLine("Please type in a valid price!! ");
                    bookPriceRead = Console.ReadLine();
                }
                bookEntry.Price = Convert.ToDouble(bookPriceRead);

                BinaryReadWriteClass readWrite = new BinaryReadWriteClass();
                readWrite.WriteBinary(bookEntry);
                readWrite.ReadBinary();


                Console.WriteLine("--------------------------------------------------------------------");
                Console.WriteLine();
                Console.WriteLine("Do you want to enter another book?(Y/N)");
                enterMoreBooks = Console.ReadLine();
                while (enterMoreBooks.Equals("Y", StringComparison.InvariantCultureIgnoreCase) == false && enterMoreBooks.Equals("N", StringComparison.InvariantCultureIgnoreCase) == false)
                {
                    Console.WriteLine("Please type in Y or N!! ");
                    enterMoreBooks = Console.ReadLine();
                }
            } while (enterMoreBooks.Equals("N", StringComparison.InvariantCultureIgnoreCase) == false);

            Console.WriteLine("\n\n..........................................................................");
            Console.WriteLine("................GOODBYE. THANKS FOR USING BOOKSTORE APP.................... ");
            Console.WriteLine("............................................................................\n\n");
            Console.WriteLine("Press any key to close!!");
            Console.ReadKey();

        }
    }
}
