using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Requests;
using WishesListBot.DAL.Repositories;
using WishesListBot.Presentation.Controllers;
using WishesListBot.Services;

const string EXIT_COMMAND = "exit";

IServiceCollection serviceCollection = new ServiceCollection();

serviceCollection.AddTransient<ITelegramBotService>(provider => 
{
    return new TelegramBotService("7425468261:AAF-3NF-UFMy_1Bw_jfrK_9eG3ss6eignkI");
});

serviceCollection.AddTransient<IUserRepository>(provider =>
{
    return new UserRepository(new WishesListBot.DAL.BotDbContext());
});

serviceCollection.AddTransient<IWishRepository>(provider =>
{
    return new WishRepository(new WishesListBot.DAL.BotDbContext());
});

serviceCollection.AddTransient<ICommandService>(provider =>
{
    var serviceProvider = serviceCollection.BuildServiceProvider();
    var userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    var wishRepository = serviceProvider.GetRequiredService<IWishRepository>();
    return new CommandService(userRepository, wishRepository);
});

var serviceProvider = serviceCollection.BuildServiceProvider();


var tgBotService = serviceProvider.GetRequiredService<ITelegramBotService>();

var tgController = new TelegramBotController(tgBotService);
tgController.Start();

while (true)
{
    var result = Console.ReadLine();
    if (result.ToLower() == EXIT_COMMAND)
        Environment.Exit(0);
}