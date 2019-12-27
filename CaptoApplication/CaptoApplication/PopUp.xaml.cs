using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CaptoApplication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUp : PopupPage
    {
        public Ingredient Ingredient { get; set; }
        public string EAN { get; set; }
        public PopUp()
        {
            InitializeComponent();
        }
        public PopUp(string ean)
        {
            InitializeComponent();
            EAN = ean;
            productEntry.Text = EAN;
        }

        public PopUp(string placeHolder, int w)
        {
            InitializeComponent();
            changePlaceholder(placeHolder);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            OnDialogClosed?.Invoke(this, new DialogResult { ProductName = productEntry.Text,  ExpirationDate = datePicker.Date });
            App.Current.MainPage.Navigation.PopPopupAsync(true);
           
        }

        public EventHandler<DialogResult> OnDialogClosed;

        public void changePlaceholder(string text)
        {
            productEntry.Placeholder = text;
        }
        
    }

        public class DialogResult
        {
            public string ProductName { get; set; }
            public string Measurement { get; set; }
            
            public DateTime ExpirationDate { get; set; }

        }
}