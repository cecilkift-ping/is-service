using Microsoft.AspNetCore.Mvc;
using PingIsService.Shared;

namespace PingIsService.Controllers
{
    [Route("api/healthCheck")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly IApplicationInsightsPublisher _applicationInsightsPublisher;

        public HealthCheckController(IApplicationInsightsPublisher applicationInsightsPublisher)
        {
            _applicationInsightsPublisher = applicationInsightsPublisher;
        }

        [HttpGet]
        public IActionResult GetHealthCheck()
        {
            _applicationInsightsPublisher.TrackMetric("IS_HealthCheck.Call", 1);
            return Content("Success");
        }
    }
}
