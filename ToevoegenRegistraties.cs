using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace MCT.Functions
{
    public class ToevoegenRegistraties
    {
        [FunctionName("ToevoegenRegistraties")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/registrations")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Trying to add registration");
            // this is the body of the request
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //convert to json
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            return new OkObjectResult(data);
        }

    }
}
