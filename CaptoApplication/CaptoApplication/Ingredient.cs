using System;
using System.Collections.Generic;
using System.Text;

namespace CaptoApplication
{

    
    public class Ingredient
    {
        
    public string Name { get; set; }
    public int Weight { get; set; }

        public Ingredient(string name, int weight)
        {
            Name = name;
            Weight = weight;
        }
    }
}
