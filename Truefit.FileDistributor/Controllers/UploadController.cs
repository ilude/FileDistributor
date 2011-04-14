using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Truefit.FileDistributor.Models;

namespace Truefit.FileDistributor.Controllers {
    public class UploadController : Controller {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            if(Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase)) {
                return Redirect("http://www.truefitsolutions.com/");
            }

            if(Request.Files.Count <= 0 || Request.Files.Count > 1) {
                return Redirect("http://www.truefitsolutions.com/");
            }

            if(Request.Files[0] == null) {
                return Redirect("http://www.truefitsolutions.com/");
            }

            var file = Request.Files[0];

            using(var hasher = new SHA1Managed())
            using(var cryptoStream = new CryptoStream(Stream.Null, hasher, CryptoStreamMode.Write))
            using(var stream = new MemoryStream()) {
                file.InputStream.CopyTo(stream);
                stream.Position = 0;

                Write(cryptoStream, "blob {0}", stream.Length);
                cryptoStream.WriteByte(0);
                Write(cryptoStream, stream);

                cryptoStream.FlushFinalBlock();

                var buffer = new StringBuilder();

                foreach(var b in hasher.Hash) {
                    buffer.Append(b.ToString("x2"));
                }

                stream.Position = 0;

                var name = buffer.ToString();
                var filepath = Path.Combine(MvcApplication.FileDirectory, name);

                using(var filestream = System.IO.File.OpenWrite(filepath)) {
                    stream.CopyTo(filestream);
                }

                var metapath = Path.Combine(MvcApplication.MetadataDirectory, name);
                var metadata = new FileMetadata(file.FileName, file.ContentLength, file.ContentType);
                var serializer = new BinaryFormatter();

                using(var metastream = new FileStream(metapath, FileMode.OpenOrCreate, FileAccess.Write)) {
                    serializer.Serialize(metastream, metadata);
                }

                var url = string.Format(
                    "{0}://{1}{2}",
                    Request.Url.Scheme,
                    Request.Url.Authority,
                    VirtualPathUtility.ToAbsolute("~" + Url.RouteUrl("Default", new {id = name}))
                );

                return Json(new {Url = url});
            }
        }

        private static void Write(Stream stream, Stream input) {
            var buffer = new byte[32768];
            int read;
            while((read = input.Read(buffer, 0, buffer.Length)) > 0) {
                stream.Write(buffer, 0, read);
            }
        }

        private static void Write(Stream stream, string format, params object[] args) {
            if(args != null && args.Length > 0) {
                format = string.Format(format, args);
            }

            var buffer = Encoding.UTF8.GetBytes(format);

            stream.Write(buffer, 0, buffer.Length);
        }
    }
}
