using Hangfire;
using Hangfire.Console;
using Hangfire.Server;

namespace Worker.Services;

public class MonitorService : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await AddJobHangfire();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static async Task AddJobHangfire()
    {
        //Scheduled Jobs
        BackgroundJob.Schedule(() => Print("This is a scheduled job", null), TimeSpan.FromSeconds(5));

        //Enqueued Jobs
        var jobId = BackgroundJob.Enqueue("QueueName", () => Print("This is a Enqueued job", null));

        //Continue Job
        BackgroundJob.ContinueJobWith(jobId, () => Print($"Continue job runned after Job with Job Id: {jobId}", null));

        //Recurring Job
        RecurringJob.AddOrUpdate("Recurring Job", () => Print("This is a recurring job", null), cronExpression: MinuteInterval(1));
    }

    //can this be private?
    private static void Print(string message, PerformContext? context) => context.WriteLine(message);

    public static string MinuteInterval(int interval) => $"*/{interval} * * * *";
}
