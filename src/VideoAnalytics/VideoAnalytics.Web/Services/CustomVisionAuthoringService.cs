﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using VideoAnalytics.Web.Configuration.Interfaces;
using VideoAnalytics.Web.Services.Interfaces;

namespace VideoAnalytics.Web.Services
{
    public class CustomVisionAuthoringService : ICustomVisionAuthoringService
    {
        private readonly ICustomVisionProjectService _projectService;
        private readonly CustomVisionTrainingClient _trainingApi;

        public CustomVisionAuthoringService(
            ICustomVisionProjectService projectService,
            ICustomVisionServiceSettings serviceSettings)
        {
            _projectService = projectService;
            _trainingApi = new CustomVisionTrainingClient(new ApiKeyServiceClientCredentials(serviceSettings.AccountKey))
            {
                Endpoint = serviceSettings.AccountEndpoint
            };
        }

        public async Task PublishImages(IList<string> imageFilePaths, string customVisionProjectName)
        {
            var imageFileEntries = new List<ImageFileCreateEntry>();
            foreach (var imageFilePath in imageFilePaths)
            {
                var fileName = Path.GetFileNameWithoutExtension(imageFilePath);
                var entry = new ImageFileCreateEntry(fileName, await File.ReadAllBytesAsync(imageFilePath));
                imageFileEntries.Add(entry);
            }

            var projectId = await _projectService.GetProjectId(customVisionProjectName);
            await _trainingApi.CreateImagesFromFilesAsync(projectId, new ImageFileCreateBatch(imageFileEntries));

            // TODO - validations
            //64 item limit on publish batches
            //6MB limit on file sizes
        }
    }
}
