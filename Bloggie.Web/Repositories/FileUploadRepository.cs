
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Bloggie.Web.Repositories
{
    public class FileUploadRepository : IImageRepository
    {
        private readonly IConfiguration _configuration;
       
        private string UploadUrl;
        private readonly IWebHostEnvironment _environment;

        public FileUploadRepository(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
            UploadUrl = _configuration["FileUploadationUrl"];
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            // Ensure Files directory exists
            string uploadsFolder = Path.Combine(_environment.WebRootPath, "Files");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate unique filename if needed
            string filePath = Path.Combine(uploadsFolder, file.FileName);
            string fileName = file.FileName;
            int count = 1;
            while (File.Exists(filePath))
            {
                string fileExt = Path.GetExtension(file.FileName);
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(file.FileName);
                fileName = $"{fileNameWithoutExt}_{count}{fileExt}";
                filePath = Path.Combine(uploadsFolder, fileName);
                count++;
            }

            // Save file locally
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Generate local URL
            string Url = $"{UploadUrl}/Files/{fileName}";

          

            return Url;
        }
    }
}
