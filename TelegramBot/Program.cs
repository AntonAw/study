using TelegramBot;

var app = await WebApplication.CreateBuilder(args)
                              .AddDomain()
                              .AddApplication()
                              .AddInfrastructure()
                              .AddPresentation()
                              .Build()
                              .InitializeDbContextAsync();

await app.AddMiddlewares()
         .RunAsync();