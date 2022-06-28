using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Extensions
{
    public static class FileManager
    {
        public static bool CheckContentType(this IFormFile file, string contentType)
        {
            return file.ContentType == contentType;
        }

        public static bool CheckFileLength(this IFormFile file, double length)
        {
            return ((double)file.Length / 1024) > length;
        }

        public async static Task<string> CreateAsync(this IFormFile file, IWebHostEnvironment env, params string[] folders)
        {
            string fileName = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + file.FileName;

            string path = Path.Combine(env.WebRootPath);

            foreach (string folder in folders)
            {
                path = Path.Combine(path, folder);
            }

            path = Path.Combine(path, fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;

        }

        //public async static Task<List<string>> CreateListAsync(this List<IFormFile> files, IWebHostEnvironment env, params string[] folders)
        //{
        //    List<string> photos = null;

        //    string fileName = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_";

        //    string path = Path.Combine(env.WebRootPath);

        //    foreach (var file in files)
        //    {
        //        fileName += file.FileName;

        //        photos.Add(fileName);

        //        foreach (string folder in folders)
        //        {
        //            path = Path.Combine(path, folder);
        //        }

        //        path = Path.Combine(path, fileName);

        //        using (FileStream stream = new FileStream(path, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }
        //    }

        //    return photos;
        //}
    }
}
