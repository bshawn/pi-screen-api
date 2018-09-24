using System;

namespace ScreenApi.Models
{
    public class Screen
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}