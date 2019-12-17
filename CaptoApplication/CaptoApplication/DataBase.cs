using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SQLite;

namespace CaptoApplication
{
    public class DataBase
    {
        public string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public bool createDataBase()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "DemoDB.db")))
                    {
                    connection.CreateTable<Ingredient>();
                    return true;
                    }
            }

            catch(SQLiteException e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            
        }

        public bool InsertIntoTable(Ingredient item)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "DemoDB.db")))
                {
                    connection.Insert(item);
                    return true;
                }
            }

            catch (SQLiteException e)
            {
                
                return false;
            }
        }

        public List<Ingredient> GetIngredientsItems()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "DemoDB.db")))
                {
                   return connection.Table<Ingredient>().ToList();
                    
                    
                }
            }

            catch (SQLiteException e)
            {
                return null;
            }
        }
    }
}
