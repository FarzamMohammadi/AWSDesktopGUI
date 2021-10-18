using System;
using System.Collections.Generic;
using System.Text;

namespace eBookReader.Models
{
    class Book
    {
        string BookName { get; set; }
        string BookAuthor { get; set; }

        public Book(string name, string author)
        {
            BookName = name;
            BookAuthor = author;
        }
    }
}
