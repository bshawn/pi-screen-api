using System;
using System.IO;
using System.Threading.Tasks;
using ScreenApi.DataAccess.Exceptions;

namespace ScreenApi.DataAccess
{
    internal class VideoRepository : IVideoRepository
    {
        private string dataPath;

        public VideoRepository(string dataPath)
        {
            if (string.IsNullOrEmpty(dataPath))
                throw new ArgumentException("Parameter cannot be null or empty string", dataPath);

            this.dataPath = dataPath;
        }

        public async Task UpdateVideo(Guid screenId, Stream video)
        {
            string filePath = BuildFilePath(screenId);
            using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096))
            {
                await video.CopyToAsync(fs);
            }
        }

        public FileStream GetVideo(Guid screenId)
        {
            string filePath = BuildFilePath(screenId);
            if (!File.Exists(filePath))
                throw new NotFoundException("The specified screen ID was not found");
            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096);
        }

        private string BuildFilePath(Guid screenId)
        {
            string fileName = $"{screenId}.mp4";
            return Path.Combine(dataPath, fileName);
        }
    }
}