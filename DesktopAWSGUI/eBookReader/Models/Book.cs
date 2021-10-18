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
        public void SetReadDate()
        {

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
