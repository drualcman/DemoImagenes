using System;
using System.Collections.Generic;
using System.Text;

namespace DemoImagenes.Shared
{
    public class FileStore
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileBytes { get; set; }
    }
}
