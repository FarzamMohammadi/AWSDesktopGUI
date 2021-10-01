using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAWSGUI.Models
{
    class Bucket
    {
        public string BucketName { get; set; }

        public string BucketDate { get; set; }
        public Bucket(string bucketName, string bucketDate)
        {
            BucketName = bucketName;
            BucketDate = bucketDate;
        }
    }
}
