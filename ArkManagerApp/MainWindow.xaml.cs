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
using ArkManager.CustomType;

namespace ArkManagerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event Action<BlueprintSearchedArgs> OnCreateBlueprintSearch, OnMyBlueprintsSearch;      // Event for when comboBox search changes
        public event Action<TestFieldsForBlueprintCreation> OnTryCreateBlueprintInstance;                 // Event for when the user pressed the create button
        //public event Func<BlueprintSearchedArgs, List<string>> OnUpdateComboBoxDropDown;

        public MainWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));
            
            // Event for user's created blueprints comboBox
            OnMyBlueprintsSearch += MyBlueprintsGUI.PlaceHolder;
            // Event for default game blueprints comboBox
            OnCreateBlueprintSearch += CreateBlueprintGUI.OnTestBlueprintSelection;
            // Event for when the user tries to create a blueprint instance
            OnTryCreateBlueprintInstance += CreateBlueprintGUI.TryToCreateBlueprintInstance;
            // Event for updating the dropdown menu's items
            CreateBlueprintComboBox.KeyUp += SearchLogic.OnTextInputReceived;
            // Event for when the comboBox gets local focus
            CreateBlueprintComboBox.GotFocus += SearchLogic.GotFocus;
            
            // Filling the blueprint creation comboBox with items
            foreach (var blueprint in Data.DefaultBlueprints)
            {
                // Populating our comboBox's Items with blueprints  
                CreateBlueprintComboBox.Items.Add(blueprint.BlueprintType);
            };
        }

        // NEED TO CHANGED THIS SO THAT OUR ITEMS ARE A OBJECT WITH VISIBILITY PORPERTYS.. easier to set visiable then deleting and adding back alter

        /// <summary>
        ///     Fires an event to fill the comboBox's items with the user's created blueprints
        /// </summary>
        private void MyBlueprintsRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            MyBlueprintsComboBox.Items.Clear();
            foreach (var item in Data.UserCreatedBlueprints)
            {
                MyBlueprintsComboBox.Items.Add(item.BlueprintType);
            }
        }

        /// <summary>
        ///     Fires an event to fill the comboBox's items with the default game blueprints
        /// </summary>
        private void DefaultBlueprintsRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            MyBlueprintsComboBox.Items.Clear();
            foreach (var item in Data.DefaultBlueprints)
            {
                MyBlueprintsComboBox.Items.Add(item.BlueprintType);
            }
        }

        /// <summary>
        ///     Fires an event when the user changes the comboBox's selected item.
        /// </summary>
        private void ComboBoxOfBlueprints_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {   
            // If the UI for my blueprints is loaded, fire this event
            if (MyBlueprintsGrid.IsLoaded)
                OnMyBlueprintsSearch?.Invoke(new BlueprintSearchedArgs(this, MyBlueprintsComboBox, MyBlueprintsGrid));
            // Otherwise if the UI for creating a blueprint is loaded fire this event
            else if (CreateBlueprintGrid.IsLoaded)
                OnCreateBlueprintSearch?.Invoke(new BlueprintSearchedArgs(this, CreateBlueprintComboBox, CreateBlueprintGrid));         
        }

        /// <summary>
        ///     Fires an event when the user has clicked the 'Create' button in the blueprint creation tab.
        /// </summary>
        public void CreateBlueprintButton_Clicked(object sender, RoutedEventArgs e)
        {
            OnTryCreateBlueprintInstance?.Invoke(new TestFieldsForBlueprintCreation(CreateBlueprintGrid));
        }
    }
}
