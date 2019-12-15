using System;
using System.Collections.Generic;
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
                    connection.CreateTable<TestItem>();
                    return true;
                    }
            }

            catch(SQLiteException e)
            {
                return false;
            }
            
        }

        public bool InsertIntoTable(TestItem item)
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

        public List<TestItem> GetTestItems()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "DemoDB.db")))
                {
                   return connection.Table<TestItem>().ToList();
                    
                    
                }
            }

            catch (SQLiteException e)
            {
                return null;
            }
        }
    }
}
