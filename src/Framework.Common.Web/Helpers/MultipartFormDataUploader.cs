using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace Framework.Common.Web.Helpers
{
    public class MultipartFormDataUploader
    {
        // You could extract these two private methods to a separate utility class since
        // they do not really belong to a controller class but that is up to you
        public static MultipartFormDataStreamProvider GetMultipartProvider(string serverPath)
        {
            //var uploadFolder = "~/Content/Uploads/FactibilidadTerrenos"; // you could put this to web.config
            //var root = HttpContext.Current.Server.MapPath(uploadFolder);
            Directory.CreateDirectory(serverPath);
            return new MultipartFormDataStreamProvider(serverPath);
        }

        // Extracts Request FormatData as a strongly typed model
        public static object GetFormData<T>(MultipartFormDataStreamProvider result)
        {
            if (result.FormData.HasKeys())
            {
                var value = result.FormData.GetValues(0).FirstOrDefault();

                var unescapedFormData = Uri.UnescapeDataString(value ?? string.Empty);
                if (!String.IsNullOrEmpty(unescapedFormData))
                    return JsonConvert.DeserializeObject<T>(unescapedFormData);
            }

            return null;
        }

        public static string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = GetFileName(fileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }

        public static string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }
    }
}
