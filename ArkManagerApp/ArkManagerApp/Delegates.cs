using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkManagerApp
{
    public delegate Task SelectedBlueprintDel(BlueprintSearchedArgs e);         // For updateing UI base off the comboBox in blueprint creation section.
    public delegate Task TextBoxFieldChangedDel(TextBoxFieldChangedArgs e);     // For interpreting textBox text field and providing feed back to the user.
}
