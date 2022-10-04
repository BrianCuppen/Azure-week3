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
using Azure_week3.Models;

namespace MCT.Functions
{
    public class ToevoegenRegistraties
    {
        [FunctionName("ToevoegenRegistraties")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/registrations")] HttpRequest req,
            ILogger log)
        {
            try
            {
                // get json and convert to readable string
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic registration = JsonConvert.DeserializeObject(requestBody);

                //generate Guid id
                Guid RegistrationId = Guid.NewGuid();

                //new user
                azure_model user = new azure_model
                {
                    RegistrationId = RegistrationId,
                    LastName = registration.LastName,
                    FirstName = registration.FirstName,
                    Email = registration.Email,
                    Zipcode = registration.Zipcode,
                    Age = registration.Age,
                    IsFirstTimer = registration.IsFirstTimer
                };


                var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO registrations (RegisrtationId, LastName, FirstName, Email, Zipcode, Age, IsFirstTimer) VALUES (@RegistrationId, @LastName, @FirstName, @Email, @Zipcode, @Age, @IsFirstTimer)";
                        command.Parameters.AddWithValue("@RegistrationId", RegistrationId);
                        command.Parameters.AddWithValue("@LastName", user.LastName);
                        command.Parameters.AddWithValue("@FirstName", user.FirstName);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Zipcode", user.Zipcode);
                        command.Parameters.AddWithValue("@Age", user.Age);
                        command.Parameters.AddWithValue("@IsFirstTimer", user.IsFirstTimer);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return new OkObjectResult("registration added");
            }
            catch (System.Exception ex)
            {

                return new BadRequestObjectResult($"something went wrong {ex}");
            }
        }
    }
}
