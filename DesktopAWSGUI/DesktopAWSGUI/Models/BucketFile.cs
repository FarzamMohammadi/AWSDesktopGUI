using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAWSGUI.Models
{
    class BucketFile
    {
        public string FileName { get; set; }

        public string FileSize { get; set; }
        public BucketFile(string fileName, string fileSize)
        {
            FileName = fileName;
            FileSize = fileSize;
        }
    }
}
