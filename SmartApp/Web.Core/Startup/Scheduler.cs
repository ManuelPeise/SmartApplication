using Quartz.Impl;
using Quartz;
using System.Net;
using System.Security.Claims;
using Logic.Shared;
using Microsoft.Extensions.Options;
using Shared.Models.Identity;
using System.Net.Http.Headers;

namespace Web.Core.Startup
{
    public static class Scheduler
    {
        //private static string EmailClassificationTrainingDataCollector = "email-classification-training-data-collector";
        private static string UserActivationTask = "user-activation-task";

        public static void ExecuteScheduler(string baseUrl, SecurityData securityData)
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

#if !DEBUG
            AddJob(scheduler,
               GetJobDetails(
                   UserActivationTask,
                   new Dictionary<string, object>
                   {
                        { "Url", $"{baseUrl}api/UserActivation/ActivateUsers"},
                        { "SecurityData", securityData}
                   }),
               GetTrigger(
                   $"{UserActivationTask}-trigger",
                   1,
                   "* 15 * * * ?"));
#endif
          


            scheduler.Start();
        }


        private static void AddJob(IScheduler scheduler, IJobDetail detail, ITrigger trigger)
        {
            scheduler.ScheduleJob(detail, trigger);
        }

        private static IJobDetail GetJobDetails(string name, IDictionary<string, object> parameters)
        {
            return JobBuilder.Create<WebJob>()
                .WithIdentity(name)
                .UsingJobData(new JobDataMap(parameters))
                .Build();
        }

        private static ITrigger GetTrigger(string name, int intervallHours, string cronExpression)
        {
            return TriggerBuilder.Create()
                .WithIdentity(name)
                //.WithSimpleSchedule(x => x.WithIntervalInMinutes(1))
                .WithCronSchedule(cronExpression)
                .StartNow()
                .Build();


        }

    }

    public class WebJob : IJob
    {
        public SecurityData SecurityData { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Params { get; set; } = string.Empty;

        public async Task Execute(IJobExecutionContext context)
        {
            var jtw = GetMaintananceUserToken(SecurityData);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", $"{jtw}");

                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(Url + Params),
                    Version = HttpVersion.Version11
                };

                await client.SendAsync(requestMessage);
            }
        }

        private string GetMaintananceUserToken(SecurityData securityData)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "MaintananceUser"),
                new Claim("userRole", "MaintananceUser")
            };

            var tokenGenerator = new JwtTokenGenerator(securityData);

            return tokenGenerator.GenerateJwtToken(claims, 1);
        }
    }
}
