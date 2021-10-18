using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eBookReader.Models
{
    public class User
    {
        public static string Username { get; set; }
        public string Password { get; set; }
        public bool LoggedIn { get; set; }
        public static List<String> Books { get; set; }

        public static string GetUsername()
        {
            return Username;
        }
        public static List<String> GetBooks()
        {
            return Books;
        }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public async Task<string> LoginAsync()
        {
            // Goes through the "Users" database and Sets user as logged in if the email and password match the database credentials
            string scanUser = "";
            string scanPass = "";
            List<string> scanBooks = null;

            Client newClient = new Client();
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(newClient.AccessKeyID, newClient.SecretKey, newClient.Region);
            string tableName = "Users";
            var request = new ScanRequest
            {
                TableName = tableName
            };

            var response = await client.ScanAsync(request);
            var result = response.Items;

          
            for (int i = 0; i < result.Count; i++)
            {
                var items = result[i];
                foreach (var item in items)
                {
                    var scanKey = item.Key;
                    var scanValue = item.Value;

                    if (scanKey == "Username")
                    {
                        scanUser = scanValue.S.ToString();
                    }

                    if (scanKey == "Password")
                    {
                        scanPass = scanValue.S.ToString();
                    }

                    if (scanKey == "BooksPurchased")
                    {
                        scanBooks = scanValue.SS;
                    }
                }
                if (scanBooks != null)
                {
                    CheckNewItem(scanUser, scanPass, scanBooks);
                    if (Books != null)
                    {
                        return "Loggin Successful";
                    }
                }
            }
            return "Loggin Unsuccessful";

        }

        private void CheckNewItem(string scanUser, string scanPass, List<String> scanBooks)
        {
            //If username and password entered by user match sets the user books and user status as logged in 
            if (Username == scanUser && Password == scanPass)
            {
                Books = scanBooks;
                LoggedIn = true;
            }
        }
        
    }
}
