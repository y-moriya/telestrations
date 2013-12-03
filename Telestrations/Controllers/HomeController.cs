using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Configuration;
using System.IO;
using Telestrations.Models;

namespace Telestrations.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult CanvasUpload(CanvasInputModel inputModel)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
            var blobStorage = storageAccount.CreateCloudBlobClient();
            var container = blobStorage.GetContainerReference("canvasimages");

            if (container.CreateIfNotExist())
            {
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }

            string uniqueBlobName = string.Format("canvasimages/image_{0}.png",
                Guid.NewGuid().ToString());
            var blob = blobStorage.GetBlockBlobReference(uniqueBlobName);
            blob.Properties.ContentType = "image/png";
            blob.UploadByteArray(System.Convert.FromBase64String(inputModel.Data));

            return View();
        }
    }
}