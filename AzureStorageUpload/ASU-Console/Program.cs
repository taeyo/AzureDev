using System;
using System.Threading;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.DataMovement;
using System.Net;
using System.Diagnostics;

namespace ASU_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount * 8;
            string storageConnectionString = "myStorageConnectionString";
            string sourcePath = @"C:\wmdownloads\Robotica_720.wmv";
            string containerName = "mycontainer";
            int threadCount = 16;

            //일본 서부
            storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=delmeplz;AccountKey=***";
            //미국 서부
            //storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=vhdsc9dn6pnh7qeemxvub333;AccountKey=***";


            CloudStorageAccount account = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerName);
            blobContainer.CreateIfNotExists();

            CloudBlockBlob destBlob = blobContainer.GetBlockBlobReference("myblob");

            // Setup the number of the concurrent operations
            TransferManager.Configurations.ParallelOperations = threadCount;
            // Setup the transfer context and track the upoload progress
            TransferContext context = new TransferContext();
            context.OverwriteCallback = (source, destination) => true;

            context.ProgressHandler = new Progress<TransferProgress>((progress) =>
            {
                Console.WriteLine("Bytes uploaded: {0}", progress.BytesTransferred);
            });
            //Stopwatch watcher = new Stopwatch();
            //watcher.Start();


            // Upload a local blob
            var task = TransferManager.UploadAsync(
                sourcePath, destBlob, null, context, CancellationToken.None);
            //task.Wait();

            //watcher.Stop();
            //// Get the elapsed time as a TimeSpan value.
            //TimeSpan ts = watcher.Elapsed;

            //// Format and display the TimeSpan value. 
            //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //    ts.Hours, ts.Minutes, ts.Seconds,
            //    ts.Milliseconds / 10);

            //Console.WriteLine("소요시간 : {0}", elapsedTime);
            //Console.ReadLine();
        }
    }
}
