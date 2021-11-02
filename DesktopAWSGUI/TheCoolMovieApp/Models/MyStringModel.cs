using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCoolMovieApp.Models
{
    public class MyStringModel
    {
        //Created for sending messages between views
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}
