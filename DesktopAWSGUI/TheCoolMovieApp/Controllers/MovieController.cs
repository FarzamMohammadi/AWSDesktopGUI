using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Syroot.Windows.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        public ActionResult AddNewComment(MovieModel movie)
        {
            //Checks to see if comment already exists. Only returns results if not past 24hr deadline
            string existingComment = GetUserComment(movie).Result.Item1;
            if(existingComment == null)
            {
                //If it doesnt exit, leaves view comment section empty
                return View("AddComment", movie);
            }
            else
            {
                //If comment exists adds it to movie before passing the model to view
                movie.Comment = existingComment;
                return View("AddComment", movie);
            }
        }


        private async Task GetAllMovieComments(MovieModel movie)
        {
            List<Tuple<string, string>> commentList = new List<Tuple<string, string>>();
            AmazonDynamoDBClient client = ClientModel.dynamoDBclient;
            string tableName = "movie-comments";
            var request = new ScanRequest
            {
                TableName = tableName
            };

            var response = await client.ScanAsync(request);
            var result = response.Items;

            //Gets current rating and number of ratings and Id if movie mathces title and creator
            for (int i = 0; i < result.Count; i++)
            {
                string scanComment = "";
                string scanTitle = "";
                string scanUser = "";

                var items = result[i];
                foreach (var item in items)
                {
                    var scanKey = item.Key;
                    var scanValue = item.Value;

                    if (scanKey == "Title")
                    {
                        scanTitle = scanValue.S.ToString();
                    }
                    if (scanKey == "CommentCreator")
                    {
                        scanUser = scanValue.S.ToString();
                    }

                    if (scanKey == "Comment")
                    {
                        scanComment = scanValue.S.ToString();
                    }
                }
                if (scanTitle == movie.Title && scanComment != null && scanUser != null)
                {
                    //If any commetns exist for this TItle
                    Tuple<string, string> comment = new Tuple<string, string>(scanUser, scanComment);
                    commentList.Add(comment);
                }
            }
            MovieModel.CommentList = commentList;
        }

        private async Task<Tuple<string, int>> GetUserComment(MovieModel movie)
        {
            AmazonDynamoDBClient client = ClientModel.dynamoDBclient;
            string tableName = "movie-comments";
            var request = new ScanRequest
            {
                TableName = tableName
            };

            var response = await client.ScanAsync(request);
            var result = response.Items;

            //Gets current rating and number of ratings and Id if movie mathces title and creator
            for (int i = 0; i < result.Count; i++)
            {
                int idToReturn = 0;
                string scanComment = "";
                string scanCreationTime = "";
                string scanTitle = "";
                string scanCreator = "";

                var items = result[i];
                foreach (var item in items)
                {
                    var scanKey = item.Key;
                    var scanValue = item.Value;

                    if (scanKey == "Id")
                    {
                        idToReturn = int.Parse(scanValue.N);
                    }
                    if (scanKey == "Title")
                    {
                        scanTitle = scanValue.S.ToString();
                    }
                    if (scanKey == "CommentCreator")
                    {
                        scanCreator = scanValue.S.ToString();
                    }
                    if (scanKey == "Comment")
                    {
                        scanComment = scanValue.S.ToString();
                    }
                    if (scanKey == "CreationTime")
                    {
                        scanCreationTime = scanValue.S.ToString();
                    }
                }
                if (scanTitle == movie.Title && scanCreator == UserModel.Username && scanComment != null && scanCreationTime != null && idToReturn != 0)
                {
                    DateTime now = DateTime.Now;
                    DateTime createdOn = DateTime.Parse(scanCreationTime);
                   if (createdOn > now.AddHours(-24) && createdOn <= now)
                    {
                        return new Tuple<string, int>(scanComment, idToReturn);
                    }
                }
            }
            //If no comment exists or if comment creation date is past 24 hrs
            return new Tuple<string, int>("", 0);
        }

        public async Task<ActionResult> SubmitComment(MovieModel movie)
        {
            int existingCommentId = GetUserComment(movie).Result.Item2;

            AmazonDynamoDBClient client = ClientModel.dynamoDBclient;
            Table table = Table.LoadTable(client, "movie-comments");
            var newComment = new Document();
            //If this is a new comment
            if (existingCommentId != 0)
            {
                //If this comment is being upaded by user within the 24hr deadline
                //Ensures record matches comment details and then updates comment
                newComment["Id"] = existingCommentId;
                newComment["Title"] = movie.Title;
                newComment["CommentCreator"] = UserModel.Username;
                newComment["Comment"] = movie.Comment;
                newComment["CreationTime"] = DateTime.Now.ToString();
                await table.UpdateItemAsync(newComment);
                GetAllMovieComments(movie).Wait();
                return View("EditMovie", movie);
            }
            else
            {
                //If no comment exist then creates new record with appropriate Id            
                var request = new ScanRequest
                {
                    TableName = "movie-comments"
                };
                var response = client.ScanAsync(request);
                var result = response.Result;
                //Gets item count from dynamodb table to add new record with correct Id
                newComment["Id"] = result.Count + 1;
                newComment["Title"] = movie.Title;
                newComment["CommentCreator"] = UserModel.Username;
                newComment["Comment"] = movie.Comment;
                newComment["CreationTime"] = DateTime.Now.ToString();
                await table.PutItemAsync(newComment);
                GetAllMovieComments(movie).Wait();
                return View("EditMovie", movie);
            }
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
                //since the file has the same name as title
                moviesToShow.Add(movieRecord);
            };

            MovieModel.MoviesToShow = moviesToShow;
        }

        public ActionResult RateMovie(MovieModel movie)
        {
            Tuple<string, string, int> movieRating = GetCurrentRating(movie.Title, movie.Creator).Result;
            string recordRating = movieRating.Item1;
            string RecordnumberOfRatings = movieRating.Item2;
            int recordId = movieRating.Item3;

            if (recordRating != "" && RecordnumberOfRatings != "")
            {
                AddRatingToDB(movie, recordRating, RecordnumberOfRatings, recordId);
                return View("EditMovie", movie);
            }
            else
            {
                AddRatingToDB(movie, "", "", 0);
                return View("EditMovie", movie);
            }
        }

        private async void AddRatingToDB(MovieModel movie, string rating, string numberOfRatings, int recordId)
        {
            AmazonDynamoDBClient client = ClientModel.dynamoDBclient;
            Table table = Table.LoadTable(client, "movie-ratings");
            //If rating record of this movie exists already
            if (recordId != 0)
            {
                int newNumberOfRatings = (int.Parse(numberOfRatings) + 1);
                double newAvgRarting = (double.Parse(rating) * double.Parse(numberOfRatings) + movie.Rating) / newNumberOfRatings;

                //Ensures record matches movie details and then enters new rating
                var newRating = new Document();
                newRating["Id"] = recordId;
                newRating["Title"] = movie.Title;
                newRating["Creator"] = movie.Creator;
                newRating["NumberOfRatings"] = newNumberOfRatings;
                newRating["Rating"] = newAvgRarting.ToString();
                await table.UpdateItemAsync(newRating);
            }
            else
            {
                //If no ratings exist then creates new record with appropriate Id            
                var request = new ScanRequest
                {
                    TableName = "movie-ratings"
                };
                var response = client.ScanAsync(request);
                var result = response.Result;
                //Ensures record matches movie details and then enters new rating
                var newRating = new Document();
                newRating["Id"] = result.Count +1;
                newRating["Title"] = movie.Title;
                newRating["Creator"] = movie.Creator;
                newRating["NumberOfRatings"] = 1;
                newRating["Rating"] = movie.Rating.ToString();
                await table.PutItemAsync(newRating);
            }
         
        }

        private async Task<Tuple<string, string, int>> GetCurrentRating(string title, string creator)
        {
            AmazonDynamoDBClient client = ClientModel.dynamoDBclient;
            string tableName = "movie-ratings";
            var request = new ScanRequest
            {
                TableName = tableName
            };

            var response = await client.ScanAsync(request);
            var result = response.Items;

            //Gets current rating and number of ratings and Id if movie mathces title and creator
            for (int i = 0; i < result.Count; i++)
            {
                int idToReturn = 0;
                string ratingToReturn = "";
                string numberOfRatingsToReturn = "";
                string scanTitle = "";
                string scanCreator = "";

                var items = result[i];
                foreach (var item in items)
                {
                    var scanKey = item.Key;
                    var scanValue = item.Value;

                    if (scanKey == "Id")
                    {
                        idToReturn = int.Parse(scanValue.N);
                    }
                    if (scanKey == "Title")
                    {
                        scanTitle = scanValue.S.ToString();
                    }

                    if (scanKey == "Creator")
                    {
                        scanCreator = scanValue.S.ToString();
                    }
                    if (scanKey == "NumberOfRatings")
                    {
                        numberOfRatingsToReturn = scanValue.N.ToString();
                    }
                    if (scanKey == "Rating")
                    {
                        ratingToReturn = scanValue.S.ToString();
                    }
                }
                if (scanTitle == title && scanCreator == UserModel.Username && ratingToReturn != null && numberOfRatingsToReturn != null && idToReturn != 0)
                {
                    return Tuple.Create(ratingToReturn, numberOfRatingsToReturn, idToReturn);
                }
            }
            //If no ratings exist, returns null values
            return Tuple.Create("", "", 0);
        }

        public IActionResult AddMovie()
        {
            return View();
        }
        public IActionResult AddComment()
        {
            return View();
        }

        public IActionResult EditMovie(MovieModel newMovie)
        {
            if (UserModel.Username == newMovie.Creator)
            {
                DeleteMovieRecord(MovieModel.MovieIDToDelete);
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
        public async void DownloadMovie(MovieModel movie)
        {
            GetObjectResponse response = await ClientModel.S3Client.GetObjectAsync(ClientModel.BucketName, movie.Title);
            string type = MimeTypes.MimeTypeMap.GetExtension(response.Headers.ContentType.ToString()).ToString();

            //Uses 'MediaTypeMap' nuget to convert content type to file extension. This saves the file as proper format that was uploaded in
            string downloadPath = KnownFolders.Downloads.DefaultPath + "\\"+movie.Title + MimeTypes.MimeTypeMap.GetExtension(response.Headers.ContentType.ToString()).ToString();
            TransferUtility fileTransferUtility =
            new TransferUtility(ClientModel.S3Client);

            fileTransferUtility.Download(downloadPath, ClientModel.BucketName, movie.Title);
        }
        public ActionResult DeleteMovie(MovieModel movie)
        {
            if (movie.Creator == UserModel.Username)
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
            else
            {
                //If upload is Unsuccessful
                MyStringModel noAccess = new MyStringModel();
                noAccess.Message = "You are not the creator of this movie and thus do not have access to edit this movie";
                return View("Error", noAccess);
            }
           

           
        }
        private void DeleteMovieRecord(string MovieIDToDelete)
        {
            //Delete record from table but once again makes sure the user and title match
            SqlConnection conn = new SqlConnection();
            string connString = ClientModel.RDSConnStr;
            conn.ConnectionString = connString;
            conn.Open();
            string newUserQuery = "DELETE FROM Movies WHERE Title = '" + MovieIDToDelete + "' AND Creator = '" + UserModel.Username + "';";
            SqlCommand myCommand = new SqlCommand(newUserQuery, conn);
            myCommand.ExecuteReader();
        }

        private bool DeleteMovieS3(string MovieIDToDelete)
        {
            try
            {
                ClientModel.S3Client.DeleteObjectAsync(new DeleteObjectRequest() { BucketName = ClientModel.BucketName, Key = MovieIDToDelete });
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
            GetAllMovieComments(movie).Wait();
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
                    MovieModel.MovieIDToDelete = reader.GetString(1);
                };
                return View("EditMovie", movie);
            }
            else
            {
                return View("EditMovie", movie);
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
                ContentType = fileToUpload.ContentType
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
