using Amazon;
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

        public ClientModel()
        {
            AccessKeyID = "AKIA5EUHVKP3AGHIXCAC";
            SecretKey = "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";
            Region = RegionEndpoint.GetBySystemName("ca-central-1");
        }
    }
}
