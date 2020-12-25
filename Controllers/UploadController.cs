using FileUploadAPI_4_6.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace FileUploadAPI_4_6.Controllers
{
    //[System.Web.Http.Route("api/[controller]")]

    public class UploadController : ApiController
    {
        public UploadController()
        {
        }


        public UploadResult Upload()
        {

            DirectoryClean();
            var docfiles = new List<string>();

            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/Uploads/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    docfiles.Add(filePath);
                }
                var r = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }


            UploadResult result = new UploadResult
            {
                FilePath = docfiles[0]
            };
            return result;
        }

        private void DirectoryClean()
        {
            var dir = HttpContext.Current.Server.MapPath("~/Uploads/");

            try
            {

                DirectoryInfo info = new DirectoryInfo(dir);
                FileInfo[] files = info.GetFiles();
                foreach (FileInfo file in files)
                {
                    DateTime dt = file.LastWriteTime;
                    TimeSpan ts = DateTime.Now - dt;

                    if (ts.TotalMinutes > 10)
                    {
                        System.IO.File.Delete(file.FullName);
                    }

                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}
