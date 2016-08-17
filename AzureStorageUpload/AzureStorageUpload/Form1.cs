using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.DataMovement;
using System.Net;
using System.Diagnostics;

namespace AzureStorageUpload
{
    public partial class Form1 : Form
    {
        CloudStorageAccount account;
        CloudBlobClient blobClient;
        CloudBlobContainer blobContainer;

        string storageConnectionString = string.Empty;
        string sourcePath = @"C:\wmdownloads\Robotica_720.wmv";
        string containerName = "mycontainer";
        int threadCount = 2;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount * 8;

        }

        private void Connect_Click(object sender, EventArgs e)
        {
            //일본 서부
            storageConnectionString = String.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", AccountName.Text, AccountKey.Text);
            //미국 서부
            //storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=vhdsc9dn6pnh7qeemxvub333;AccountKey=ZGLnkaIicSr/bwf4loDxdNC0f4uxlBXzKAmECku9FAIAZscA4UR+aBaRg7jYikp6R0T9osP0vi3Cf++Hdd6ZUA==";

            try
            {
                account = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch(FormatException ex)
            {
                MessageBox.Show("유효한 계정 정보가 아닙니다", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            blobClient = account.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(containerName);
            blobContainer.CreateIfNotExists();

            // 컨트롤들의 상태 변경
            uploadButton.Enabled = true;
            ConnectState.ForeColor = Color.Blue;
            ConnectState.Text = "* 연결되었습니다";
        }
        private void uploadButton_Click(object sender, EventArgs e)
        {
            Stopwatch watcher = new Stopwatch();
            watcher.Start();

            // Create the destination CloudBlob instance
            CloudBlockBlob destBlob = blobContainer.GetBlockBlobReference("myblob");

            // Setup the number of the concurrent operations
            TransferManager.Configurations.ParallelOperations = threadCount;
            // Setup the transfer context and track the upoload progress
            TransferContext context = new TransferContext();
            context.OverwriteCallback = (source, destination) => true;

            context.ProgressHandler = new Progress<TransferProgress>((progress) =>
            {
                Log.Text = "Bytes uploaded: " + progress.BytesTransferred.ToString("N0");
                Console.WriteLine("Bytes uploaded: {0}", progress.BytesTransferred);
            });
            context.FileTransferred += new EventHandler<TransferEventArgs>((o, args) =>
            {
                watcher.Stop();
                Console.WriteLine("FileTransferred");

                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = watcher.Elapsed;

                // Format and display the TimeSpan value. 
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);

                timespent.Text = "소요시간 : " + elapsedTime.ToString();
            });

            // Upload a local blob
            //var task = TransferManager.UploadAsync(
            //    sourcePath, destBlob, null, context, CancellationToken.None);
            //task.Wait();
            UploadAsync(sourcePath, destBlob, context);
                        
        }

        private void Context_FileTransferred(object sender, TransferEventArgs e)
        {
            throw new NotImplementedException();
        }

        async void UploadAsync(string sourcepath, CloudBlockBlob destBlob, TransferContext context)
        {
            await TransferManager.UploadAsync(
                sourcePath, destBlob, null, context, CancellationToken.None);
        }


    }
}
