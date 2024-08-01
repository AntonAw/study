using Application.Helper;
using Application.Services;
using Application.Services.Abstarctions;
using Domain.Domain.Abstractions;
using Infrastructure.SendNewReports;
using Infrastructure.SendNewReports.Refit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Persistence;
using Persistence.Interfaces;
using Persistence.Interfaces.Repositories;
using Persistence.Seeder;
using Quartz;
using Refit;
using Telegram.Bot;
using TelegramBot.Services;
using TelegramBot.Services.Abstraction;

namespace TelegramBot;

public static class ProgramExtenstion
{
    public static WebApplicationBuilder AddDomain(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IThemeRepository, ThemeRepository>();
        builder.Services.AddScoped<IReportRepository, ReportRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        return builder;
    }

    public static WebApplicationBuilder AddApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient("telegram_bot_client")
            .AddTypedClient<ITelegramBotClient>((httpClient) =>
            {
                TelegramBotClientOptions options = new(builder.Configuration["TelegramBotToken"]!); 
                return new TelegramBotClient(options, httpClient);
            });

        builder.Services.AddScoped<IThemeService, ThemeService>();
        builder.Services.AddScoped<IUploadHelpService, UploadHelpService>();
        builder.Services.AddScoped<IReplyService, ReplyService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IReportingHelpService, ReportingHelpService>();
        builder.Services.AddScoped<ISalaryHelpService, SalaryHelpService>();
        builder.Services.AddScoped<IEstimateHelpService, EstimateHelpService>();

        builder.Services.AddRefitClient<IReportSenderRefit>();

        InitializeHelper(builder);

        return builder;

        static void InitializeHelper(WebApplicationBuilder builder)
        {
            var section = builder.Configuration.GetSection("Aes");
            AesHelper.Initialize(section.GetValue<string>("Key")!,
                                 section.GetValue<string>("IV")!);
        }
    }

    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<IDatabaseContext, DatabaseContext>(options => 
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddQuartz(cfg =>
        {
            var key = new JobKey(nameof(ReportSender));

            cfg.AddJob<ReportSender>(key)
            .AddTrigger(trigger => trigger.ForJob(key)
            .WithSimpleSchedule(schedule => schedule.WithInterval(TimeSpan.FromMinutes(10)).RepeatForever()));
        });

        builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        return builder;
    }

    public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUpdateHandler, UpdateHandler>();
        builder.Services.AddHostedService<WebhookService>();

        builder.Services.AddControllers()
            .AddNewtonsoftJson();
        //builder.Services.AddProblemDetails();

        return builder;
    }

    public static async Task<WebApplication> InitializeDbContextAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
        if (context.Database.EnsureCreated())
        {
            await Seeder.SeedAsync(context);
        }

        return app;
    }

    public static WebApplication AddMiddlewares(this WebApplication app)
    {
        app.UseHsts()
           .UseRouting();

        app.MapControllers();
        return app;
    }
}