using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace azfmaho
{
    public static class Ping
    {
        public const string DefaultVersion = "v1.0";

        [FunctionName("ping")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Azure Functions Maho Docker Image - Ping triggered");

            var version = Environment.GetEnvironmentVariable("AZFMAHO_VERSION", EnvironmentVariableTarget.Process) ?? DefaultVersion;

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? $"Pong. {version}."
                : $"Pong, {name}. {version}.";

            return new OkObjectResult(responseMessage);
        }
    }
}
