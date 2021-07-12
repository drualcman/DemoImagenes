using BlazorIndexedDb.Models;
using BlazorIndexedDb.Configuration;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorIndexedDb.Store;
using DemoImagenes.Shared;

namespace DemoImagenes.Client.IndexedDb
{
    public class DBContext : StoreContext
    {
        #region properties
        //public FilesStoreService FilesStoreService { get; private set; }
        public StoreSet<FileStore> FilesStore { get; set; }
        #endregion


        #region constructor
        public DBContext(IJSRuntime js) : base(js)
        {
            Settings.EnableDebug = true;
            Settings settings = new Settings("FilesStoreDb");
            settings.AssemblyName = "DemoImagenes.Shared";
            Init(settings);
        }
        #endregion

        #region helpers
        public void ProcessErrors(List<ResponseJsDb> result)
        {
            StringBuilder errors = new StringBuilder();
            foreach (ResponseJsDb error in result)
            {
                errors.Append($"{error.Message}< br />");
            }
            Console.WriteLine(errors);
        }
        #endregion
    }
}
