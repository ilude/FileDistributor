using System;

namespace Truefit.FileDistributor.Models {

    [Serializable]
    public class FileMetadata {
        public string Filename { get; private set; }
        public long Filesize { get; private set; }
        public string ContentType { get; private set; }
        public Guid Guid { get; private set; }

        public FileMetadata(string filename, long filesize, string contentType) {
            Filename = filename;
            Filesize = filesize;
            ContentType = contentType;
            Guid = Guid.NewGuid();
        }
    }
}