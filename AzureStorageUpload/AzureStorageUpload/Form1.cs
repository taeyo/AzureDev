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
        string destBlobName = "myblob";
        int threadCount = 4;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount * 8;

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            //일본 서부
            storageConnectionString = String.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", AccountName.Text, AccountKey.Text);
            //미국 서부
            //storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=vhdsc9dn6pnh7qeemxvub333;AccountKey=***";

            try
            {
                account = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException fex)
            {
                MessageBox.Show("유효한 계정 정보가 아닙니다", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
 

            blobClient = account.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(containerName);
            try
            {
                blobContainer.CreateIfNotExists();
            }
            catch (Exception ex)
            {
                MessageBox.Show("저장소에 접근할 수 없습니다. 계정명을 다시 확인하세요", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 컨트롤들의 상태 변경
            uploadButton.Enabled = true;
            ConnectState.ForeColor = Color.Blue;
            ConnectState.Text = "* 연결되었습니다";
        }
        private void uploadButton_Click(object sender, EventArgs e)
        {
            if(FilePath.Text.Length == 0)
            {
                MessageBox.Show("파일을 선택해 주세요", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Stopwatch watcher = new Stopwatch();
            watcher.Start();

            // Create the destination CloudBlob instance
            CloudBlockBlob destBlob = blobContainer.GetBlockBlobReference(destBlobName);

            // Setup the number of the concurrent operations
            TransferManager.Configurations.ParallelOperations = threadCount;
            // Setup the transfer context and track the upoload progress
            TransferContext context = new TransferContext();

            //기존 파일이 있다면 덮어쓰도록 설정
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

                timespent.Text = "Total Elapsed Time : " + elapsedTime.ToString();
            });

            // Upload a local blob
            //var task = TransferManager.UploadAsync(
            //    sourcePath, destBlob, null, context, CancellationToken.None);
            //task.Wait();
            UploadAsync(sourcePath, destBlob, context);
                        
        }

        async void UploadAsync(string sourcepath, CloudBlockBlob destBlob, TransferContext context)
        {
            await TransferManager.UploadAsync(
                sourcePath, destBlob, null, context, CancellationToken.None);
        }

        private void Browse_Click(object sender, EventArgs e)
        { 

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FilePath.Text = openFileDialog1.FileName;
                destBlobName = openFileDialog1.SafeFileName;
            }
        }
    }
}
