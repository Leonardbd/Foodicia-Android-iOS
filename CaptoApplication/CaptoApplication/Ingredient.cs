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

    public bool selectedItem { get; set; }

        public Ingredient(string name)
        {
            Name = name;
            selectedItem = false;
            
        }

        public Ingredient(string name, string date)
        {
            Name = name;
            Date = date;
            selectedItem = false;
        }

        public Ingredient()
        {

        }
    }
}
