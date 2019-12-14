using System;
using System.Collections.Generic;
using System.Text;

namespace CaptoApplication
{
    public class TestItem
    {

        public int ID { get; set; }

        public string Namn { get; set; }


        public TestItem(int id, string namn)
        {
            Namn = namn;
            ID = id;
        }

        public TestItem()
        {

        }
    }
}
