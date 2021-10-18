using Amazon;
using System;
using System.Collections.Generic;
using System.Text;

namespace eBookReader.Models
{
    class Client
    {
        public string AccessKeyID { get; set; }
        public string SecretKey { get; set; }
        public RegionEndpoint Region { get; set; }

        public Client()
        {
            AccessKeyID = "AKIA5EUHVKP3AGHIXCAC";
            SecretKey = "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";
            Region = RegionEndpoint.GetBySystemName("us-east-1");
        }
    }
}
