using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Monitoring;
using Serilog;
using System.Diagnostics;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry;

namespace AddService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddServiceController : ControllerBase
    { 

        public AddServiceController()
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
                MonitorService.Log.Debug("entered add method");
                MonitorService.Log.Error("HAs ther ebeen an errro?");



                long res = a + b;
                long operat = 2;
                try
                {
                    RestClient dbtest = new RestClient("http://calchistory-service/");

                    var request = new ServiceBRequest();
                    var activityContext = activity?.Context ?? Activity.Current?.Context ?? default;
                    var propagationContext = new PropagationContext(activityContext, Baggage.Current);
                    var propagator = new TraceContextPropagator();
                    propagator.Inject(propagationContext, request, (r, key, value) =>
                    {
                        r.Header.Add(key, value);
                    });

                    _ = dbtest.PostAsync<long>(new RestRequest($"/CalcHistoryService?a={a}&b={b}&res={res}&op={operat}"));



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