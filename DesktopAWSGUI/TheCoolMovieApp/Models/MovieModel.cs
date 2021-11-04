using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheCoolMovieApp.Models
{
    public class MovieModel
    {
        [Required]
        public string Title { get; set; }
        public string Year { get; set; }
        public string Origin { get; set; }
        public string Length { get; set; }
        public static string FilePath { get; set; }
    }
}
