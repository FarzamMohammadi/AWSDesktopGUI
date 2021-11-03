using Amazon;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace eBookReader.Models
{
    class ClientModel
    {
        public string AccessKeyID { get; set; }
        public string SecretKey { get; set; }
        public RegionEndpoint Region { get; set; }

        public static string RDSConnStr = "Data Source=movie-app-db.ceerdmlyna5j.ca-central-1.rds.amazonaws.com,1433;" + "Initial Catalog=movie-app-db;" + "User id=farzam;" + "Password=123456789";
        public ClientModel()
        {
            AccessKeyID = "AKIA5EUHVKP3AGHIXCAC";
            SecretKey = "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";
            Region = RegionEndpoint.GetBySystemName("ca-central-1");
        }
    }
}
