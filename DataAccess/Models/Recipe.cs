using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;

internal class Recipe
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public List<Ingredient> Ingredients { get; set; }

    public string MealType { get; set; }

    public string Instructions { get; set; }
   

    //add image property
}
