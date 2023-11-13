using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Monitoring;
using Serilog;
using System.Diagnostics;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry;

namespace MultiplyService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MultiplyServiceController : ControllerBase
    {
        public MultiplyServiceController()
        {

        }

        [HttpGet]
        public void Get()
        {

        }

        [HttpPost]
        public long Post([FromQuery] long a, [FromQuery] long b)
        {
            using (var activity = MonitorService.ActivitySource.StartActivity())
            {
                MonitorService.Log.Debug("entered Multiply method");
                MonitorService.Log.Error("HAs ther ebeen an errro?");



                long res = a * b;
                long operat = 3;
                try
                {
                    RestClient rclient = new RestClient("http://calchistory-service/");

                    var request = new ServiceBRequest();
                    var activityContext = activity?.Context ?? Activity.Current?.Context ?? default;
                    var propagationContext = new PropagationContext(activityContext, Baggage.Current);
                    var propagator = new TraceContextPropagator();
                    propagator.Inject(propagationContext, request, (r, key, value) =>
                    {
                        r.Header.Add(key, value);
                    });
                    
                    _ = rclient.PostAsync<long>(new RestRequest($"/CalcHistoryService?a={a}&b={b}&res={res}&op={operat}"));



                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Log.CloseAndFlush();
                return res;
            }

        }

    }
}