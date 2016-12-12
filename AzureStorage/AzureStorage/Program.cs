using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace BlobStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            CloudStorageAccount account =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient client = account.CreateCloudBlobClient();

            var container =  client.GetContainerReference("mycontainer");

            container.CreateIfNotExists();

            Console.WriteLine("Upload a blob");

            Console.WriteLine("Please provide full filepath.");
            string filepath = Console.ReadLine();

            //1. Upload to block blob
            if(File.Exists(filepath))
            {
                using (var fileStream = File.OpenRead(filepath))
                {
                    var blob= container.GetBlockBlobReference("myblob");
                    blob.UploadFromStream(fileStream);
                }
                
            }
            Console.WriteLine("Block blob uploaded successfully.");


            //2. Listing the blobs in a container
            Console.WriteLine("List the blob items? Y/N");
            if (Console.ReadLine().Equals("Y", StringComparison.CurrentCultureIgnoreCase))
            {

                foreach (IListBlobItem item in container.ListBlobs(null, false))
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        CloudBlockBlob blockblob = (CloudBlockBlob)item;

                        Console.WriteLine("Block blob of length {0}:{1}", blockblob.Properties.Length, blockblob.Uri);
                    }
                    if (item.GetType() == typeof(CloudAppendBlob))
                    {
                        CloudAppendBlob appendblob = (CloudAppendBlob)item;

                        Console.WriteLine("Append blob of length {0}:{1}", appendblob.Properties.Length, appendblob.Uri);
                    }
                    if (item.GetType() == typeof(CloudBlobDirectory))
                    {
                        CloudBlobDirectory directory = (CloudBlobDirectory)item;

                        Console.WriteLine("Direcotry:{0}", directory.Uri);
                    }
                }
            }

            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference("myblob");
            //3. Download a blob
            Console.WriteLine("Download a blob? Y/N");
            if (Console.ReadLine().Equals("Y", StringComparison.CurrentCultureIgnoreCase))
            {
                string downloadFilePath = Console.ReadLine();

                using (var fileStream = File.OpenWrite(downloadFilePath))
                {
                    cloudBlockBlob.DownloadToStream(fileStream);
                }

                Console.WriteLine("Download complete.");
            }


            //4. Delete the blob
            Console.WriteLine("Delete the blob? Y/N");
            if (Console.ReadLine().Equals("Y", StringComparison.CurrentCultureIgnoreCase))
            {
                cloudBlockBlob.Delete();
                Console.WriteLine("Deleted successfully.");
            }


        }
    }
}
