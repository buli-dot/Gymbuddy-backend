﻿namespace Gymbuddy.Utilities
{
    public class FileManager
    {
        private string? postFile;
        private readonly IWebHostEnvironment _hostEnviroment;
        public FileManager(IWebHostEnvironment hostEnviroment)
        {
            _hostEnviroment = hostEnviroment;
        }

        public string postUpload(IFormFile? file)
        {
            string wwwRootPath = _hostEnviroment.WebRootPath;
            string fileName = Guid.NewGuid().ToString();
            var uploads = Path.Combine(@"images");
            var extension = Path.GetExtension(file.FileName);
            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                file.CopyTo(fileStreams);
            }
            return postFile = @"\images\" + fileName + extension;


        }
        public string profilePhoto(IFormFile? file)
        {

            string wwwRootPath = _hostEnviroment.WebRootPath;
            string fileName = Guid.NewGuid().ToString();
            var uploads = Path.Combine(wwwRootPath, @"images\profilePhotos");
            var extension = Path.GetExtension(file.FileName);
            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                file.CopyTo(fileStreams);
            }
            return postFile = @"\images\profilePhotos\" + fileName + extension;


        }
    }
}
