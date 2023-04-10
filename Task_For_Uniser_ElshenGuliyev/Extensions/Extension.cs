using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Task_For_Uniser_ElshenGuliyev.Extensions
{
    public static class Extension
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image");
        }
        public static bool Max5Mb(this IFormFile file)
        {
            return file.Length / 1024 < 5120;
        }
        public static async Task<string> SaveFileAsync(this IFormFile file,string folder)
        {
            string filename = Guid.NewGuid().ToString() + file.FileName;
            string path = Path.Combine(folder, filename);
            using(FileStream fileStream=new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return filename;
        }
    }
}
