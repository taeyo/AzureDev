﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.DataMovement;
using System.Net;
using System.Diagnostics;
using AzureStorageUpload.Extensions;

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
        string destBlobName = string.Empty;
        int threadCount = 4;
        long fileSize;

        Stopwatch watcher;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount * 8;

            watcher = new Stopwatch();

            // 파일 다이얼로그 기본 설정
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            // 진행바 기본 설정
            progressBar1.Minimum = 1;
            progressBar1.Maximum = 100;
            progressBar1.Value = 1;

            //progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.Enabled = true;
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
                MessageBox.Show("유효한 계정 정보가 아닙니다\n\n" + fex.Message, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("저장소에 접근할 수 없습니다. 계정명을 다시 확인하세요\n\n" + ex.Message, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 컨트롤들의 상태 변경
            uploadButton.Enabled = true;
            ConnectState.ForeColor = Color.Blue;
            ConnectState.Text = "* 연결되었습니다 (동일한 이름의 파일은 덮어씁니다)";
        }
        private void uploadButton_Click(object sender, EventArgs e)
        {
            timespent.Text = string.Empty;

            if (FilePath.Text.Length == 0)
            {
                MessageBox.Show("파일을 선택해 주세요", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int tNum = 0;
            if (Int32.TryParse(ThreadNum.Text, out tNum)) threadCount = tNum;

            uploadButton.Enabled = false;
            watcher.Restart();

            // Create the destination CloudBlob instance
            CloudBlockBlob destBlob = blobContainer.GetBlockBlobReference(destBlobName);

            // Setup the number of the concurrent operations
            TransferManager.Configurations.ParallelOperations = threadCount;

            // Setup the transfer context and track the upoload progress
            TransferContext context = new TransferContext();

            //기존 파일이 있다면 덮어쓰도록 설정
            context.OverwriteCallback = (source, destination) => true;

            long bytesTransferred = 0;
            context.ProgressHandler = new Progress<TransferProgress>((progress) =>
            {
                bytesTransferred = progress.BytesTransferred;
                
                Log.Text = "Bytes uploaded: " + (bytesTransferred == 0 ? "초기화 중.." : bytesTransferred.ToFileSize());
                Mps.Text = string.Format("({0}/s)", GetMps(bytesTransferred, watcher.Elapsed.TotalMilliseconds).ToFileSize());

                // 상태바 진행상황 변경
                progressBar1.Value = GetProgressPercent(bytesTransferred);              

                Console.WriteLine("Bytes uploaded: {0}", bytesTransferred);
                //Console.WriteLine("Stopwatch: {0}", watcher.Elapsed.TotalMilliseconds);
                //Console.WriteLine("Stopwatch: {0}", mps);
                //Console.WriteLine("Stopwatch: {0}", mps.ToFileSize());
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
                uploadButton.Enabled = true;
            });

            // 다음의 코드는 UI가 있는 환경에서는 UI 블로킹이 일어나기에 async로 분리함.
            //var task = TransferManager.UploadAsync(
            //    sourcePath, destBlob, null, context, CancellationToken.None);
            //task.Wait();

            UploadAsync(sourcePath, destBlob, context);
                        
        }

        private int GetProgressPercent(long bytesTransferred)
        {
            int percent = (int)Math.Round((double)(100 * bytesTransferred) / fileSize);
            return percent = percent == 0 ? 1 : percent;
        }

        private long GetMps(long fileSize, double totalMilliseconds)
        {
            return (long)Math.Round((double)(fileSize) / (totalMilliseconds / 1000));
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
                sourcePath = openFileDialog1.FileName;
                FilePath.Text = sourcePath;
                destBlobName = System.IO.Path.GetFileName(sourcePath);
                fileSize = new System.IO.FileInfo(sourcePath).Length;
            }
        }
    }
}
