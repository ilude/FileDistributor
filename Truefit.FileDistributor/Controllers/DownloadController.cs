using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Mvc;
using Truefit.FileDistributor.Models;

namespace Truefit.FileDistributor.Controllers {
    public class DownloadController : Controller {
        public ActionResult Index(string id) {
            FileMetadata metadata;

            var filepath = Path.Combine(MvcApplication.FileDirectory, id);

            if (!System.IO.File.Exists(filepath)) {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Content("No such file!");
            }

            var serializer = new BinaryFormatter();
            using (var stream = System.IO.File.OpenRead(Path.Combine(MvcApplication.MetadataDirectory, id))) {
                metadata = serializer.Deserialize(stream) as FileMetadata;
            }

            return File(filepath, metadata.ContentType, metadata.Filename);
        }
    }
}
