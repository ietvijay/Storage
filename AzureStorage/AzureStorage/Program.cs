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

            if(File.Exists(filepath))
            {
                using (var fileStream = File.OpenRead(filepath))
                {

                    var blob= container.GetBlobReference("myblob");

                  
                }
                
            }


        }
    }
}
