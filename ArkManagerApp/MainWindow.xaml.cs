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
        public event SelectedBlueprintDel OnSelectSearch;         // Event for when comboBox search changes
        public event TextBoxFieldChangedDel OnTextBoxLostFocus;   // Event for when TextBox in blueprint creation loses user focus

        public MainWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));

            OnSelectSearch += BlueprintCreationUI.OnTestBlueprintSelection;
            OnTextBoxLostFocus += BlueprintCreationUI.OnTestBlueprintTextBoxField;

            foreach (var blueprint in Data.Blueprints.Values)
            {
                // Populating our comboBox's Items with blueprints
                ComboBoxOfBlueprints.Items.Add(blueprint.BlueprintType);
            }
        }
        
        private void ComboBoxOfBlueprints_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnSelectSearch?.Invoke(new BlueprintSearchedArgs(this, ComboBoxOfBlueprints, MainGrid));
        }


        public void TextBox_FieldChanged(object sender, RoutedEventArgs e)
        {
            OnTextBoxLostFocus?.Invoke(new TextBoxFieldChangedArgs((TextBox)sender));
        }
    }
}
