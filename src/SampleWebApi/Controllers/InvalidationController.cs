using Asp.Versioning;
using Diginsight.CAOptions;
using Diginsight.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace SampleWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "common")]
    public class InvalitationController : ControllerBase
    {
        private readonly ILogger<InvalitationController> logger;
        private readonly IClassAwareOptionsMonitor<FeatureFlagOptions> featureFlagsOptionsMonitor;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public InvalitationController(
            ILogger<InvalitationController> logger,
            IClassAwareOptionsMonitor<FeatureFlagOptions> featureFlagsOptionsMonitor)
        {
            this.logger = logger;
            this.featureFlagsOptionsMonitor = featureFlagsOptionsMonitor;
        }

        [HttpGet("getplants", Name = nameof(InvalidateAsync))]
        [ApiVersion(ApiVersions.V_2024_04_26.Name)]
        public IEnumerable<Plant> InvalidateAsync()
        {
            using var activity = Program.ActivitySource.StartMethodActivity(logger); // , new { foo, bar }

            var result = default(IEnumerable<Plant>);

            Thread.Sleep(1000);

            activity?.SetOutput(result);
            return result;
        }
    }
}
