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
        public async Task<Plant> GetPlantByIdImplAsync([FromQuery] Guid id)
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

            var plants = await smartCache.GetAsync(
                new MethodCallCacheKey(cacheKeyService, typeof(PlantsController), nameof(GetPlantsAsync)),
                _ => GetPlantsImplAsync(),
                new SmartCacheOperationOptions() { MaxAge = TimeSpan.FromMinutes(10) }
            );

            activity?.SetOutput(plants);
            return plants;
        }

        [HttpGet("getplantbyid", Name = nameof(GetPlantByIdAsync))]
        [ApiVersion(ApiVersions.V_2024_04_26.Name)]
        public async Task<Plant> GetPlantByIdAsync(Guid plantId)
        {
            using var activity = Program.ActivitySource.StartMethodActivity(logger);

            var plant = await smartCache.GetAsync(
                new GetPlantByIdCacheKey(plantId),
                _ => GetPlantByIdImplAsync(plantId),
                new SmartCacheOperationOptions() { MaxAge = TimeSpan.FromMinutes(10) }
            );

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
            }
            else { plants?.Add(newplant); }



            // Save plants to content file /Content/plants.json
            var plantsString = JsonConvert.SerializeObject(plants);
            await System.IO.File.WriteAllTextAsync("Content/plants.json", plantsString);

            activity?.SetOutput(plants);
            return plants;
        }


        public record PlantInvalidationRule(string PlantId, string PlantType) : IInvalidationRule;
        [SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Local")]
        private sealed record GetPlantByIdCacheKey(string PlantId) : IInvalidatable
        {
            public bool IsInvalidatedBy(IInvalidationRule invalidationRule, out Func<Task> ic)
            {
                ic = null;
                return invalidationRule is PlantInvalidationRule pir
                        && pir.PlantId == PlantId;
                        //&& pir.PlantType == PlantType;
            }
        }

    }
}
