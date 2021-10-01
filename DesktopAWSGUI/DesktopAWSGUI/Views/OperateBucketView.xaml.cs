using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
    public partial class OperateBucketView : UserControl
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
