using System;
using System.Collections.Generic;
using ScreenApi.Models;

namespace ScreenApi.DataAccess
{
    internal interface IScreenRepository
    {
        IEnumerable<Screen> GetAllScreens();
        Screen GetScreenById(Guid id);
        IEnumerable<Screen> FindScreens(string searchTerm);
        void AddScreen(Screen screen);
        void UpdateScreen(Screen screen);
        void DeleteScreen(Screen screen);
    }
}