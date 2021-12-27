# AWSDesktopGUI
 
# 1.	“DesktopAWSGUI” 
– C# WPF MVVM app that allows the user to create AWS S3 buckets and upload files to it from local device 
- Programmatical communication and access to S3 bucket to retrieve the list of items in selected bucket, with their name and size

# 2.	“eBookReader”
- C# WPF MVVM app that allows the registered user to view books from AWS S3 bucket and read them using Syncfusion PdfViewer NuGet
- User credentials, pdf bookmark & book order saved in DynamoDB

# 3.	“TheCoolMovieApp” 
– ASP.NET Core MVC app that allows registered users to upload movies to the AWS S3 bucket (Up to 2GB), modify, delete, download them, and save movie metadata, ratings, and comments, which other registered users can view and interact with
- User credentials, movie metadata, ratings, comments are saved in DynamoDB
- Secondary index used to facilitate movie search system
- User can modify their comment if creation date is < 24hrs
- Deployed to Elastic Beanstalk
