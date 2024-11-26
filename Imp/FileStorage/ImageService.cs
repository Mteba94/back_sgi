namespace WebApi_SGI_T.Imp.FileStorage
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ImageStorage _imageStorage;

        public ImageService(ImageStorage imageStorage, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _imageStorage = imageStorage;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> SaveImage(IFormFile image, string container)
        {
            var webRootPath = _env.WebRootPath;
            var scheme = _httpContextAccessor.HttpContext!.Request.Scheme;
            var host = _httpContextAccessor.HttpContext!.Request.Host;

            return await _imageStorage.SaveFile(container, image, webRootPath, scheme, host.Value);
        }

        public async Task<string> UpdateUserImage(string container, string route, IFormFile newImage)
        {
            var webRootPath = _env.WebRootPath;
            var scheme = _httpContextAccessor.HttpContext!.Request.Scheme;
            var host = _httpContextAccessor.HttpContext!.Request.Host;

            return await _imageStorage.UpdateFile(container, newImage, route, webRootPath, scheme, host.Value);
        }

        public async Task RemoveUserImage(string route, string container)
        {
            var webRootPath = _env.WebRootPath;
            await _imageStorage.DeleteFile(route, container, webRootPath);
        }
    }
}
