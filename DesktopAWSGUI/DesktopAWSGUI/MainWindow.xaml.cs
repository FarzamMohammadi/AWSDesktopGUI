using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using DesktopAWSGUI.ViewModels;
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

namespace DesktopAWSGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.CACentral1;

        private readonly IAmazonS3 s3Client;

        public MainWindow()
        {
            InitializeComponent();
            s3Client = new AmazonS3Client(
                     "AKIA5EUHVKP3OKLQQPTA",
                     "qp9tc6V82Sq7LrAWxAijVHd0czY7SdumD+f6B0cl",
                        RegionEndpoint.CACentral1);
        }
        private void BucketOperations(object sender, RoutedEventArgs e)
        {
            DataContext = new OperateBucketViewModel();
        }
        private void CreateBucket(object sender, RoutedEventArgs e)
        {
            DataContext = new CreateBucketViewModel();
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
