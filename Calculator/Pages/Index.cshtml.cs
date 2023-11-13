using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using FeatureHubSDK;
using IO.FeatureHub.SSE.Model;

namespace Calculator.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
       

        public void OnGet()
        {
            
            try
            {
                RestClient dbtest = new RestClient("http://calchistory-service/");
                var taskdb = dbtest.GetAsync<List<string>>(new RestRequest($"/CalcHistoryService"));
               
                
                foreach (var task in taskdb.Result)
                {
                    Console.WriteLine(task.ToString());
                    ViewData["History"] += task.ToString();
                    ViewData["History"] +=  "<br />";
                    ViewData["History"] +=  Environment.NewLine; 
                }  

                 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cant get data now");
                ViewData["History"] = "Cant get data now";

            } 

        }
        public async void OnPost()
        {

            var aaa = Request.Form["AAA"]; 
            var bbb = Request.Form["BBB"];
            var action = Request.Form["action"];

            if (action == "AddService")
            {
                try
                {
                    // ADD
                    RestClient restClient = new RestClient("http://add-service/");
                    var task = restClient.PostAsync<long>(new RestRequest($"/addservice?a={aaa}&b={bbb}"));

                    Console.WriteLine(task.Result);
                    ViewData["Result"] = task.Result;
                }
                catch (Exception ex)
                {
                    ViewData["Result"] = "No service currenty";
                    Console.WriteLine("No service currenty");
                }
            }
            else if (action == "SubService")
            {
                try
                {
                    // SUB
                    RestClient restClient = new RestClient("http://sub-service/");
                    var task = restClient.PostAsync<long>(new RestRequest($"/subservice?a={aaa}&b={bbb}"));

                    Console.WriteLine(task.Result);
                    ViewData["Result"] = task.Result;
                }catch(Exception ex)
                {
                    ViewData["Result"] = "No service currenty";
                    Console.WriteLine("No service currenty");
                }

            }
            else if (action == "MultiplyService")
            { 

                FeatureLogging.DebugLogger += (sender, s) => Console.WriteLine("DEBUG: " + s);
                FeatureLogging.TraceLogger += (sender, s) => Console.WriteLine("TRACE: " + s);
                FeatureLogging.InfoLogger += (sender, s) => Console.WriteLine("INFO: " + s);
                FeatureLogging.ErrorLogger += (sender, s) => Console.WriteLine("ERROR: " + s);

                var config = new EdgeFeatureHubConfig("http://featurehub:8085", "739cf51a-85be-4118-a4ef-b102554114a5/6v5TQDPNEHFlMCUoW7MyQ6Z3pMrfC09R7ElPu9s5");
                var fh = await config.NewContext().Build();

                try
                {
                    await Task.Run(() =>
                    {

                    if (fh["MultiplyFeature"].IsEnabled)
                    {
                        
                            // Multiply
                            RestClient restClient = new RestClient("http://multiply-service/");
                            var task = restClient.PostAsync<long>(new RestRequest($"/multiplyservice?a={aaa}&b={bbb}"));

                            Console.WriteLine(task.Result);
                            ViewData["Result"] = task.Result;
                       
                    }
                    else
                    {
                        ViewData["Result"] = "Service Disabled";
                        Console.WriteLine("Service Disabled");
                    }
                    });
                }
                catch (Exception ex)
                {
                ViewData["Result"] = "No service currenty";
                Console.WriteLine("No service currenty");
                }



            }
        }
    }
}