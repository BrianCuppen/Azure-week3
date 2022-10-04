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
    public class OpvragenRegistraties
    {
        [FunctionName("OpvragenRegistraties")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/registrations")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("trying to get registrations");

            try
            {
                string connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string sql = "SELECT * FROM Registrations";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                log.LogInformation(reader.GetString(0));
                                return new OkObjectResult(reader.GetString(0));
                            }
                        }
                    }
                    return new OkObjectResult("no registrations found");
                }
            }
            catch (System.Exception ex)
            {

                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
