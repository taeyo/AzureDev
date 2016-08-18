using System;
using System.Threading;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.DataMovement;
using System.Net;

namespace ASU_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //기본 연결이 2개이기에 늘려줌.
            ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount * 8;

            string storageConnectionString = "myStorageConnectionString";
            string sourcePath = @"C:\wmdownloads\Robotica_720.wmv";
            string containerName = "mycontainer";
            string destBlobName = "";
            int threadCount = 4;

            //일본 서부
            storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=delmeplz;AccountKey=***";
            //미국 서부
            //storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=vhdsc9dn6pnh7qeemxvub333;AccountKey=***";

            destBlobName = System.IO.Path.GetFileName(sourcePath);

            CloudStorageAccount account = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient blobClient = account.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerName);
            blobContainer.CreateIfNotExists();

            CloudBlockBlob destBlob = blobContainer.GetBlockBlobReference(destBlobName);

            // Setup the number of the concurrent operations
            TransferManager.Configurations.ParallelOperations = threadCount;
            // Setup the transfer context and track the upoload progress
            TransferContext context = new TransferContext();
            
            //동일한 파일이 있을 경우 덮어쓰기 설정
            context.OverwriteCallback = (source, destination) => true;

            context.ProgressHandler = new Progress<TransferProgress>((progress) =>
            {
                Console.WriteLine("Bytes uploaded: {0}", progress.BytesTransferred);
            });

            // Upload a local blob
            var task = TransferManager.UploadAsync(
                sourcePath, destBlob, null, context, CancellationToken.None);

        }
    }
}
