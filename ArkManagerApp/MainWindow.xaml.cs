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
        public event Action<BlueprintSearchedArgs> OnCreateBlueprintSearch, OnBrowseMyBlueprintsSearch;         // Event for when comboBox search changes
        public event Action<TestFieldsForBlueprintCreation> OnTryCreateBlueprintInstance;                       // Event for when the user pressed the create button

        public MainWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));

            OnBrowseMyBlueprintsSearch += MyBlueprintsGUI.PlaceHolder;
            OnCreateBlueprintSearch += CreateBlueprintGUI.OnTestBlueprintSelection;
            OnTryCreateBlueprintInstance += CreateBlueprintGUI.TryToCreateBlueprintInstance;

            foreach (var blueprint in Data.Blueprints.Values)
            {
                // Populating our comboBox's Items with blueprints
                ComboBoxOfBlueprints.Items.Add(blueprint.BlueprintType);
            }
        }
        
        private void ComboBoxOfBlueprints_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyBlueprintsGrid.IsLoaded)
                OnBrowseMyBlueprintsSearch?.Invoke(new BlueprintSearchedArgs(this, ComboBoxOfBlueprints, MyBlueprintsGrid));
            else if (CreateBlueprintGrid.IsLoaded)
                OnCreateBlueprintSearch?.Invoke(new BlueprintSearchedArgs(this, ComboBoxOfBlueprints, CreateBlueprintGrid));
        }

        public void CreateBlueprintButton_Clicked(object sender, RoutedEventArgs e)
        {
            OnTryCreateBlueprintInstance?.Invoke(new TestFieldsForBlueprintCreation(CreateBlueprintGrid));
        }
    }
}
