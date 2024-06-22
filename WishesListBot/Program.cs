using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using PRTelegramBot.Attributes;
using PRTelegramBot.Configs;
using PRTelegramBot.Core;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models.EventsArgs;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using WishesListBot.DAL;
using WishesListBot.DAL.Repositories;
using WishesListBot.Domain;
using WishesListBot.Presentation.Controllers;
using WishesListBot.Services;
using WishesListBot.Services.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

const string EXIT_COMMAND = "exit";

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile(".\\appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {

        services.AddBotHandlers();
        services.Configure<TelegramBotOptions>(context.Configuration.GetSection("TelegramBot"));

        services.AddDbContext<BotDbContext>(options =>
        {
            options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection"));
        }, ServiceLifetime.Singleton);

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWishRepository, WishRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICommandService, CommandService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddMemoryCache();
    })
    .Build();

var serviceProvider = host.Services;
var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();
var authorizationFilter = new AuthorizationFilter(authorizationService);

var telegram = new PRBotBuilder("7210697457:AAGfIaWrTejyz5VMDeWMvHKt45zofO5Leec")
                    .SetClearUpdatesOnStart(true)
                    .SetServiceProvider(serviceProvider)
                    .Build();

telegram.Events.OnErrorLog += Events_OnErrorLog;
telegram.Events.OnCommonLog += Events_OnCommonLog;

telegram.Events.OnPostMessageUpdate += async (args) => await Events_Authorize(args);

async Task Events_Authorize(BotEventArgs args)
{
    var commandService = serviceProvider.GetRequiredService<ICommandService>();
    var methods = typeof(CommandService).GetMethods()
        .Where(m => m.GetCustomAttributes(typeof(ReplyMenuHandlerAttribute), true).Any());

    foreach (var method in methods)
    {
        var replyMenuHandlerAttribute = method.GetCustomAttribute<ReplyMenuHandlerAttribute>();
        if (replyMenuHandlerAttribute != null && args.Update.Message.Text == replyMenuHandlerAttribute.Commands.FirstOrDefault())
        {
            var isAuthorized = await authorizationFilter.ExecuteWithAuthorizationCheck(commandService, method, args.BotClient, args.Update, new object[] { args.BotClient, args.Update });
            if (!isAuthorized)
            {
                return;
            }
            break;
        }
    }
}


telegram.Start();


async Task Events_OnCommonLog(CommonLogEventArgs arg)
{
    Console.WriteLine(arg.Message);
}

async Task Events_OnErrorLog(ErrorLogEventArgs arg)
{
    Console.WriteLine(arg.Exception);
}

while (true)
{
    var result = Console.ReadLine();
    if (result.ToLower() == EXIT_COMMAND)
        Environment.Exit(0);
}
