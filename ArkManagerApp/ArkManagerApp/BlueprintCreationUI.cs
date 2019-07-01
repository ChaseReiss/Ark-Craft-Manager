using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArkManager.CustomType;
using ArkManager;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Threading;

namespace ArkManagerApp
{
    public struct BlueprintCreationUI
    {               
        public static async void OnTestBlueprintSelection(BlueprintSearchedArgs e)
        {
            await TestBlueprintSelection(e);
        }

        public static Task TestBlueprintSelection(BlueprintSearchedArgs e)
        {

            // Searching for a blueprint that matches the user input text
            // This comboBox is the same one in the panel but just pasted as a argument
            //var resources = Data.Blueprints.Where(x => x.BlueprintType == e.SearchComboBox.SelectedItem.ToString()).SelectMany(y => y.Resources);
                      
            CreatePanelComponents(e, Data.Blueprints[e.SearchComboBox.SelectedItem.ToString()]);
                       
            return Task.CompletedTask;
        }
        public static void CreatePanelComponents(BlueprintSearchedArgs e, Blueprint _selectedBlueprint)
        {
            // Assigning the blueprint creator the correct blueprint so that we can add their inputs to it (build it)
             

            // ATTENTION:::::::
            // Need to set all the defualt values of the user blueprint to 0, rn they are assigned the default primitve values
            Data.userBlueprint = _selectedBlueprint;


            // Creating boolean array so our program can check that when the create button is pressed what to do
            //bool[] Fields = new bool[_selectedBlueprint.Resources.Count()]; // All values are initialized to 0 or false
            //Data.FieldBools = Fields;

            // Need to clear the MainGrid of the old UI elements
            e.BlueprintCreationGrid.Children.RemoveRange(2, e.BlueprintCreationGrid.Children.Count);
            
            int topMargin = 70;

            // Creating a foreach to loop through all resources required to craft an instance of this blueprint
            foreach (var resource in _selectedBlueprint.Resources)
            {
                //var meow = resource.ResourceType.Contains<char>('_') ? spaceCharacterAdder() : resource.ResourceType;                
                
                // Creating a textbox that will show the resource type text
                e.BlueprintCreationGrid.Children.Add(new Label()
                {
                    Content = resource.ResourceType.Contains<char>('_') ? spaceCharacterAdder() : resource.ResourceType,
                    Width = 100,
                    Height = 20,
                    Margin = new Thickness(5, topMargin, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(0),
                    IsTabStop = false
                });
                
                // If the name has a _ we need to replace it with a ' ' instead
                string spaceCharacterAdder()
                {
                    if (resource.ResourceType.Contains('_'))
                    {
                        string[] s = new string[2];
                        s = resource.ResourceType.Split('_');                       
                        return s[0] + " " + s[1];
                    }
                    else
                    {
                        return resource.ResourceType;
                    }
                }

                // Creating a textbox for user input and adding it to the display panel
                e.BlueprintCreationGrid.Children.Add(new TextBox()
                {                    
                    Name =  resource.ResourceType,
                    Width = 75,
                    Height = 20,
                    Margin = new Thickness(200, topMargin, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    BorderThickness = new Thickness(1),                    
                    ToolTip = "Enter a whole number."
                });
                topMargin += 25;
                
                // Wiring up event to check user's input as the ui element loses their focus
                e.BlueprintCreationGrid.Children[e.BlueprintCreationGrid.Children.Count - 1].LostFocus += e.MainWindow.TextBox_FieldChanged;
            }

            // Creating the "Create" button 
            e.BlueprintCreationGrid.Children.Add(new Button()            {
                Content = "Create",
                Width = 55,
                Height = 20,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, topMargin, 0, 0),                
                Padding = new Thickness(0)
            });

            // Wiring up event for when the user attempts to create a new instance of a blueprint
            ((Button)e.BlueprintCreationGrid.Children[e.BlueprintCreationGrid.Children.Count - 1]).Click += e.MainWindow.CreateBlueprintButton_Clicked;
        }

        public static async void OnTestBlueprintTextBoxField(TextBoxFieldChangedArgs e)
        {
            await Task.Factory.StartNew(() =>
            {
                int userInputedQuantity = 0;
                e.TextBox.Dispatcher.Invoke(() =>
                {
                    // Trys to convert user input to a integer and saves conversion if succeeds
                    if (int.TryParse(e.TextBox.Text, out userInputedQuantity))
                    {
                        foreach (var item in Data.userBlueprint.Resources)
                        {
                            if (item.ResourceType == e.TextBox.Name.ToString())
                            {
                                // Creating a lock to keep the static data save from being mixed with other threads
                                object _lock = new object();
                                lock (_lock)
                                {

                                    item.Quantity = userInputedQuantity;

                                }
                                //MessageBox.Show(item.ResourceType + " = " + item.Quantity);
                                e.TextBox.BorderBrush = Brushes.LightSlateGray;
                                e.TextBox.ToolTip = "Enter a whole number.";
                            }
                        }
                    }
                    // Provides Error Report
                    else
                    {
                        e.TextBox.BorderBrush = Brushes.Red;
                        e.TextBox.ToolTip = "Your entry isn't a whole number.";
                    }
                });                
            });
        }

        //public static Task TestBlueprintTextBoxField(TextBoxFieldChangedArgs e)
        //{
        //    return Task.Run(() =>
        //    {
        //        int userInputedQuantity = 0;
        //        e.TextBox.Dispatcher.Invoke(() =>
        //        {
        //            // Trys to convert user input to a integer and saves conversion if succeeds
        //            if (int.TryParse(e.TextBox.Text, out userInputedQuantity))
        //            {
        //                foreach (var item in Data.userBlueprint.Resources)
        //                {
        //                    if (item.ResourceType == e.TextBox.Name.ToString())
        //                    {
        //                        // Creating a lock to keep the static data save from being mixed with other threads
        //                        object _lock = new object();
        //                        lock (_lock)
        //                        {

        //                            item.Quantity = userInputedQuantity;

        //                        }
        //                        Thread.Sleep(5000);
        //                        MessageBox.Show(item.ResourceType + " = " + item.Quantity);
        //                        e.TextBox.BorderBrush = Brushes.LightSlateGray;
        //                        e.TextBox.ToolTip = "Enter a whole number.";
        //                    }
        //                }
        //            }
        //            // Provides Error Report
        //            else
        //            {
        //                e.TextBox.BorderBrush = Brushes.Red;
        //                e.TextBox.ToolTip = "Your entry isn't a whole number.";
        //            }
        //        });                
        //    });            
        //}

        public static async void OnCreateBlueprintButtonClicked(object sender, EventArgs e)
        {
            await Task.Factory.StartNew(() =>
            {
                bool flag = false;
                MessageBox.Show("Create blueprint await is running");
                // If the user has filled each field then run this
                foreach (var resource in Data.userBlueprint.Resources)
                {
                    if (resource.Quantity != 0)
                    {
                        flag = true;
                        MessageBox.Show("assigned true, resource.Quantity == " + resource.Quantity);
                    }
                    else
                    {
                        flag = false;
                        MessageBox.Show("assigned false");
                    }
                }      
                if (!flag)
                {

                    // Otherwise a field remains unfilled and therefore the blueprint cannot be created 
                    MessageBox.Show("Unable to create the blueprint, make sure all your fields are filled properly.");
                }
            });            
        }
    }
}
