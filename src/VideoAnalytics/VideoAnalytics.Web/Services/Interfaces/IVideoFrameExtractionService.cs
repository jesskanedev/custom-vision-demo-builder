﻿using System.Threading.Tasks;
using VideoAnalytics.Web.Models;

namespace VideoAnalytics.Web.Services.Interfaces
{
    public interface IVideoFrameExtractionService
    {
        Task<VideoFrameExtractionResponse> SaveImageFrames(string videoFile, string saveImagesTo, int frameStepMilliseconds, int maxDurationMilliseconds);
    }
}