using RestSharp;
using System.Text.Json;
using System.Net;

GameInstance game = new GameInstance();

game.ShowStartMenu();
if(game.gameType==1)
{
    game.StartNormalQuiz();
}
else if(game.gameType==2)
{
    game.StartHardQuiz();
}
else
{
    System.Console.WriteLine("ERROR: for some reason no choice of game type was made!");
}

