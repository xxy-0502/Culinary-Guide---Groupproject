using System.Collections.Concurrent;

namespace Culinary_Guide.Services
{
    public interface IImageCacheService
    {
        Task<ImageSource> GetImageAsync(string url);
        void ClearCache();
    }

    public class ImageCacheService : IImageCacheService
    {
        private readonly ConcurrentDictionary<string, ImageSource> _cache = new();

        public Task<ImageSource> GetImageAsync(string url)
        {
            if (_cache.TryGetValue(url, out var cached))
                return Task.FromResult(cached);

            var imageSource = ImageSource.FromUri(new Uri(url));
            _cache[url] = imageSource;
            return Task.FromResult(imageSource);
        }

        public void ClearCache() => _cache.Clear();
    }
}
