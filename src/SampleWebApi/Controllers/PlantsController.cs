using Asp.Versioning;
using Diginsight.CAOptions;
using Diginsight.Diagnostics;
using Diginsight.SmartCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
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

            using var activity = Program.ActivitySource.StartMethodActivity(logger); // , new { foo, bar }

        }

        [HttpGet("getplantsimpl", Name = nameof(GetPlantsImplAsync))]
        [ApiVersion(ApiVersions.V_2024_04_26.Name)]
        public async Task<IEnumerable<Plant>> GetPlantsImplAsync()
        {
            using var activity = Program.ActivitySource.StartMethodActivity(logger); // , new { foo, bar }

            var result = default(IEnumerable<Plant>);

            Thread.Sleep(1000);

            // read string plantsString from content file /Content/plants.json
            var plantsString = await System.IO.File.ReadAllTextAsync("Content/plants.json");
            var plants = JsonConvert.DeserializeObject<IEnumerable<Plant>>(plantsString);

            activity?.SetOutput(plants);
            return plants;
        }

        [HttpGet("getplantbyidimpl/{id}", Name = nameof(GetPlantByIdImplAsync))]
        [ApiVersion(ApiVersions.V_2024_04_26.Name)]
        public async Task<Plant> GetPlantByIdImplAsync([FromRoute] Guid id)
        {
            using var activity = Program.ActivitySource.StartMethodActivity(logger, new { id });

            var result = default(IEnumerable<Plant>);

            Thread.Sleep(1000);

            var plants = await GetPlantsAsync();

            var plant = plants.FirstOrDefault(p => p.Id == id);

            activity?.SetOutput(plant);
            return plant;
        }


        [HttpGet("getplants", Name = nameof(GetPlantsAsync))]
        [ApiVersion(ApiVersions.V_2024_04_26.Name)]
        public async Task<IEnumerable<Plant>> GetPlantsAsync()
        {
            using var activity = Program.ActivitySource.StartMethodActivity(logger);

            var options = new SmartCacheOperationOptions() { MaxAge = TimeSpan.FromMinutes(10) };
            var cacheKey = new GetPlantByIdCacheKey(Guid.Empty); // cacheKeyService, 

            Task<IEnumerable<Plant>> getCachedValuesAsync() => smartCache.GetAsync(
                cacheKey, _ => GetPlantsImplAsync(), options
            );
            //cacheKey.ReloadAsync = getCachedValuesAsync;

            var plants = await getCachedValuesAsync();
            activity?.SetOutput(plants);
            return plants;
        }

        [HttpGet("getplantbyid/{plantId}", Name = nameof(GetPlantByIdAsync))]
        [ApiVersion(ApiVersions.V_2024_04_26.Name)]
        public async Task<Plant> GetPlantByIdAsync([FromRoute] Guid plantId)
        {
            using var activity = Program.ActivitySource.StartMethodActivity(logger);

            var options = new SmartCacheOperationOptions() { MaxAge = TimeSpan.FromMinutes(10) };
            var cacheKey = new GetPlantByIdCacheKey(plantId); //cacheKeyService, 

            Task<Plant?> getCachedValueAsync() => smartCache.GetAsync(
                cacheKey, _ => GetPlantByIdImplAsync(plantId), options
            );
            //cacheKey.ReloadAsync = getCachedValueAsync;

            var plant = await getCachedValueAsync();

            activity?.SetOutput(plant);
            return plant;
        }


        [HttpPost("createorupdateplant", Name = nameof(CreateOrUpdatePlant))]
        [ApiVersion(ApiVersions.V_2024_04_26.Name)]
        public async Task<IEnumerable<Plant>> CreateOrUpdatePlant(Plant newplant)
        {
            using var activity = Program.ActivitySource.StartMethodActivity(logger); // , new { foo, bar }

            var plants = (await GetPlantsImplAsync())?.ToList();

            var plant = plants?.FirstOrDefault(p => p.Id == newplant.Id);
            if (plant != null)
            {
                plant.Name = newplant.Name;
                plant.Description = newplant.Description;
                plant.Address = newplant.Address;
                plant.CreationDate = newplant.CreationDate;
            }
            else { plants?.Add(newplant); }

            var plantsString = JsonConvert.SerializeObject(plants);
            await System.IO.File.WriteAllTextAsync("Content/plants.json", plantsString);

            smartCache.Invalidate(new PlantInvalidationRule(newplant.Id)); logger.LogDebug($"smartCache.Invalidate(new PlantInvalidationRule({newplant.Id}));");

            activity?.SetOutput(plants);
            return plants;
        }
    }
}
