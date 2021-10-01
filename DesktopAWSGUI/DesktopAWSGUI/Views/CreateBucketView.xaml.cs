using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using DesktopAWSGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DesktopAWSGUI.Views
{
    /// <summary>
    /// Interaction logic for CreateBucketView.xaml
    /// </summary>
    public partial class CreateBucketView : UserControl
    {
        private static IAmazonS3 s3Client;
        private static string accessKey = "AKIA5EUHVKP3OKLQQPTA";
        private static string secretKey = "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";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.CACentral1;



        public CreateBucketView()
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
            this.bucketInfo.Items.Clear();
            ListBucketsResponse objListBucketsResponse = s3Client.ListBuckets();

            if (objListBucketsResponse.Buckets.Count > 0) 
            {
                List<Bucket> bucketList = new List<Bucket>();
                foreach (S3Bucket bucket in objListBucketsResponse.Buckets)
                {
                   Bucket bucketToAdd = new Bucket(bucket.BucketName, bucket.CreationDate.ToString());
                    this.bucketInfo.Items.Add(bucketToAdd);

                }
            }
        }

        private void CreateBucket(object sender, RoutedEventArgs e)
        {
            string userInput = bNameTBox.Text;
            CreateBucketAsync(userInput);
        }
        private void CreateBucketAsync(string userInput)
        {
            string bucketName = userInput;
            try
            {
                
                if (!AmazonS3Util.DoesS3BucketExistV2(s3Client, bucketName))
                {
                    PutBucketRequest request = new PutBucketRequest();
                    request.BucketName = bucketName;
                    s3Client.PutBucket(request);
                    MessageBox.Show("Bucket Added.");
                    LoadData();
                    bNameTBox.Text = "";
                }
                else
                {
                    MessageBox.Show("This name already exists. Please try again.");
                }
               
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

         private void BackToMain(object sender, RoutedEventArgs e)
         {

         }
        }
}
