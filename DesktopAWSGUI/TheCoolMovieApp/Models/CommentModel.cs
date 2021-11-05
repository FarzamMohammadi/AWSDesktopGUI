using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCoolMovieApp.Models
{
    public class CommentModel
    {
        public static List<Tuple<string, string>> CommentList = new List<Tuple<string, string>>();
        public string Comment { get; set; }
        public string User { get; set; }
    }
}
