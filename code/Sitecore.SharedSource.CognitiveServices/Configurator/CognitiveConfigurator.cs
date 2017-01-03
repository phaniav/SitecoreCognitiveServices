﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Foundation.DependencyInjection;
using Sitecore.SharedSource.CognitiveServices.Api;
using Sitecore.SharedSource.CognitiveServices.Api.Common;
using Sitecore.SharedSource.CognitiveServices.Api.Knowledge;
using Sitecore.SharedSource.CognitiveServices.Api.Language;
using Sitecore.SharedSource.CognitiveServices.Api.Speech;
using Sitecore.SharedSource.CognitiveServices.Api.Video;
using Sitecore.SharedSource.CognitiveServices.Api.Vision;
using Sitecore.SharedSource.CognitiveServices.Foundation;

namespace Sitecore.SharedSource.CognitiveServices.Configurator
{
    public class CognitiveConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICognitiveContext, CognitiveContext>();
            serviceCollection.AddSingleton<IApiKeys, ApiKeys>();
            serviceCollection.AddSingleton<IApiService, ApiService>();
            serviceCollection.AddSingleton<IEmotionApi, EmotionApi>();
            serviceCollection.AddSingleton<IEntityLinkingApi, EntityLinkingApi>();
            serviceCollection.AddSingleton<IFaceApi, FaceApi>();
            serviceCollection.AddSingleton<ILanguageApi, LanguageApi>();
            serviceCollection.AddSingleton<ISentimentApi, SentimentApi>();
            serviceCollection.AddSingleton<ISpeakerIdentificationApi, SpeakerIdentificationApi>();
            serviceCollection.AddSingleton<ISpeakerVerificationApi, SpeakerVerificationApi>();
            serviceCollection.AddSingleton<IVideoApi, VideoApi>();
            serviceCollection.AddSingleton<IVisionApi, VisionApi>();
            serviceCollection.AddSingleton<IWebUtilWrapper, WebUtilWrapper>();

            serviceCollection.AddMvcControllersInCurrentAssembly();
        }
    }
}