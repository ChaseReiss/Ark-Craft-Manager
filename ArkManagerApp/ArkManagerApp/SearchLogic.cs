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
    public static class SearchLogic
    {
        // Contains the items that were removed from the comboBox dropdown
        private static List<string> removedItems = new List<string>();
        // Contains the number of character that were inside the searchbox the last time it was used
        private static int LastSearchStringLength = 0;

        /// <summary>
        ///     Changes the event params to be more friendly to work with and calls a decision making method
        /// </summary>
        /// <param name="sender"> the comboBox this was sent from </param>
        /// <param name="e"> Event args of the keystokes </param>
        public static void OnTextInputReceived(object sender, KeyEventArgs e)
        {
            UpdateDropDownList((ComboBox)sender, e);          
        }

        /// <summary>
        ///     Calls appropriate methods according to the user's input
        /// </summary>  
        public static void UpdateDropDownList(ComboBox comboBox, KeyEventArgs e)
        {            
            // Narrow the search if the user added another char to the searchbar
            if (comboBox.Text.Length > LastSearchStringLength)               
                NarrowTheSearch(comboBox, e);           
            // Widen the search if the user removed a char from the searchbar
            else           
                WidenTheSearch(comboBox, e);
                        
            // Finishes up any opertations needed
            FinishUpdatingSearch(comboBox, e);
        }
        

        /* ------------------------------ Methods Change GUI ---------------------------------- */


        /// <summary>
        ///     Removes items from the comboBox.Items collection
        /// </summary>
        private static void NarrowTheSearch(ComboBox comboBox, KeyEventArgs e)
        {
            // Loop through the entire collection of Items
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                // If the items first character doesn't match the users input then remove it from the drop down
                if (comboBox.Items[i].ToString().ElementAt(LastSearchStringLength) != e.Key.ToString().ElementAt(0))
                {

                    // Adding the string to list of removed strings
                    removedItems.Add(comboBox.Items[i].ToString());
                    // Removing the string from the comboBox items
                    comboBox.Items.RemoveAt(i);

                    // If an item is deleted, we need to go back one to compensate for the deleted item and the collection concatenating   
                    i--;
                }
            }
        }

        /// <summary>
        ///     Adds items back to the comboBox.Items Collection
        /// </summary>
        private static void WidenTheSearch(ComboBox comboBox, KeyEventArgs e)
        {
            // Adding back items to the comboBox item list
            for (int i = 0; i < removedItems.Count; i++)
            {
                comboBox.Items.Add(removedItems[i]);
            }

            // Clearing the list to prevent duplication
            removedItems.Clear();
        }

        /// <summary>
        ///     Completes all last needed operations before finishing
        /// </summary>
        private static void FinishUpdatingSearch(ComboBox comboBox, KeyEventArgs e)
        {
            // Recording the text string length
            LastSearchStringLength = comboBox.Text.Length;            
        }

        /* Focus Changed */

        public static void GotFocus(object sender, RoutedEventArgs e)
        {
            ((ComboBox)sender).IsDropDownOpen = true;
        }
    }
}
