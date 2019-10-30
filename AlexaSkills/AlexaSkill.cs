using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Alexa.NET;

namespace AlexaSkills
{
    public static class AlexaSkill
    {
        [FunctionName("OneDriveSkill")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string json = await req.ReadAsStringAsync();
            var skillRequest = JsonConvert.DeserializeObject<SkillRequest>(json);

            Request request = skillRequest.Request;
            SkillResponse response = null;

            if (request is LaunchRequest launchRequest)
            {
                string welcome = "Welcome to the skill!";
                string reprompt = "You can ask me to do random stuff!";
                response = ResponseBuilder.Ask(welcome, new Reprompt(reprompt));
            }
            if (request is IntentRequest intentRequest)
            {
                if (intentRequest.Intent.Name == "RollDice")
                {
                    Random random = new Random();
                    int value = random.Next(1, 6);
                    string message = $"It's a {value}!";

                    response = ResponseBuilder.Tell(message);
                }
            }

            return new OkObjectResult(response);
        }
    }
}
