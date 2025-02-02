using Quartz.Impl;
using Quartz;
using System.Net;
using System.Security.Claims;
using Logic.Shared;
using Shared.Models.Identity;
using System.Net.Http.Headers;

namespace Web.Core.Startup
{
    public static class Scheduler
    {
        //private static string EmailClassificationTrainingDataCollector = "email-classification-training-data-collector";
        private static string UserActivationTask = "user-activation-task";
        private static string EmailDataImportTask = "email-data-import-task";

        public static void ExecuteScheduler(string baseUrl, SecurityData securityData)
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

            // task to import email data for all active email cleaner instances runs all 60 minutes
            AddJob(scheduler,
                GetJobDetails(EmailDataImportTask,
                    new Dictionary<string, object>
                    {
                        { "Url", $"{baseUrl}api/EmailDataImport/ImportEmailData"},
                        { "SecurityData", securityData}
                    }),
                GetCronTrigger(
                    $"{EmailDataImportTask}-trigger",
                    "1 0/1 * * * ?"));
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
                   "* 1 * * * ?"));
   
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

        private static ITrigger GetTrigger(string name, int intervallMinutes)
        {
            return TriggerBuilder.Create()
                .WithIdentity(name)
                .StartNow()
                .WithSchedule(SimpleScheduleBuilder.RepeatMinutelyForever(intervallMinutes))
                .Build();


        }

        private static ITrigger GetCronTrigger(string name, string cronSchedule)
        {
            return TriggerBuilder.Create()
               .WithIdentity(name)
               .StartNow()
               .WithCronSchedule(cronSchedule)
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
