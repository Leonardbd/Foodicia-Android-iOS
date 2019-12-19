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
    public DateTime Date { get; set; }
    
    public string Color { get; set; }
    public string Date2 { get; set; }
    public bool selectedItem { get; set; }

        public Ingredient(string name)
        {
            Name = name;
            selectedItem = false;
            
        }

        public Ingredient(string name, DateTime date)
        {
            Name = name;
            Date = date;
            selectedItem = false;
            Date2 = date.ToString("dd/MM/yyyy");

            if((date - DateTime.Today).TotalDays < 3)
            {
                Color = "Red";

            }
            else
            {
                Color = "Green";
            }
        }

        public Ingredient()
        {
            
        }
    }
}
