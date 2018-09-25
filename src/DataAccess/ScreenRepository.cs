using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using ScreenApi.DataAccess.Exceptions;
using ScreenApi.Models;

namespace ScreenApi.DataAccess
{
    internal class ScreenRepository : IScreenRepository
    {
        private string dataFilePath;
        private JsonSerializer serializer;

        public ScreenRepository(string dataFilePath)
        {
            if (string.IsNullOrEmpty(dataFilePath))
                throw new ArgumentException("Parameter cannot be null or empty string", dataFilePath);

            this.dataFilePath = dataFilePath;

            this.serializer = new JsonSerializer();
            this.serializer.Converters.Add(new StringEnumConverter());
            this.serializer.Formatting = Formatting.Indented;
            this.serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public IEnumerable<Screen> GetAllScreens()
        {
            return ReadScreenFile().OrderBy(s => s.Name);
        }

        private IEnumerable<Screen> ReadScreenFile()
        {
            using (Stream fs = new FileStream(dataFilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read, 4096))
            using (StreamReader sr = new StreamReader(fs))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                // read the json from a stream
                // json size doesn't matter because only a small piece is read at a time from the HTTP request
                return serializer.Deserialize<IEnumerable<Screen>>(reader);
            }
        }

        public Screen GetScreenById(Guid id)
        {
            return GetScreenById(GetAllScreens(), id);
        }

        private Screen GetScreenById(IEnumerable<Screen> screens, Guid id)
        {
            return screens.FirstOrDefault(s => s.Id == id);
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

        public void AddScreen(Screen screen)
        {
            var screens = GetAllScreens().ToList();
            if (GetScreenById(screens, screen.Id) != null)
                throw new AlreadyExistsException("A screen with the specified ID already exists");

            screens.Add(screen);
            WriteScreenFile(screens);
        }

        private void WriteScreenFile(IEnumerable<Screen> screens)
        {
            using (Stream fs = new FileStream(dataFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write, 4096))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                // read the json from a stream
                // json size doesn't matter because only a small piece is read at a time from the HTTP request
                serializer.Serialize(writer, screens);
            }
        }

        public void UpdateScreen(Screen screen)
        {
            var screens = GetAllScreens().ToList();
            var old = GetScreenById(screens, screen.Id);
            if (old == null)
                throw new NotFoundException("The specified screen ID was not found");

            screens.Remove(old);
            screens.Add(screen);
            WriteScreenFile(screens);
        }

        public void DeleteScreen(Guid id)
        {
            var screens = GetAllScreens().ToList();
            var screen = GetScreenById(screens, id);
            if (screen == null)
                throw new NotFoundException("The specified screen ID was not found");

            screens.Remove(screen);
            WriteScreenFile(screens);
        }
    }
}