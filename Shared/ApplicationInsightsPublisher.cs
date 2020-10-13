using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;

namespace PingIsService.Shared
{
    public class ApplicationInsightsPublisher : IApplicationInsightsPublisher
    {
        private readonly TelemetryClient _telemetryClient;

        public ApplicationInsightsPublisher(IConfiguration configuration)
        {
            var key = configuration["ApplicationInsightsInstrumentationKey"];

            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            telemetryConfiguration.InstrumentationKey = key;
            _telemetryClient = new TelemetryClient(telemetryConfiguration);

            var entryAssembly = Assembly.GetEntryAssembly();
            var version = Version.Parse("1.0.0.0");

            if (entryAssembly != null)
            {
                version = Assembly.GetEntryAssembly()?.GetName().Version;
            }

            _telemetryClient.Context.Component.Version = version?.ToString();
            _telemetryClient.Context.Session.Id = Guid.NewGuid().ToString();
        }

        public void TrackMetric(string name, double value)
        {
            _telemetryClient?.GetMetric(name).TrackValue(value);
        }

        public void TrackException(Exception ex)
        {
            _telemetryClient?.TrackException(ex);
        }

        public void TrackEvent(string name, Dictionary<string, string> properties)
        {
            _telemetryClient?.TrackEvent(name, properties);
        }
    }
}
