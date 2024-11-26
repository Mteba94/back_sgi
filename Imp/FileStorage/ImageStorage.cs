namespace WebApi_SGI_T.Imp.FileStorage
{
    public class ImageStorage
    {
        //private readonly string _storagePath;

        //public ImageStorage(IConfiguration configuration)
        //{
        //    var storagePath = configuration["Storage:Path"];
        //    if (string.IsNullOrEmpty(storagePath))
        //    {
        //        throw new InvalidOperationException("Storage path is not configured.");
        //    }

        //    _storagePath = Path.Combine(AppContext.BaseDirectory, storagePath);
        //    Directory.CreateDirectory(_storagePath);
        //}

        public async Task<string> SaveFile(string container, IFormFile file, string webRootPath, string scheme, string host)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(webRootPath, container);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string filePath = Path.Combine(folder, fileName);

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                await File.WriteAllBytesAsync(filePath, content);
            }

            var currentUrl = $"{scheme}://{host}";
            var pathDb = Path.Combine(currentUrl, container, fileName).Replace("\\", "/");
            return pathDb;
        }

        public async Task<string> UpdateFile(string container, IFormFile newFile, string route, string webRootPath, string scheme, string host)
        {
            await DeleteFile(route, container, webRootPath);
            return await SaveFile(container, newFile, webRootPath, scheme, host);
        }

        public Task DeleteFile(string route, string container, string webRootPath)
        {
            if (string.IsNullOrEmpty(route))
            {
                return Task.CompletedTask;
            }
            
            var fileName = Path.GetFileName(route);
            var directoryFile = Path.Combine(webRootPath, container, fileName);

            if(File.Exists(directoryFile))
            {
                File.Delete(directoryFile);
            }

            return Task.CompletedTask;
        }

    }
}
