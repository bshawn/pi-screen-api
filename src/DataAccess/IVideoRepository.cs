using System;
using System.Collections.Generic;
using System.IO;
using ScreenApi.Models;

namespace ScreenApi.DataAccess
{
    internal interface IVideoRepository
    {
        FileStream GetVideo(Guid screenId);
        void AddVideo(Guid screenId, Stream video);
    }
}