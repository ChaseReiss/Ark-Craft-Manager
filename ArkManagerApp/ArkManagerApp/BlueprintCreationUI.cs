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

namespace ArkManagerApp
{
    public struct BlueprintCreationUI
    {               
        public static async Task OnTestBlueprintSelection(BlueprintSearchedArgs e)
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
            Data.userBlueprint = _selectedBlueprint;

            // Need to clear the MainGrid of the old UI elements
            e.MainGrid.Children.RemoveRange(2, e.MainGrid.Children.Count);
            
            int topMargin = 70;

            // Creating a foreach to loop through all resources required to craft an instance of this blueprint
            foreach (var resource in _selectedBlueprint.Resources)
            {
                // Creating a textbox that will show the resource type text
                e.MainGrid.Children.Add(new Label()
                {
                    Content = resource.ResourceType,
                    Width = 55,
                    Height = 20,
                    Margin = new Thickness(5, topMargin, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(0),
                    //IsReadOnly = true,
                    IsTabStop = false
                });

                // Creating a textbox for user input and adding it to the display panel
                e.MainGrid.Children.Add(new TextBox()
                {
                    Name = resource.ResourceType,
                    Width = 75,
                    Height = 20,
                    Margin = new Thickness(70, topMargin, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    BorderThickness = new Thickness(1),                    
                    ToolTip = "Enter a whole number."
                });
                topMargin += 25;
                
                e.MainGrid.Children[e.MainGrid.Children.Count - 1].LostFocus += e.MainWindow.TextBox_FieldChanged;
            }

            // Creating the "Create" button 
            e.MainGrid.Children.Add(new Button()            {
                Content = "Create",
                Width = 55,
                Height = 20,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, topMargin, 0, 0),                
                Padding = new Thickness(0)
            });
        }

        public static async Task OnTestBlueprintTextBoxField(TextBoxFieldChangedArgs e)
        {
            await TestBlueprintTextBoxField(e);
        }

        public static Task TestBlueprintTextBoxField(TextBoxFieldChangedArgs e)
        {
            int userInputedQuantity = 0;

            // Trys to convert user input to a integer and saves conversion if succeeds
            if (int.TryParse(e.TextBox.Text, out userInputedQuantity))
            {
                foreach (var item in Data.userBlueprint.Resources)
                {
                    if (item.ResourceType == e.TextBox.Name.ToString())
                    {
                        item.Quantity = userInputedQuantity;
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

            return Task.CompletedTask;
        }
    }
}
