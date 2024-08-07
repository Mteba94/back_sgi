namespace WebApi_SGI_T.Imp.FileStorage
{
    public class ImageService
    {
        private readonly ImageStorage _imageStorage;

        public ImageService(ImageStorage imageStorage)
        {
            _imageStorage = imageStorage;
        }

        public async Task<string> SaveImage(IFormFile image, string container)
        {
            if (image == null || image.Length == 0)
            {
                throw new ArgumentException("No image provided.");
            }

            var imageUrl = await _imageStorage.SaveFile(container, image);
            return imageUrl;
        }

        public async Task<string> UpdateUserImage(string container, string existingFilePath, IFormFile newImage)
        {
            var newFilePath = await _imageStorage.UpdateFile(container, existingFilePath, newImage);
            // Actualiza la información del usuario en la base de datos con la nueva ruta de la imagen
            return newFilePath;
        }

        public void RemoveUserImage(string filePath)
        {
            _imageStorage.DeleteFile(filePath);
            // Elimina la referencia de la imagen en la base de datos si es necesario
        }
    }
}
