using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace TheCoolMovieApp.Models
{
    public class MovieModel
    {
        [Required]
        public string Title { get; set; }
        public string Year { get; set; }
        public string Origin { get; set; }
        public string Length { get; set; }
        public string Creator { get; set; }
        [Required]
        public IFormFile File { get; set; }
        public static List<MovieModel> MoviesToShow { get; set; }
        public static string MovieIDToDelete {get;set;}
        public int Rating { get; set; }
    }
}
