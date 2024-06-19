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

const string EXIT_COMMAND = "exit";

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("C:\\Users\\rusta\\source\\repos\\WishesListBot\\WishesListBot\\appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddBotHandlers();
        services.Configure<TelegramBotOptions>(context.Configuration.GetSection("TelegramBot"));

        services.AddDbContext<BotDbContext>(options =>
        {
            options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWishRepository, WishRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICommandService, CommandService>();

        services.AddScoped<ITelegramBotService, TelegramBotService>();
    })
    .Build();

var serviceProvider = host.Services;

var tgBotService = serviceProvider.GetRequiredService<ITelegramBotService>();

var tgController = new TelegramBotController(tgBotService);
tgController.Start();
while (true)
{
    var result = Console.ReadLine();
    if (result.ToLower() == EXIT_COMMAND)
        Environment.Exit(0);
}
