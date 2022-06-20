using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace Launcher.Converters
{
    internal class AwsUploader
    {
        string AWSAccessKey = "AKIAUBGDL2QQ5P5EW7YT";
        string AWSSecretKey = "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";

        public void DownloadStatic()
        {
            TransferUtility fileTransferUtility = new TransferUtility(
            new AmazonS3Client(AWSAccessKey, AWSSecretKey, RegionEndpoint.EUNorth1));

            // Note the 'fileName' is the 'key' of the object in S3 (which is usually just the file name)
            fileTransferUtility.Download("Static.txt", "rustylauncher", "Static.txt");
        }


        /*
         AwsUploader uploader = new AwsUploader();
         uploader.UploadStatic();*/
        public void UploadStatic()
        {
            // input explained :
            // localFilePath = the full local file path e.g. "c:\mydir\mysubdir\myfilename.zip"
            // bucketName : the name of the bucket in S3 ,the bucket should be alreadt created
            // subDirectoryInBucket : if this string is not empty the file will be uploaded to
            // a subdirectory with this name
            // fileNameInS3 = the file name in the S3
            // create an instance of IAmazonS3 class ,in my case i choose RegionEndpoint.EUWest1
            // you can change that to APNortheast1 , APSoutheast1 , APSoutheast2 , CNNorth1
            // SAEast1 , USEast1 , USGovCloudWest1 , USWest1 , USWest2 . this choice will not
            // store your file in a different cloud storage but (i think) it differ in performance
            // depending on your location

            IAmazonS3 client = new AmazonS3Client(AWSAccessKey, AWSSecretKey, RegionEndpoint.EUNorth1);
            // create a TransferUtility instance passing it the IAmazonS3 created in the first step
            TransferUtility utility = new TransferUtility(client);
            // making a TransferUtilityUploadRequest instance
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

            request.BucketName = "rustylauncher"; //no subdirectory just bucket name

            request.Key = "Static.txt"; //file name up in S3
            request.FilePath = "Static.txt"; //local file name
            utility.Upload(request); //commensing the transfer
        }
    }
}