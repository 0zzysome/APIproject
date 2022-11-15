using RestSharp;
using System.Text.Json;
using System.Net;
RestClient pokeClient = new("https://pokeapi.co/api/v2/");

RestRequest request = new("pokemon/416");

RestResponse response = pokeClient.GetAsync(request).Result;


if (response.StatusCode == HttpStatusCode.OK)
{
    Pokemon p = JsonSerializer.Deserialize<Pokemon>(response.Content);

    System.Console.WriteLine(p.Name);
    System.Console.WriteLine(p.Weight);

}
else
{
    System.Console.WriteLine("not found");
}

Console.ReadLine();
