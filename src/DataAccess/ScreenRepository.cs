using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ScreenApi.Models;

namespace ScreenApi.DataAccess
{
    internal class ScreenRepository : IScreenRepository
    {
        private string dataFilePath;

        public ScreenRepository(string dataFilePath)
        {
            if (string.IsNullOrEmpty(dataFilePath))
                throw new ArgumentException("Parameter cannot be null or empty string", dataFilePath);

            this.dataFilePath = dataFilePath;
        }

        public IEnumerable<Screen> GetAllScreens()
        {
            using (Stream fs = new FileStream(dataFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096))
            using (StreamReader sr = new StreamReader(fs))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                // read the json from a stream
                // json size doesn't matter because only a small piece is read at a time from the HTTP request
                return serializer.Deserialize<IEnumerable<Screen>>(reader)
                    .OrderBy(s => s.Name);
            }
        }

        public Screen GetScreenById(Guid id)
        {
            return GetAllScreens().FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Screen> FindScreens(string searchTerm)
        {
            return GetAllScreens().Where(s =>
            {
                return
                    s.Name.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                    s.Description.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                    s.Location.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase);
            });
        }
    }
}