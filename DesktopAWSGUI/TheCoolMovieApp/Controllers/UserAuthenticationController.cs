using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using eBookReader.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
                //If all textboxes are filled then first creates new 'Users' table if it does not already exist
                bool tableCreated = CreateUserTable().Result;
                if (tableCreated)
                {
                    //If table creation was successful
                    //Then creates new record in the 'Users' table with user input data
                    bool newUserCreated = LoadUserData(userData).Result;
                    if (newUserCreated == false)
                    {
                        //If table creation was unsuccessful returns error message and error view
                        MyStringModel userNotCreated = new MyStringModel();
                        userNotCreated.Message = "User was not created";
                        return View("Error", userNotCreated);
                    }
                }
                else
                {
                    //If table creation was unsuccessful returns error message and error view
                    MyStringModel tableNotCreated = new MyStringModel();
                    tableNotCreated.Message = "database error. User not created";
                    return View("Error", tableNotCreated);
                }
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
                bool correctUserCredentials = CheckUserCredenitals(userData).Result;
               
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

        private async Task<bool> CheckUserCredenitals(UserModel userData)
        {
            // Goes through the Users database and checks if credentials match
            ClientModel newClient = new ClientModel();
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(newClient.AccessKeyID, newClient.SecretKey, newClient.Region);
            string tableName = "MovieUsers";
            var request = new ScanRequest
            {
                TableName = tableName
            };

            var response = await client.ScanAsync(request);
            var result = response.Items;

            string recordEmail = "";
            string recordPassword = "";
            //Checks each entry to make sure all user credentials match
            for (int i = 0; i < result.Count; i++)
            {
                var items = result[i];
                foreach (var item in items)
                {
                    var scanKey = item.Key;
                    var scanValue = item.Value;

                    if (scanKey == "Email")
                    {
                        recordEmail = scanValue.S.ToString();
                    }

                    if (scanKey == "Password")
                    {
                        recordPassword = scanValue.S.ToString();
                    }

                    if(recordEmail != "" && recordPassword != "")
                    {
                        if (userData.Email == recordEmail)
                        {
                            if (userData.Password == recordPassword)
                            {
                                // if both email and password macth record
                                return true;
                            }
                        }
                    }

                }
            }
            return false;
        }

        private async Task<bool> CreateUserTable()
        {
            string tableName = "MovieUsers";
            ClientModel newClient = new ClientModel();
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(newClient.AccessKeyID, newClient.SecretKey, newClient.Region);


            //Table Attributes and table creation
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        AttributeType = "N"
                    }
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType = "HASH"
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 1,
                    WriteCapacityUnits = 1
                }
            };
            

            //Shows message to of whether table already exits or if it has been createds
            try
            {
                var response = await client.CreateTableAsync(request);
                var res = await client.DescribeTableAsync(new DescribeTableRequest { TableName = "MovieUsers" });
                while (res.Table.TableStatus != "Active")
                {
                    // Wait for Table to be created before loading data
                    System.Threading.Thread.Sleep(1000);
                    res = await client.DescribeTableAsync(new DescribeTableRequest { TableName = "MovieUsers" });
                }
                if (res.Table.TableStatus == "Active")
                {
                    //if table exists already just load data in case it is not there
                    return true;
                }
            }
            catch
            {
                // If table already exists
                return true;
            }
            return false;
        }
        private async Task<bool> LoadUserData(UserModel userData)
        {
            ClientModel newClient = new ClientModel();
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(newClient.AccessKeyID, newClient.SecretKey, newClient.Region);
            Table table = Table.LoadTable(client, "MovieUsers");

            var request = new ScanRequest
            {
                TableName = "MovieUsers"
            };
            var response = await client.ScanAsync(request);
            var result = response.Items;

            //Loading table with initial defualt user credentials
            try
            {
                var newUser = new Document();
                newUser["Id"] = result.Count + 1;
                newUser["FirstName"] = userData.FirstName;
                newUser["LastName"] = userData.LastName;
                newUser["Email"] = userData.Email;
                newUser["Password"] = userData.Password;

                await table.PutItemAsync(newUser);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
