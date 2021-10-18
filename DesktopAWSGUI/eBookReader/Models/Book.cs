using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eBookReader.Models
{
    public class Book
    {
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public Book(string name)
        {
            BookName = name;
        }
        public async void SetReadDate()
        {
            Client newClientInfo = new Client();
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(newClientInfo.AccessKeyID, newClientInfo.SecretKey, newClientInfo.Region);
            Table table = Table.LoadTable(client, "Users");
            switch (User.Username)
            {
                case "farzam1@hotmail.com":
                    var user1 = new Document();
                    user1["Id"] = 1;
                    user1["Username"] = "farzam1@hotmail.com";
                    user1["Password"] = "124689";
                    user1["BooksPurchased"] = new List<string> { "Lab 1", "Lab 2" };
                    user1["LastBookRead"] = BookName;
                    await table.PutItemAsync(user1);
                    break;
                case "farzam2@hotmail.com":
                    var user2 = new Document();
                    user2["Id"] = 2;
                    user2["Username"] = "farzam2@hotmail.com";
                    user2["Password"] = "124689";
                    user2["BooksPurchased"] = new List<string> { "Lab 1", "Lab 3" };
                    user2["LastBookRead"] = BookName;
                    await table.PutItemAsync(user2);
                    break;
                case "farzam3@hotmail.com":
                    var user3 = new Document();
                    user3["Id"] = 3;
                    user3["Username"] = "farzam3@hotmail.com";
                    user3["Password"] = "124689";
                    user3["BooksPurchased"] = new List<string> { "Lab 1", "Lab 2", "Lab 3" };
                    user3["LastBookRead"] = BookName;
                    await table.PutItemAsync(user3);
                    break;
            }
        }
        public async Task<string> GetAuthor()
        {
            string scanBookName = "";
            string scanBookAuthor = "";
            Client newClient = new Client();
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(newClient.AccessKeyID, newClient.SecretKey, newClient.Region);
            string tableName = "BookShelf";

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

                    if (scanKey == "BookName")
                    {
                        scanBookName = scanValue.S.ToString();
                    }

                    if (scanKey == "BookAuthor")
                    {
                        scanBookAuthor = scanValue.S.ToString();
                    }
                }
                if (scanBookAuthor != null && scanBookName != null)
                {
                    bool userPurchasedBook = CheckIfUserHasBook(scanBookName);
                    if (userPurchasedBook == true)
                    {
                        BookAuthor = scanBookAuthor;
                    }
                }
            }
            return scanBookAuthor;


        }

        private bool CheckIfUserHasBook(string scanBookName)
        {
            foreach (string book in User.Books)
            {
                if (BookName == scanBookName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
