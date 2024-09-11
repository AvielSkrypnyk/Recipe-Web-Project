using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private const string FilePath = "wwwroot/data/recipes.json";

        [HttpGet]
        public async Task<ActionResult<List<Recipe>>> Get()
        {
            if (!System.IO.File.Exists(FilePath))
            {
                return new List<Recipe>();
            }

            using var stream = System.IO.File.OpenRead(FilePath);
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            var recipes = JsonSerializer.Deserialize<List<Recipe>>(json);
            return recipes;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> Get(int id)
        {
            if (!System.IO.File.Exists(FilePath))
            {
                return NotFound();
            }

            var json = await System.IO.File.ReadAllTextAsync(FilePath);
            var recipes = JsonSerializer.Deserialize<List<Recipe>>(json);

            var recipe = recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        [HttpPost]
        public async Task<ActionResult<Recipe>> Post([FromBody] Recipe recipe)
        {
            // Simulate loading delay
            await Task.Delay(5000);

            if (recipe == null || string.IsNullOrWhiteSpace(recipe.Title))
            {
                return BadRequest("Invalid recipe data");
            }

            var recipes = new List<Recipe>();
            if (System.IO.File.Exists(FilePath))
            {
                var json = await System.IO.File.ReadAllTextAsync(FilePath);
                recipes = JsonSerializer.Deserialize<List<Recipe>>(json) ?? new List<Recipe>();
            }

            if (recipes.Any(r => r.Title.Equals(recipe.Title, System.StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest("This recipe already exists");
            }

            // Assign a unique ID
            int newId = recipes.Count > 0 ? recipes.Max(r => r.Id) + 1 : 1;

            var newRecipe = new Recipe
            {
                Id = newId,
                Title = recipe.Title,
                Ingredients = recipe.Ingredients,
                Instructions = recipe.Instructions
                // PhotoUrl = recipe.PhotoUrl // Uncomment for full functionality if needed
            };

            recipes.Add(newRecipe);

            var updatedJson = JsonSerializer.Serialize(recipes, new JsonSerializerOptions { WriteIndented = true });
            await System.IO.File.WriteAllTextAsync(FilePath, updatedJson);

            return Ok(newRecipe);
        }
    }

    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        // public string PhotoUrl { get; set; } // Commented out for now
    }
}