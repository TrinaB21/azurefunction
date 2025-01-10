using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MyFunctionApp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Read 'name' parameter from the query string
            string name = req.Query["name"];

            // Read request body if 'name' is not in query
            if (string.IsNullOrEmpty(name))
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                name = data?.name;
            }

            // Return a greeting message or error if 'name' is not provided
            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}!")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
