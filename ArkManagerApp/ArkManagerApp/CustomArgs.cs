using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ArkManagerApp
{
    public class BlueprintSearchedArgs : EventArgs
    {
        MainWindow mainWindow;       
        ComboBox searchComboBox;
        Grid mainGrid;

        public ComboBox SearchComboBox { get => searchComboBox; set => searchComboBox = value; }
        public Grid MainGrid { get => mainGrid; set => mainGrid = value; }
        public MainWindow MainWindow { get => mainWindow; set => mainWindow = value; }

        public BlueprintSearchedArgs(MainWindow _mainWindow, ComboBox _comboBox, Grid _mainGrid)
        {
            MainWindow = _mainWindow;
            SearchComboBox = _comboBox;
            MainGrid = _mainGrid;
        }
    }

    public class TextBoxFieldChangedArgs : EventArgs
    {
        TextBox textBox;

        public TextBox TextBox { get => textBox; set => textBox = value; }

        public TextBoxFieldChangedArgs(TextBox _textBox)
        {
            TextBox = _textBox;
        }
    }
}
