using Microsoft.AspNetCore.Mvc;
using ML;
using Newtonsoft.Json.Linq;
using NuGet.Packaging.Signing;
using System.Drawing.Drawing2D;

namespace PL.Controllers
{
    public class GetAllController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        string URL_API = "https://pokeapi.co/api/v2/";
        string image = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/home/";
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Pokemon peliculas = new ML.Pokemon();
            peliculas.Pokemons = new List<object>();
            //var url = "https://raw.githubusercontent.com/PokeAPI/Sprites/master/sprites/pokemon/";
            //string url = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/home/1.png";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
                var responseTask = client.GetAsync("pokemon?limit=20");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    dynamic json = JObject.Parse(readTask.Result.ToString());

                    int i = 1;
                    foreach (var resultItem in json.results)
                    {
                        ML.Pokemon peliList = new ML.Pokemon();
                        peliList.name = resultItem.name;
                        peliList.url = resultItem.url;
                        peliList.imagen = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/home/" + IdPokemon(peliList.url) + ".png";
                        peliList.Id = IdPokemon(peliList.url);

                        peliculas.Pokemons.Add(peliList);
                        i++;

                    }
                }
            }
            return View(peliculas);
        }

        public int IdPokemon(string url)
        {
            string id = url;

            if (id.Length == 36)
            {
                return int.Parse(id.Substring(34, 1));
            }
            else
            {

                return int.Parse(id.Substring(34, 2));


            }
        }

        [HttpGet]
        public ActionResult Busqueda(int Id)
        {
            ML.Result resultTipo = new ML.Result();
            resultTipo.Objects = new List<object>();

            ML.Result resultHabilidades = new ML.Result();
            resultHabilidades.Objects = new List<object>();

            ML.Pokemon pokemon = new ML.Pokemon();
            pokemon.Pokemons = new List<object>();

            pokemon.Detalles = new ML.Detalles();
            pokemon.Detalles.Tipo = new ML.Tipo();
            pokemon.Detalles.Habilidades = new ML.Habilidad();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL_API);
                var responseTask = client.GetAsync("pokemon/" + Id);
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    dynamic json = JObject.Parse(readTask.Result.ToString());

                    pokemon.Id = json.id;
                    pokemon.name = json.name;
                    pokemon.imagen = image + pokemon.Id + ".png";

                    //Detalles
                    pokemon.Detalles.Altura = json.height;
                    pokemon.Detalles.Altura = pokemon.Detalles.Altura / 10;
                    pokemon.Detalles.Peso = json.weight;
                    pokemon.Detalles.Peso = pokemon.Detalles.Peso / 10;

                    //Tipo
                    foreach (var item in json.types)
                    {
                        ML.Pokemon pokemonitem = new ML.Pokemon();
                        pokemonitem.Detalles = new ML.Detalles();
                        pokemonitem.Detalles.Tipo = new ML.Tipo();

                        pokemonitem.Detalles.Tipo.Nombre = item.type.name;
                        pokemonitem.Detalles.Tipo.url = item.type.url;
                        pokemonitem.Detalles.Tipo.Id = GetIdTipo(pokemonitem.Detalles.Tipo.url);

                        resultTipo.Objects.Add(pokemonitem);
                    }
                    pokemon.Detalles.Tipo.Tipos = resultTipo.Objects;

                    //Habilidades
                    foreach (var item in json.stats)
                    {
                        ML.Pokemon pokemonItemStatus = new ML.Pokemon();
                        pokemonItemStatus.Detalles = new ML.Detalles();
                        pokemonItemStatus.Detalles.Habilidades = new ML.Habilidad();

                        pokemonItemStatus.Detalles.Habilidades.Name = item.stat.name;
                        pokemonItemStatus.Detalles.Habilidades.Value = item.base_stat;

                        resultHabilidades.Objects.Add(pokemonItemStatus);
                    }
                    pokemon.Detalles.Habilidades.Habilidades = resultHabilidades.Objects;
                }
            }
            return View(pokemon);
        }

        public int GetIdTipo(string URL)
        {
            string Id;
            Id = URL.Substring(31);
            Id = Id[..^1];
            return int.Parse(Id);
        }
        
    }
}


