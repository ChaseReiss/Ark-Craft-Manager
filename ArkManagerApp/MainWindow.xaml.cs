using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ArkManager;

namespace ArkManagerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event Action<BlueprintSearchedArgs> OnCreateBlueprintSearch, OnMyBlueprintsSearch;         // Event for when comboBox search changes
        public event Action<TestFieldsForBlueprintCreation> OnTryCreateBlueprintInstance;                 // Event for when the user pressed the create button

        public MainWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));

            OnMyBlueprintsSearch += MyBlueprintsGUI.PlaceHolder;
            OnCreateBlueprintSearch += CreateBlueprintGUI.OnTestBlueprintSelection;
            OnTryCreateBlueprintInstance += CreateBlueprintGUI.TryToCreateBlueprintInstance;

            // Adding all the blueprints to the Create ComboBox
            foreach (var blueprint in Data.Blueprints)
            {
                // Populating our comboBox's Items with blueprints
                CreateBlueprintComboBox.Items.Add(blueprint.BlueprintType);                   
            }

            // Adding all the blueprints to the 'My blueprints comboBox'
            foreach (var blueprint in Data.UserCreatedBlueprints)
            {
                MyBlueprintsComboBox.Items.Add(blueprint.BlueprintType);
            }
        }
        
        private void ComboBoxOfBlueprints_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {   
            // If the UI for my blueprints is loaded, fire this event
            if (MyBlueprintsGrid.IsLoaded)
                OnMyBlueprintsSearch?.Invoke(new BlueprintSearchedArgs(this, MyBlueprintsComboBox, MyBlueprintsGrid));
            // Otherwise if the UI for creating a blueprint is loaded fire this event
            else if (CreateBlueprintGrid.IsLoaded)
                OnCreateBlueprintSearch?.Invoke(new BlueprintSearchedArgs(this, CreateBlueprintComboBox, CreateBlueprintGrid));         
        }

        public void CreateBlueprintButton_Clicked(object sender, RoutedEventArgs e)
        {
            OnTryCreateBlueprintInstance?.Invoke(new TestFieldsForBlueprintCreation(CreateBlueprintGrid));
        }
    }
}
