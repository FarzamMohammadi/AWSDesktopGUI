using Amazon.DynamoDBv2;
using Amazon.S3;
using Amazon.S3.Model;
using eBookReader.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace eBookReader.Views
{
    /// <summary>
    /// Interaction logic for PDFView.xaml
    /// </summary>
    public partial class PDFView : Window
    {
        string Bookname { get; set; }
        string BookPage { get; set; }
        public PDFView(Book bookToView)
        {
            Bookname = bookToView.BookName;
            InitializeComponent();
            GetPDF(bookToView.BookName);
        }

        private async void GetPDF(string bookName)
        {
            //Retrieves and loads the book based on user input from the datagrid 
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
            pdfViewer.IsBookmarkEnabled = true;
            pdfViewer.Load(_documentStream);
            await GetBookmarkedPage();
            pdfViewer.GoToPageAtIndex(Int32.Parse(BookPage));


        }

        private async Task GetBookmarkedPage()
        {
            string uniqueID = "";
            string bookPage = "";

            Client newClient = new Client();
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(newClient.AccessKeyID, newClient.SecretKey, newClient.Region);
            string tableName = "Bookmark";
            var request = new ScanRequest
            {
                TableName = tableName
            };

            var response = await client.ScanAsync(request);
            var result = response.Items;

            //Checks all bookmarks to see if bookmark for this user and this book exists 
            for (int i = 0; i < result.Count; i++)
            {
                var items = result[i];
                foreach (var item in items)
                {
                    var scanKey = item.Key;
                    var scanValue = item.Value;

                    if (scanKey == "Id")
                    {
                        uniqueID = scanValue.S.ToString();
                    }

                    if (scanKey == "BookPage")
                    {
                        bookPage = scanValue.S.ToString();
                    }
                }
                if (uniqueID == User.Username + Bookname)
                {
                    BookPage = bookPage;
                    break;
                }
            }
            if (BookPage == null)
            {
                BookPage = "1";
            }
        }

        private void Bookmark_OnClick(object sender, RoutedEventArgs e)
        {
            //Calls BookmarkPage to handle event
            string bookmarkPage = pdfViewer.CurrentPageIndex.ToString();
            BookmarkPage(bookmarkPage);
        }

        void Window_OnClose(object sender, CancelEventArgs e)
        {
            //Calls BookmarkPage to handle event
            string bookmarkPage = pdfViewer.CurrentPageIndex.ToString();
            BookmarkPage(bookmarkPage);
        }
        private async void BookmarkPage(string pageNumber)
        {
            Client newClient = new Client();
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(newClient.AccessKeyID, newClient.SecretKey, newClient.Region);
            Amazon.DynamoDBv2.DocumentModel.Table table = Amazon.DynamoDBv2.DocumentModel.Table.LoadTable(client, "Bookmark");

            //Enters unique Id into "Bookmark" table with page number either the user bookmarks or closes the window on
            var user1 = new Document();
            user1["Id"] = User.Username+Bookname;
            user1["BookPage"] = pageNumber;
            await table.PutItemAsync(user1);

        }
    }
}
