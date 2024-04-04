using Hangfire;
using Hangfire.Console;
using Hangfire.Server;

namespace Worker.Services;

public class MonitorService : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await AddJobs();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static async Task AddJobs()
    {
        // Scheduled Jobs: These are jobs that will be executed once after a specified delay.
        BackgroundJob.Schedule(() => Print("This is a scheduled job", null), TimeSpan.FromSeconds(5));

        // Enqueued Jobs: These are jobs that are added to a queue and will be executed when resources are available.
        // Queue names must be lower case.
        var jobId = BackgroundJob.Enqueue("queue-name", () => Print("This is an enqueued job", null));

        // Continue Job: This is a job that will be executed after another job completes.
        BackgroundJob.ContinueJobWith(jobId, () => Print($"Continue job executed after job with Job Id: {jobId}", null));

        // Recurring Job: These are jobs that are executed repeatedly based on a cron expression or time interval.
        RecurringJob.AddOrUpdate("Recurring Job", () => Print("This is a recurring job", null), cronExpression: Cron.MinuteInterval(1));
    }


    //Only public methods can be invoked in the background
    public static void Print(string message, PerformContext? context) => context.WriteLine(message);

    public static string MinuteInterval(int interval) => $"*/{interval} * * * *";
}
