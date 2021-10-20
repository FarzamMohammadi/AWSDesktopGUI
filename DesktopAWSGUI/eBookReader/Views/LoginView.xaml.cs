using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Amazon.DynamoDBv2.DocumentModel;
using eBookReader.Models;
using eBookReader.ViewModels;
using eBookReader.Commands;
using System.Threading.Tasks;

namespace eBookReader.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            LoadCredentials();
        }

        private static async void LoadCredentials()
        {
            // AWS Client login
            string tableName = "Users";
            Client newClient = new Client();
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
                var res = await client.DescribeTableAsync(new DescribeTableRequest { TableName = "Users" });
                while (res.Table.TableStatus != "Active")
                {
                    // Wait for Table to be created before loading data
                    System.Threading.Thread.Sleep(1000);
                    res = await client.DescribeTableAsync(new DescribeTableRequest { TableName = "Users" });
                }
                if (res.Table.TableStatus == "Active")
                {
                    //if table exists already just load data in case it is not there
                    LoadTableData(client);
                }
            }              
            catch
            {
                LoadTableData(client);
            }
        }

        private static async void LoadTableData(AmazonDynamoDBClient client)
        {
            Amazon.DynamoDBv2.DocumentModel.Table table = Amazon.DynamoDBv2.DocumentModel.Table.LoadTable(client, "Users");
            
            var user1 = new Document();
            user1["Id"] = 1;
            user1["Username"] = "farzam1@hotmail.com";
            user1["Password"] = "124689";
            user1["BooksPurchased"] = new List<string> { "Lab 1", "Lab 2" };

            var user2 = new Document();
            user2["Id"] = 2;
            user2["Username"] = "farzam2@hotmail.com";
            user2["Password"] = "124689";
            user2["BooksPurchased"] = new List<string> { "Lab 1", "Lab 3" };

            var user3 = new Document();
            user3["Id"] = 3;
            user3["Username"] = "farzam3@hotmail.com";
            user3["Password"] = "124689";
            user3["BooksPurchased"] = new List<string> { "Lab 1", "Lab 2", "Lab 3" };

            await table.PutItemAsync(user1);
            await table.PutItemAsync(user2);
            await table.PutItemAsync(user3);
            MessageBox.Show("Table 'Users' Created and Credentials Loaded.");
        }

        private async void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = emailTBox.Text;
            string password = passwordTBox.Password;
            User user = new User(username, password);
           
            var task = await user.LoginAsync();
            bool login = user.LoggedIn;

            if (login == true)
            {
                MessageBox.Show("Login Succesful.");
                Mediator.Notify("GoToProfileView", "");
            }
            else
            {
                MessageBox.Show("Login Unsuccesful. Please Try Again.");
                emailTBox.Text = "";
                passwordTBox.Password = "";
            }
        }
    }
}
