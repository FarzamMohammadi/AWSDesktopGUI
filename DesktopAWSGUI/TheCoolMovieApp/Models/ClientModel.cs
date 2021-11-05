using Amazon;
using Amazon.DynamoDBv2;
using Amazon.S3;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TheCoolMovieApp.Models
{
    class ClientModel
    {
        private static string AccessKeyID = "AKIAW6YHDHANOHPKQLD2";
        private static string SecretKey = "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";
        public static RegionEndpoint Region = RegionEndpoint.GetBySystemName("ca-central-1");
        public static AmazonS3Client S3Client = new AmazonS3Client(AccessKeyID, SecretKey, Region);
        public static AmazonDynamoDBClient dynamoDBclient = new AmazonDynamoDBClient(AccessKeyID, SecretKey, Region);
        public static string BucketName = "movie-bucket-farzam";
        public static string RDSConnStr = GetValueAsync("/Client/RDSConnStr").Result;

        public static async Task<string> GetValueAsync(string parameter)
        {
            //Gets Parameter store value of DB connection string 
            var ssmClient = new AmazonSimpleSystemsManagementClient(AccessKeyID, SecretKey, Region);

            var response = await ssmClient.GetParameterAsync(new GetParameterRequest
            {
                Name = parameter,
                WithDecryption = true
            });
            return response.Parameter.Value;
        }
    }
}
