using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ArkManagerApp
{
    public static class SearchLogistics
    {
        public static void OnTextInputReceived(object sender, KeyEventArgs e)
        {
            UpdateDropDownList((ComboBox)sender, e);          
        }

        public static void UpdateDropDownList(ComboBox comboBox, KeyEventArgs e)
        {
            // When the user enters text we need to open the text box
            comboBox.IsDropDownOpen = true;
           
            // Loop through the entire collection of Items
            for (int i = 0; i < comboBox.Items.Count; i++)
            {                
                // If the items first character doesn't match the users input then remove it from the drop down
                if (comboBox.Items[i].ToString().ElementAt(0) != e.Key.ToString().ElementAt(0))
                {
                    //MessageBox.Show("removing " + comboBox.Items.GetItemAt(i) + " Item char: " + comboBox.Items[i].ToString().ToCharArray().ElementAt(0));
                    //MessageBox.Show("Count: " + comboBox.Items.Count + " e.Key == " + e.Key);
                    comboBox.Items.RemoveAt(i);
                    
                    //comboBox.Items[i].Visibility = Visibility.Collapsed;
                    i--;
                }               
            }
        }
    }
}
