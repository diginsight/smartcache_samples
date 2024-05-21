using Asp.Versioning;
using Diginsight.CAOptions;
using Diginsight.Diagnostics;
using Diginsight.SmartCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace SampleWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "common")]
    public class PlantsController : ControllerBase
    {
        private readonly ILogger<PlantsController> logger;
        private readonly IClassAwareOptionsMonitor<FeatureFlagOptions> featureFlagsOptionsMonitor;
        private readonly ISmartCache smartCache;
        private readonly ICacheKeyService cacheKeyService;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public PlantsController(ILogger<PlantsController> logger,
            IClassAwareOptionsMonitor<FeatureFlagOptions> featureFlagsOptionsMonitor,
            ISmartCache smartCache,
            ICacheKeyService cacheKeyService)
        {
            this.logger = logger;
            this.featureFlagsOptionsMonitor = featureFlagsOptionsMonitor;
            this.smartCache = smartCache;
            this.cacheKeyService = cacheKeyService;
        }

        [HttpGet("getplants", Name = nameof(GetPlantsAsync))]
        [ApiVersion(ApiVersions.V_2024_04_26.Name)]
        public async Task<IEnumerable<Plant>> GetPlantsAsync()
        {
            using var activity = Program.ActivitySource.StartMethodActivity(logger); // , new { foo, bar }

            var result = default(IEnumerable<Plant>);

            Thread.Sleep(1000);

            // read string plantsString from content file /Content/plants.json
            var plantsString = await System.IO.File.ReadAllTextAsync("Content/plants.json");
            var plants = JsonConvert.DeserializeObject<IEnumerable<Plant>>(plantsString);

            activity.SetOutput(plants);
            return plants;
        }

        [HttpGet("getplantscached", Name = nameof(GetPlantsCachedAsync))]
        [ApiVersion(ApiVersions.V_2024_04_26.Name)]
        public async Task<IEnumerable<Plant>> GetPlantsCachedAsync()
        {
            using var activity = Program.ActivitySource.StartMethodActivity(logger);

            var plants = await smartCache.GetAsync(
                new MethodCallCacheKey(cacheKeyService, typeof(PlantsController), nameof(GetPlantsCachedAsync)),
                _ => GetPlantsAsync(),
                new SmartCacheOperationOptions() { MaxAge = TimeSpan.FromMinutes(10) }
            );

            activity.SetOutput(plants);
            return plants;
        }

        [HttpGet("getplantbyid", Name = nameof(GetPlantByIdAsync))]
        [ApiVersion(ApiVersions.V_2024_04_26.Name)]
        public async Task<IEnumerable<Plant>> GetPlantByIdAsync(Guid id)
        {
            using var activity = Program.ActivitySource.StartMethodActivity(logger); // , new { foo, bar }

            var result = default(IEnumerable<Plant>);

            Thread.Sleep(1000);

            var plants = new List<Plant>();
            plants.Add(new Plant() { Id = Guid.NewGuid(), Name = "Plant1 Name" });
            plants.Add(new Plant() { Id = Guid.NewGuid(), Name = "Plant2 Name" });

            var s = JsonConvert.SerializeObject(plants);

            activity.SetOutput(result);
            return result;
        }

        [HttpGet("getplantbyidcached", Name = nameof(GetPlantByIdCachedAsync))]
        [ApiVersion(ApiVersions.V_2024_04_26.Name)]
        public async Task<IEnumerable<Plant>> GetPlantByIdCachedAsync(Guid id)
        {
            using var activity = Program.ActivitySource.StartMethodActivity(logger);

            var plants = await smartCache.GetAsync(
                new MethodCallCacheKey(cacheKeyService, typeof(PlantsController), nameof(GetPlantByIdCachedAsync)),
                _ => GetPlantByIdAsync(id),
                new SmartCacheOperationOptions() { MaxAge = TimeSpan.FromMinutes(10) }
            );

            activity.SetOutput(plants);
            return plants;
        }
    }
}
