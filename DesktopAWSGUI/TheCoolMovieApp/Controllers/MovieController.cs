using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TheCoolMovieApp.Models;

namespace TheCoolMovieApp.Controllers
{
    public class MovieController : Controller
    {
        public IActionResult ViewAllMovies()
        {
            GetAllMovieToShow();
            return View();
        }

        private void GetAllMovieToShow()
        {   //Creates table if it doesnt exist to prevent exception
            CreateDBTable();
            SqlConnection conn = new SqlConnection();
            string connString = ClientModel.RDSConnStr;
            conn.ConnectionString = connString;
            conn.Open();
            string newUserQuery = "SELECT * FROM Movies;";
            SqlCommand myCommand = new SqlCommand(newUserQuery, conn);

            var reader = myCommand.ExecuteReader();
            List<MovieModel> moviesToShow = new List<MovieModel>();
            while (reader.Read())
            {
                MovieModel movieRecord = new MovieModel();
                movieRecord.Title = reader.GetString(1);
                movieRecord.Year = reader.GetString(2);
                movieRecord.Origin = reader.GetString(3);
                movieRecord.Length = reader.GetString(4);
                movieRecord.Creator = reader.GetString(5);
                moviesToShow.Add(movieRecord);
            };
            MovieModel.MoviesToShow = moviesToShow;
        }

        public IActionResult AddMovie()
        {
            return View();
        }

        public IActionResult EditMovie(MovieModel newMovie)
        {
            if (UserModel.Username == newMovie.Creator)
            {
                DeleteMovieRecord(MovieModel.movieIDToDelete);
                AddNewMovieToDB(newMovie);
            }
            else
            {
                //If upload is Unsuccessful
                MyStringModel noAccess = new MyStringModel();
                noAccess.Message = "You are not the creator of this movie and thus do not have access to edit this movie";
                return View("Error", noAccess);
            }
            GetAllMovieToShow();
            return View("ViewAllMovies");
        }

        public ActionResult DeleteMovie(MovieModel movie)
        {
            if (DeleteMovieS3(movie.Title))
            {
                DeleteMovieRecord(movie.Title);
            }
            else
            {
                //Delete Unsuccessful
                MyStringModel deleteUnsuccessful = new MyStringModel();
                deleteUnsuccessful.Message = "Delete Unsuccessful";
                return View("Error", deleteUnsuccessful);
            }

            GetAllMovieToShow();
            return View("ViewAllMovies");
        }
        private void DeleteMovieRecord(string movieIDToDelete)
        {
            //Delete record from table but once again makes sure the user and title match
            SqlConnection conn = new SqlConnection();
            string connString = ClientModel.RDSConnStr;
            conn.ConnectionString = connString;
            conn.Open();
            string newUserQuery = "DELETE FROM Movies WHERE Title = '" + movieIDToDelete + "' AND Creator = '" + UserModel.Username + "';";
            SqlCommand myCommand = new SqlCommand(newUserQuery, conn);
            var reader = myCommand.ExecuteReader();
        }

        private bool DeleteMovieS3(string movieIDToDelete)
        {
            try
            {
                ClientModel.S3Client.DeleteObjectAsync(new DeleteObjectRequest() { BucketName = ClientModel.BucketName, Key = movieIDToDelete });
                return true;
            }
            catch
            {
                return false;
            }
            

        }
        public ActionResult RedirectToAddNewMovie()
        {
            return View("AddMovie");
        }

        public ActionResult RedirectToEditMovie(MovieModel movie)
        {
            //Sets title in to delete in database if user makes changes
            if (UserModel.Username == movie.Creator)
            {
                SqlConnection conn = new SqlConnection();
                string connString = ClientModel.RDSConnStr;
                conn.ConnectionString = connString;
                conn.Open();
                string newUserQuery = " SELECT * FROM Movies WHERE Title = '" + movie.Title + "' AND Creator = '" + movie.Creator +"';";
                SqlCommand myCommand = new SqlCommand(newUserQuery, conn);


                var reader = myCommand.ExecuteReader();
                List<MovieModel> moviesToShow = new List<MovieModel>();
                while (reader.Read())
                {
                    MovieModel.movieIDToDelete = reader.GetString(1);
                };
                return View("EditMovie", movie);
            }
            else
            {
                //If upload is Unsuccessful
                MyStringModel noAccess = new MyStringModel();
                noAccess.Message = "You are not the creator of this movie and thus do not have access to edit this movie";
                return View("Error", noAccess);
            }
        }
    

        //Limit of 2GB also added to web.config
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 2097152000)]
        public ActionResult AddNewMovie(MovieModel newMovie)
        {
            bool uploadSuccess = AddFileToBucket(newMovie.File, newMovie.Title).Result;
            if (uploadSuccess)
            {
                AddNewMovieToDB(newMovie);
            }
            else
            {
                //If upload is Unsuccessful
                MyStringModel uploadUnsuccessful = new MyStringModel();
                uploadUnsuccessful.Message = "Upload Unsuccessful";
                return View("Error", uploadUnsuccessful);
            }
            GetAllMovieToShow();
            return View("ViewAllMovies");
        }

        private void AddNewMovieToDB(MovieModel newMovie)
        {
            //Check to see if table exists, if not, create one
            CreateDBTable();
            //Initialize connection
            SqlConnection conn = new SqlConnection();
            string connString = ClientModel.RDSConnStr;
            conn.ConnectionString = connString;
            conn.Open();

            //Setup query
            string newUserQuery = "INSERT INTO Movies (Title, Year, Origin, Length, Creator)";
            newUserQuery += " VALUES (@Title, @Year, @Origin, @Length, @Creator)";
            SqlCommand myCommand = new SqlCommand(newUserQuery, conn);

            //Setup new movie record details
            myCommand.Parameters.AddWithValue("@Title", newMovie.Title);
            myCommand.Parameters.AddWithValue("@Year", newMovie.Year);
            myCommand.Parameters.AddWithValue("@Origin", newMovie.Origin);
            myCommand.Parameters.AddWithValue("@Length", newMovie.Length);
            myCommand.Parameters.AddWithValue("@Creator", UserModel.Username);
            myCommand.ExecuteNonQuery();
        }

        private async Task<bool> AddFileToBucket(IFormFile fileToUpload, string title)
        {
            IFormFile file = fileToUpload;
            AmazonS3Client client = ClientModel.S3Client;
            string bucketName = ClientModel.BucketName;
            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = title,
            };
            try
            {
                // Put object
                var filePath = Path.GetTempFileName();

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                    request.InputStream = stream;
                    PutObjectResponse response = await client.PutObjectAsync(request);
                }
                return true;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                return false;
            }
        }

        private void CreateDBTable()
        {
            SqlConnection conn = new SqlConnection();
            string connString = ClientModel.RDSConnStr;
            conn.ConnectionString = connString;
            conn.Open();

            bool exists;

            // check if table exists
            string tableExistQuery = "select case when exists((select * from information_schema.tables where table_name = '" + "Movies" + "')) then 1 else 0 end";
            SqlCommand myCommand = new SqlCommand(tableExistQuery, conn);
            exists = (int)myCommand.ExecuteScalar() == 1;

            //If table does not exist, it gets created
            if (!exists)
            {
                string createTable = "CREATE TABLE Movies (Id int IDENTITY(1,1) PRIMARY KEY, Title varchar(50),Year varchar(4),Origin varchar(30),Length varchar(4), Creator varchar(50));";
                myCommand = new SqlCommand(createTable, conn);
                myCommand.ExecuteNonQuery();
            }
        }
    }
}
