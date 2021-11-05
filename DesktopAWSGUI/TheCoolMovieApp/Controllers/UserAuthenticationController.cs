using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCoolMovieApp.Models;

namespace TheCoolMovieApp.Controllers
{
    public class UserAuthenticationController : Controller
    {
        //Returns Login view
        public IActionResult Login()
        {
            return View();
        }

        //Returns Sign up View
        public IActionResult SignUp()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserModel userData)
        {
            //Check to see if all required registration textboxes have been filled
            if (ModelState.IsValid)
            {
                LoadUserData(userData);
            }
            else
            {
                //If user input had issues returns error message and error view
                MyStringModel EmptyFields = new MyStringModel();
                EmptyFields.Message = "somefields were left blank";
                return View("Error", EmptyFields);
            }
            return View("Login", userData);
        }

        [HttpPost]
        public ActionResult Login(UserModel userData)
        {

            //Check to see if all required registration textboxes have been filled
            if (ModelState.IsValid)
            {
                bool correctUserCredentials = CheckUserCredenitals(userData);
               
                if (correctUserCredentials)
                {
                    //If credentials match, sets user as logged in using the UserModel and returns user to landing page
                    UserModel.LoggedIn = true;
                    return View("Views/Home/Index.cshtml");
                }

            }
            //If user crenditals don't match or textboxes are empty, error message and error view is shown 
            MyStringModel EmptyFields = new MyStringModel();
            EmptyFields.Message = "your credentials did not match our records";
            return View("Error", EmptyFields);

        }

        private bool CheckUserCredenitals(UserModel userData)
        {

            SqlConnection conn = new SqlConnection();
            string connString = ClientModel.RDSConnStr;
            conn.ConnectionString = connString;
            conn.Open();
            string newUserQuery = " SELECT * FROM Users WHERE Email = '" + userData.Email + "' AND Password = '" + userData.Password +"';";
            SqlCommand myCommand = new SqlCommand(newUserQuery, conn);

            var reader = myCommand.ExecuteReader();
            bool userCredsMatch = reader.HasRows;

            if (userCredsMatch)
            {
                var user = new UserModel();
                while (reader.Read())
                {
                    user.FirstName = reader.GetString(1);
                    user.LastName = reader.GetString(2);
                    user.Email = reader.GetString(3); 
                    user.Password = reader.GetString(4);
                };
                //Sets navbar username for extra coolness
                UserModel.Username = user.Email;
                return true;
            }

            return false;
        }

   
        private void LoadUserData(UserModel userData)
        {
            //If table 'Users' does not exist it creates it
            CreateDBTable();
            //Initialize connection
            SqlConnection conn = new SqlConnection();
            string connString = ClientModel.RDSConnStr;
            conn.ConnectionString = connString;
            conn.Open();

            //Setup query
            string newUserQuery = "INSERT INTO Users (First_Name, Last_Name, Email, Password)";
            newUserQuery += " VALUES (@First_Name, @Last_Name, @Email, @Password)";
            SqlCommand myCommand = new SqlCommand(newUserQuery, conn);

            //Setup new user data
            myCommand.Parameters.AddWithValue("@First_Name", userData.FirstName);
            myCommand.Parameters.AddWithValue("@Last_Name", userData.LastName);
            myCommand.Parameters.AddWithValue("@Email", userData.Email);
            myCommand.Parameters.AddWithValue("@Password", userData.Password);

            myCommand.ExecuteNonQuery();
        }

        private void CreateDBTable()
        {
            SqlConnection conn = new SqlConnection();
            string connString = ClientModel.RDSConnStr;
            conn.ConnectionString = connString;
            conn.Open();
           
            bool exists;

            // check if table exists
            string tableExistQuery = "select case when exists((select * from information_schema.tables where table_name = '" + "Users" + "')) then 1 else 0 end";
            SqlCommand myCommand = new SqlCommand(tableExistQuery, conn);
            exists = (int)myCommand.ExecuteScalar() == 1;

            //If table does not exist, it gets created
            if (!exists)
            {
                string createTable = "CREATE TABLE Users (Id int IDENTITY(1,1) PRIMARY KEY, First_Name varchar(50),Last_Name varchar(50),Email varchar(50),Password varchar(50));";
                myCommand = new SqlCommand(createTable, conn);
                myCommand.ExecuteNonQuery();
            }
        }
    }
}
