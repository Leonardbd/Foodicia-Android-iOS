using System;
using System.Collections.Generic;
using System.Text;

namespace CaptoApplication
{

    
    public class Ingredient
    {
        
    public string Name { get; set; }
    public string Measure { get; set; }

    public string Date { get; set; }

        public Ingredient(string name, string measure)
        {
            Name = name;
            Measure = measure;
        }
    }
}
