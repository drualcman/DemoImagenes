using BlazorIndexedDb.Store;
using DemoImagenes.Shared;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoImagenes.Client.IndexedDb
{
    public class FilesStoreService: StoreService<FileStore>
    {
        public FilesStoreService(IJSRuntime js) : base(js) { }

    }
}
