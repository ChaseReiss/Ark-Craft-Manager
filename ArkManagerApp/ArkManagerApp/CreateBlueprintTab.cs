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
using System.IO;
using Newtonsoft.Json;

namespace ArkManagerApp
{
    public static class CreateBlueprintGUI
    {
        private static int REMOVE_OUTDATED_UI_ELEMENTS_INDEX = 2;

        public static void OnTestBlueprintSelection(BlueprintSearchedArgs e)
        {
            TestBlueprintSelection(e);
        }
        
        public static void TestBlueprintSelection(BlueprintSearchedArgs e)
        {
            // Check if the comboBox has a selected item
            if (e.SearchComboBox.SelectedItem != null)
            {
                Blueprint bp = Data.DefaultBlueprints.Where(x => x.BlueprintType == e.SearchComboBox.SelectedItem.ToString()).First();
                CreatePanelComponents(e, bp);
            }                     
        }
        public static void CreatePanelComponents(BlueprintSearchedArgs e, Blueprint _selectedBlueprint)
        {
            // Assigning the choosen blueprint to a variable to build off of
            Data.userBlueprint = _selectedBlueprint;

            // Need to clear the MainGrid of the old UI elements
            e.BlueprintCreationGrid.Children.RemoveRange(REMOVE_OUTDATED_UI_ELEMENTS_INDEX, e.BlueprintCreationGrid.Children.Count);

            int topMargin = 70;

            // Creating a foreach to loop through all resources required to craft an instance of this blueprint
            foreach (var resource in _selectedBlueprint.Resources)
            {
                //var meow = resource.ResourceType.Contains<char>('_') ? spaceCharacterAdder() : resource.ResourceType;                

                // Creating a textbox that will show the resource type text
                e.BlueprintCreationGrid.Children.Add(new Label()
                {
                    // If the resource's name contains a space, we need to replace it to a '_' <- contructor for Label is picky
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
                    string[] s = new string[2];
                    s = resource.ResourceType.Split('_');
                    return s[0] + " " + s[1];                    
                }

                // Creating a textbox for user input and adding it to the display panel
                e.BlueprintCreationGrid.Children.Add(new TextBox()
                {
                    Name = resource.ResourceType,
                    Width = 75,
                    Height = 20,
                    Margin = new Thickness(200, topMargin, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    BorderThickness = new Thickness(1),
                    ToolTip = "Enter a whole number."
                });
                topMargin += 25;
            }

            // Creating the "Create" button 
            e.BlueprintCreationGrid.Children.Add(new Button()
            {
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

        public static async void TryToCreateBlueprintInstance(TestFieldsForBlueprintCreation e)
        {
            // Need to use dispatcher otherwise will get an threading exception
            await Task.Factory.StartNew(() =>
            {
                var textBoxes = new List<TextBox>();
                

                e.BlueprintCreateGrid.Dispatcher.Invoke(() =>
                {   
                    // Collecting textBoxes, cannot using linq with Grid
                    foreach (var UIElement in e.BlueprintCreateGrid.Children)
                    {
                        if (UIElement is TextBox)
                            textBoxes.Add((TextBox)UIElement);
                    }
                   
                    foreach (var textBox in textBoxes)
                    {
                        if (Int32.TryParse(textBox.Text, out int value))
                        {
                            // Query the corresponding resource that matches the representing TextBox
                            var resource = Data.userBlueprint.Resources.Where(x => x.ResourceType == textBox.Name);

                            // Element(0) because we should only have 1 matching resource, if they do match run this
                            if (resource.ElementAt(0).ResourceType == textBox.Name)
                            {                  
                                // Assigning the value we parsed eariler
                                resource.ElementAt(0).Quantity = value;
                                // Re-setting borderbrush incase it was changed because of an error earlier
                                textBox.BorderBrush = Brushes.LightSlateGray;
                                textBox.ToolTip = "Field data is accepted.";
                            }
                            // This should never run but if we didnt find a match throw an exception
                            else
                            {
                                // crummy error for not finding the resource right now - will add an exception
                                MessageBox.Show("Resource corresponding to blueprint was not found.");
                            }
                        }
                        // User input wasnt of numeric integer type so set textBox border to red
                        else
                        {
                            textBox.BorderBrush = Brushes.Red;
                            textBox.ToolTip = "ERROR: Input was not of numeric integer type.";
                        }
                    }

                    // Writes our created blueprint object to the userCreated
                    if (Data.userBlueprint.Resources.All(x => x.Quantity != 0))
                    {
                        // NEED TO ADD CODE FOR WHEN THE FILE CONTAINS NO BLUEPRINTS
                        string jsonString = JsonConvert.SerializeObject(Data.userBlueprint);
                        
                        StreamReader reader = new StreamReader(Data.USER_CREATED_BLUEPRINTS_PATH);
                        string doc = reader.ReadToEnd();
                        doc = doc.Remove(doc.Length - 1);
                        doc += "," + jsonString + "]";
                        reader.Close();

                        StreamWriter writer = new StreamWriter(Data.USER_CREATED_BLUEPRINTS_PATH);
                        writer.Write(doc);
                        writer.Close();
                    }
                });       
            });
        }
    }
}
