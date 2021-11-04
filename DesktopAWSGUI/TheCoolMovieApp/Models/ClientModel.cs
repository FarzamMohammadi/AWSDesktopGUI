using Amazon;
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
        public string AccessKeyID { get; set; }
        public string SecretKey { get; set; }
        private readonly RegionEndpoint Region;

        public string RDSConnStr { get; set; }
        public ClientModel()
        {
            AccessKeyID = "AKIAW6YHDHANOHPKQLD2";
            SecretKey = "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";
            Region = RegionEndpoint.GetBySystemName("ca-central-1");
            //Sets DB connections string using parameter store value
            RDSConnStr = GetValueAsync("/Client/RDSConnStr").Result;
        }

        public async Task<string> GetValueAsync(string parameter)
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
