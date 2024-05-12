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

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public PlantsController(ILogger<PlantsController> logger,
            IClassAwareOptionsMonitor<FeatureFlagOptions> featureFlagsOptionsMonitor)
        {
            this.logger = logger;
            this.featureFlagsOptionsMonitor = featureFlagsOptionsMonitor;
        }

        [HttpGet("getplants", Name = nameof(GetPlantsAsync))]
        [ApiVersion(ApiVersions.V_2024_04_26.Name)]
        public async Task<IEnumerable<Plant>> GetPlantsAsync()
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

        [HttpGet("getplantscached", Name = nameof(GetPlantsCachedAsync))]
        [ApiVersion(ApiVersions.V_2024_04_26.Name)]
        public async Task<IEnumerable<Plant>> GetPlantsCachedAsync()
        {
            using var activity = Program.ActivitySource.StartMethodActivity(logger);

            //return await smartCache.GetAsync(
            //    new GetAllCacheKey(skip, take),
            //    _ => plantRepository.GetAllAsync(skip, take),
            //    new SmartCacheOperationOptions() { MaxAge = TimeSpan.FromMinutes(10) }
            //);



            // activity.SetOutput(result);
            return null;
        }

    }
}
