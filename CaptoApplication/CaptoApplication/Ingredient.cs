using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaptoApplication
{

    
    public class Ingredient
    {

    [PrimaryKey, AutoIncrement, Column("ID")]
    public int ID { get; set; }
    public string Name { get; set; }
    public string Measure { get; set; }

    public string Date { get; set; }

    public string selectedItem { get; set; }

        public Ingredient(string name, string measure)
        {
            Name = name;
            Measure = measure;
            
        }

        public Ingredient(string name)
        {
            Name = name;
        }

        public Ingredient(string name, string measure, string date)
        {
            Name = name;
            Measure = measure;
            Date = date;
        }

        public Ingredient()
        {

        }
    }
}
