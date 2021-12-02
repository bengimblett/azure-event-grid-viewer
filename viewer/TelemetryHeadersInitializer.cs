using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace viewer
{
    using System.Collections.Generic;
    using System.Web;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Http;

    public class TelemetryHeadersInitializer: ITelemetryInitializer
    {
        public List<string> RequestHeaders { get; set; }
        public List<string> ResponseHeaders { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        public TelemetryHeadersInitializer(IHttpContextAccessor httpContextAccessor)
        {
            RequestHeaders = new List<string>();
            ResponseHeaders = new List<string>();

            _httpContextAccessor = httpContextAccessor;

        }
        public void Initialize(ITelemetry telemetry)
        {
            var requestTelemetry = telemetry as RequestTelemetry;
            // Is this a TrackRequest() ?
            if (requestTelemetry == null) return;

            var context = _httpContextAccessor.HttpContext;
            if (context == null) return;
            foreach (var header in context.Request.Headers)
            {
                telemetry.Context.GlobalProperties.Add($"Request-{header.Key}", header.Value);
            }
            foreach (var header in context.Response.Headers)
            {
                telemetry.Context.GlobalProperties.Add($"Response-{header.Key}", header.Value);
            }
        }
    }
}
