using WishesListBot.Services;

const string EXIT_COMMAND = "exit";


var tgService = new TelegramBotService("7425468261:AAF-3NF-UFMy_1Bw_jfrK_9eG3ss6eignkI");
tgService.Start();

while (true)
{
    var result = Console.ReadLine();
    if (result.ToLower() == EXIT_COMMAND)
        Environment.Exit(0);
}