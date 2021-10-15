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
            var builder = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

          /*  var accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            var secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;*/
            var region = RegionEndpoint.GetBySystemName("us-east-1");

            var accessKeyID = "AKIA5EUHVKP3AGHIXCAC";
            var secretKey = "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";

            AmazonDynamoDBClient client = new AmazonDynamoDBClient(accessKeyID, secretKey, region);
            string tableName = "ProductCatalog";

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
            var response = await client.CreateTableAsync(request);
            var res = await client.DescribeTableAsync(new DescribeTableRequest { TableName = "ProductCatalog" });
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
