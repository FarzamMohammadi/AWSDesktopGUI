using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using DesktopAWSGUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopAWSGUI.Views
{
    /// <summary>
    /// Interaction logic for OperateBucketView.xaml
    /// </summary>
    public partial class OperateBucketView : System.Windows.Controls.UserControl
    {
        private static IAmazonS3 s3Client;
        private static string accessKey = "AKIA5EUHVKP3OKLQQPTA";
        private static string secretKey = "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";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.CACentral1;

        public OperateBucketView()
        {
            InitializeComponent();

            s3Client = new AmazonS3Client(
                       accessKey,
                       secretKey,
                       bucketRegion);
            LoadData();
        }

        private void LoadData()
        {
            ListBucketsResponse objListBucketsResponse = s3Client.ListBuckets();

            if (objListBucketsResponse.Buckets.Count > 0)
            {
                foreach (S3Bucket bucket in objListBucketsResponse.Buckets)
                {
                    bucketCBox.Items.Add(bucket.BucketName);

                }
            }
        }

        private void ChooseFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "C# Corner Open File Dialog";
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                fileNameTBox.Text = fdlg.FileName;
            }
        }

        private void UploadFileAsync(string bName, string fPath)
        {
            string bucketName = bName;
            string filePath = fPath;
            try
            {
                var fileTransferUtility =
                    new TransferUtility(s3Client);

                fileTransferUtility.Upload(filePath, bucketName);
                System.Windows.MessageBox.Show("Upload completed.");

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }

        }
        private void Upload(object sender, RoutedEventArgs e)
        {
            
            string bucketName = bucketCBox.SelectedItem.ToString();
            string filePath = fileNameTBox.Text;
            if (bucketName != "" && filePath != "")
            {
                UploadFileAsync(bucketName, filePath);
            }
            else
            {

                System.Windows.MessageBox.Show("Please provide the necessary information for upload.");
            }
        }

        private void bucketCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedBucket = bucketCBox.SelectedItem.ToString();
            if (selectedBucket != "")
            {
                this.fileInfo.Items.Clear();
                ListObjectsRequest listRequest = new ListObjectsRequest
                {
                    BucketName = selectedBucket,
                };

                ListObjectsResponse listResponse;
                do
                {
                    // Get a list of objects
                    listResponse = s3Client.ListObjects(listRequest);
                    foreach (S3Object obj in listResponse.S3Objects)
                    {
                        BucketFile file = new BucketFile(obj.Key.ToString(), obj.Size.ToString());
                        this.fileInfo.Items.Add(file);
                    }

                    // Set the marker property
                    listRequest.Marker = listResponse.NextMarker;
                } while (listResponse.IsTruncated);
            }

        }
    }
}
