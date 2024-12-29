using Quartz.Impl;
using Quartz;
using System.Net;

namespace Web.Core.Startup
{
    public static class Scheduler
    {
        private static string EmailClassificationTrainingDataCollector = "email-classification-training-data-collector";

        public static void ExecuteScheduler(string baseUrl)
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

            AddJob(scheduler,
                GetJobDetails(
                    EmailClassificationTrainingDataCollector,
                    new Dictionary<string, object>
                    {
                        { "Url", $"{baseUrl}api/EmailClassification/CollectTrainingData"}
                    }),
                GetTrigger(
                    $"{EmailClassificationTrainingDataCollector}-trigger",
                    24,
                    "5 6 * * * ?"));


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
# if DEBUG
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(5))
#else
                .WithCronSchedule(cronExpression)
#endif
                .StartNow()
                .Build();


        }

    }

    public class WebJob : IJob
    {
        public string Url { get; set; } = string.Empty;
        public string Params { get; set; } = string.Empty;

        public async Task Execute(IJobExecutionContext context)
        {
            using (var client = new HttpClient())
            {
                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(Url + Params),
                    Version = HttpVersion.Version11
                };

                await client.SendAsync(requestMessage);
            }
        }
    }
}
