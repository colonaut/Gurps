using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MedienKultur.Gurps.Controllers
{
    public class JUploadController : ApiController
    {
        //jupload -> this will upload a file into the user directory
        public async Task<HttpResponseMessage> PostFormData(string type)
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }


            //save as file
            //string root = HttpContext.Current.Server.MapPath("~/App_Data");
            //var provider = new MultipartFormDataStreamProvider(root);

            //get a memory stream
            var provider = new MultipartMemoryStreamProvider();



            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                string foo = "?";

                foreach (var httpContent in provider.Contents)
                {
                    Trace.WriteLine(httpContent.Headers.ContentDisposition.FileName);
                    foo = httpContent.ReadAsStringAsync().Result;
                }

                var bar = foo;

                // This illustrates how to get the file names.
                //foreach (MultipartFileData file in provider.FileData)
                //{
                //    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                //    Trace.WriteLine("Server file path: " + file.LocalFileName);
                //}
                
                
                
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

    }
}
