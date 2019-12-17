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
    public string Date { get; set; }

    public string selectedItem { get; set; }

        public Ingredient(string name)
        {
            Name = name;
            
        }

        public Ingredient(string name, string date)
        {
            Name = name;
            Date = date;
        }

        public Ingredient()
        {

        }
    }
}
