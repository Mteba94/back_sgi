using WebApi_SGI_T.Models;

namespace WebApi_SGI_T.Imp.FileStorage
{
    public class ImageStorage
    {
        private readonly string _storagePath;

        public ImageStorage(IConfiguration configuration)
        {
            var storagePath = configuration["Storage:Path"];
            if (string.IsNullOrEmpty(storagePath))
            {
                throw new InvalidOperationException("Storage path is not configured.");
            }

            _storagePath = Path.Combine(AppContext.BaseDirectory, storagePath);
            Directory.CreateDirectory(_storagePath);
        }

        public async Task<string> SaveFile(string container, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file provided.");
            }

            var containerPath = Path.Combine(_storagePath, container);
            Directory.CreateDirectory(containerPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(containerPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

        public async Task<string> UpdateFile(string container, string existingFilePath, IFormFile newFile)
        {
            if (newFile == null || newFile.Length == 0)
            {
                throw new ArgumentException("No file provided.");
            }

            // Eliminar el archivo existente si existe
            if (File.Exists(existingFilePath))
            {
                File.Delete(existingFilePath);
            }

            // Guardar el nuevo archivo
            var containerPath = Path.Combine(_storagePath, container);
            Directory.CreateDirectory(containerPath);

            var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(newFile.FileName);
            var newFilePath = Path.Combine(containerPath, newFileName);

            using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await newFile.CopyToAsync(stream);
            }

            return newFilePath;
        }

        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                throw new FileNotFoundException("File not found.", filePath);
            }
        }

    }
}
