using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecipeApp
{
    public partial class MainWindow : Window
    {
        private List<Recipe> recipes;
        private List<Recipe> filteredRecipes;

        public MainWindow()
        {
            InitializeComponent();
            recipes = new List<Recipe>();
            filteredRecipes = new List<Recipe>();
        }

        private void AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            Recipe recipe = new Recipe();

            recipe.Name = RecipeNameTextBox.Text;

            // Parse ingredients from text box
            recipe.Ingredients = IngredientsTextBox.Text.Split('\n')
                .Select(line =>
                {
                    string[] parts = line.Split(';');
                    if (parts.Length == 5)
                    {
                        return new Ingredient
                        {
                            Name = parts[0].Trim(),
                            Quantity = double.Parse(parts[1].Trim()),
                            Unit = parts[2].Trim(),
                            Calories = int.Parse(parts[3].Trim()),
                            FoodGroup = parts[4].Trim()
                        };
                    }
                    return null;
                })
                .Where(ingredient => ingredient != null)
                .ToList();

            // Parse steps from text box
            recipe.Steps = StepsTextBox.Text.Split('\n')
                .Select(description => new Step { Description = description.Trim() })
                .ToList();

            recipes.Add(recipe);
            RecipesListBox.Items.Add(recipe.Name);
        }

        private void RecipesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecipesListBox.SelectedItem != null)
            {
                string selectedRecipeName = RecipesListBox.SelectedItem.ToString();
                var selectedRecipe = recipes.FirstOrDefault(r => r.Name.Equals(selectedRecipeName, StringComparison.OrdinalIgnoreCase));

                if (selectedRecipe != null)
                {
                    selectedRecipe.DisplayRecipe();
                }
            }
        }
    }

    class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public int Calories { get; set; }
        public string FoodGroup { get; set; }
    }

    class Step
    {
        public string Description { get; set; }
    }

    class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Step> Steps { get; set; }

        public int TotalCalories => Ingredients.Sum(i => i.Calories);

        public void DisplayRecipe()
        {
            string recipeInfo = $"Recipe: {Name}\nIngredients:\n";
            foreach (var ingredient in Ingredients)
            {
                recipeInfo += $"{ingredient.Name} - {ingredient.Quantity} {ingredient.Unit}\n";
            }
            recipeInfo += "Steps:\n";
            foreach (var step in Steps)
            {
                recipeInfo += step.Description + "\n";
            }
            recipeInfo += $"Total Calories: {TotalCalories}\n";
            if (TotalCalories > 300)
            {
                recipeInfo += "WARNING: This recipe exceeds 300 calories!";
            }
            MessageBox.Show(recipeInfo, "Recipe Information");
        }
    }
}

