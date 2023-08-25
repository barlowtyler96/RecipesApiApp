﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeLibrary.Models;

public class RecipeIngredient
{
    public int RecipeId { get; set; }

    public int IngredientId { get; set; }

    public decimal Amount { get; set; }

    public string? Unit { get; set; }

    public string? IngredientName { get; set; }
}
