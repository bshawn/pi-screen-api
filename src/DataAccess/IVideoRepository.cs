using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ScreenApi.Models;

namespace ScreenApi.DataAccess
{
    internal interface IVideoRepository
    {
        FileStream GetVideo(Guid screenId);
        Task UpdateVideo(Guid screenId, Stream video);
    }
}