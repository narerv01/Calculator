using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        }
    }
}