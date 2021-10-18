using Amazon.S3;
using Amazon.S3.Model;
using eBookReader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace eBookReader.Views
{
    /// <summary>
    /// Interaction logic for PDFView.xaml
    /// </summary>
    public partial class PDFView : Window
    {
        public PDFView(Book bookToView)
        {
            InitializeComponent();
            GetPDF(bookToView.BookName);
        }

        private async void GetPDF(string bookName)
        {
            Client newClient = new Client();
            AmazonS3Client s3Client = new AmazonS3Client(newClient.AccessKeyID, newClient.SecretKey, newClient.Region);
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = "farzam-books-bucket",
                Key = bookName + ".pdf"
            };

            GetObjectResponse response = await s3Client.GetObjectAsync(request);
            MemoryStream _documentStream = new MemoryStream();
            response.ResponseStream.CopyTo(_documentStream);
            pdfViewer.Load(_documentStream);
        }
    }
}
