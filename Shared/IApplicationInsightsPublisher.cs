using System;
using System.Collections.Generic;

namespace PingIsService.Shared
{
    public interface IApplicationInsightsPublisher
    {
        void TrackMetric(string name, double value);
        void TrackException(Exception ex);
        public void TrackEvent(string name, Dictionary<string, string> properties);
    }
}