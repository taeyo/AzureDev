using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DxRemember.Utils
{
    public class Upload2ASS
    {
        string containerName = "";
        string StorageConnectionString = string.Empty;
        CloudBlobContainer storageContainer;

        public Upload2ASS()
        {
            // 저장소 연결 문자열 가져오기
            StorageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");

            containerName = CloudConfigurationManager.GetSetting("ContainerName");

            // 저장소 계정 가져오기
            // 여기서 runtime 에러나면 Web.config에 가서 저장소 계정 액세스 키를 설정하세요
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);

            // blob 클라이언트 생성.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // 기존의 컨테이너 참조 가져오기
            storageContainer = blobClient.GetContainerReference(containerName);
        }

        public Uri UploadFilesToAzureStorage(string fileName, Stream stream)
        {
            CloudBlockBlob blockBlob = storageContainer.GetBlockBlobReference(fileName);
            blockBlob.UploadFromStream(stream);

            return blockBlob.StorageUri.PrimaryUri;
        }
    }
}