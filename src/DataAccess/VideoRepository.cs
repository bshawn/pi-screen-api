using System;
using System.IO;

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

        public void AddVideo(Guid screenId, Stream video)
        {
            throw new NotImplementedException();
        }

        public FileStream GetVideo(Guid screenId)
        {
            string fileName = $"{screenId}.mp4";
            string filePath = Path.Combine(dataPath, fileName);
            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096);
        }
    }
}