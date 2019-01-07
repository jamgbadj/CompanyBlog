using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyBlog.Data.FileManager
{
    public class FileManager : IFileManager
    {
        private string _imagePath;

        public FileManager(IConfiguration config)
        {
            _imagePath = config["Path:Images"];
        }

        public FileStream ImageStream(string image)
        {
            return new FileStream(Path.Combine(_imagePath, image), FileMode.Open, FileAccess.Read);
        }

        public async Task<string> SaveImage(IFormFile image)
        {
            try
            {
                var saveDir = Path.Combine(_imagePath);
                if (!Directory.Exists(saveDir))
                {
                    Directory.CreateDirectory(saveDir);
                }

                //Get file format/mime type
                var mimeType = image.FileName.Substring(image.FileName.LastIndexOf('.'));
                //create file name to be saved to saveDir
                var fileName = $"img_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm")}{mimeType}";

                using (var fileStream = new FileStream(Path.Combine(saveDir, fileName), FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                return fileName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Error, Image could not be saved";
            }
        }
    }
}
