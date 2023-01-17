using RestSharp;
using System.Text.Json;
using System.Net;

GameInstance Game = new GameInstance();

Game.ShowStartMenu();
if(Game.GameType==1)
{
    Game.StartNormalQuiz();
}
else if(Game.GameType==2)
{
    Game.StartHardQuiz();
}
else
{
    System.Console.WriteLine("ERROR: for some reason no choice of game type was made!");
}

