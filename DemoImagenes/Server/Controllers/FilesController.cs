using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoImagenes.Server.Controllers
{
    [Route("files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment Env;
        public FilesController(IWebHostEnvironment env)
        {
            Env = env;
        }

        [HttpPost("a/disco")]
        public async Task<bool> Disco([FromForm] IEnumerable<IFormFile> files)
        {
            bool result;
            if (files.Count() > 0)
            {
                string folder = Path.Combine(
                    Env.ContentRootPath.Replace("Server", "Client"), 
                    "wwwroot\\images");
                result = await UploadImage(files, folder);
            }
            else
            {
                result = false;
            }
            return result;
        }

        [HttpPost("a/db")]
        public async Task<bool> DB([FromForm] IEnumerable<IFormFile> files)
        {
            bool result;

            string cadenaConexion = 
                "Server=(localdb)\\mssqllocaldb;Database=FilesDb";

            using SqlConnection sqlConnection = new SqlConnection(cadenaConexion);
            using SqlCommand cmd = sqlConnection.CreateCommand();
            string sql = @"insert into FilesStore (FileName, FileSize, FileBytes) 
                           values (@name, @size, @bytes);";

            (string name, long size, byte[] content) file = 
                await GetBytesAsync(files);

            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@name", file.name);
            cmd.Parameters.AddWithValue("@size", file.size);
            cmd.Parameters.Add("@bytes", SqlDbType.Image);
            cmd.Parameters["@bytes"].Value = file.content;
            await cmd.Connection.OpenAsync();
            try
            {
                await cmd.ExecuteNonQueryAsync();
                result = true;
            }
            catch
            {
                result = false;
            }
            await cmd.Connection.CloseAsync();

            return result;
        }

        async Task<(string name, long size, byte[] content)> 
            GetBytesAsync(IEnumerable<IFormFile> files)
        {
            (string name, long size, byte[] content) response = new();
            try
            {
                if (files.Count() > 0)
                {
                    IFormFile file = files.First();
                    using StreamContent stream =
                        new StreamContent(file.OpenReadStream());
                    response.content = await stream.ReadAsByteArrayAsync();
                    response.name = file.FileName;
                    response.size = file.Length;
                }
                else
                {
                    response.content = new byte[] { 0 };
                    response.name = "No files";
                    response.size = 0;
                }
            }
            catch (Exception ex)
            {

                response.content = new byte[] { 0 };
                response.name = ex.Message;
                response.size = 0;
            }
            return response;
        }

        async Task<bool> UploadImage(IEnumerable<IFormFile> files, 
            string folderPath)
        {
            bool result = false;
            FileStream ms;
            foreach (IFormFile file in files)
            {
                try
                {
                    string FileNameForFileStorage;
                    string FileName = file.FileName;
                    string FileNameForDisplay = WebUtility.HtmlEncode(FileName);
                    string extension = Path.GetExtension(file.FileName);
                    FileNameForFileStorage = Path.GetRandomFileName()
                        .Replace(".", "") + extension;
                    string path = Path.Combine(folderPath, FileNameForFileStorage);

                    ms = System.IO.File.Create(path);
                    await file.CopyToAsync(ms);
                    await ms.DisposeAsync();
                    result = true;
                }
                catch 
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
