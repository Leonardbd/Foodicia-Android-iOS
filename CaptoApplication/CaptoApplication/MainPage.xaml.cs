using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;

namespace CaptoApplication
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        public DataBase db { get; set; }
        public List<TestItem> testItems { get; set; }
        public MainPage()
        {
            InitializeComponent();
            db = new DataBase();
            testItems = new List<TestItem>();

            db.createDataBase();
            testItems = db.GetTestItems();

            TestEntry.Text = testItems[1].Namn;



        }

        private void btn_Clicked(object sender, EventArgs e)
        {
            
            db.InsertIntoTable(new TestItem(1, TestEntry.Text));
            
        }
    }
}
