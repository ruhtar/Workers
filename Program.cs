using Hangfire;
using Hangfire.Console;
using Hangfire.Redis.StackExchange;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(options =>
{
    var connectionString = builder.Configuration.GetValue<string>("RedisConnection");

    if (string.IsNullOrEmpty(connectionString))
        throw new Exception("Empty Connection String from Redis");

    var redis = ConnectionMultiplexer.Connect(connectionString);

    options.UseRedisStorage(redis, options: new RedisStorageOptions() { Prefix = "HANGFIRE" });
    options.UseConsole();
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseHangfireDashboard("/hangfire");

app.Run();