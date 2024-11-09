using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheet_Generator
{
    [Transaction(TransactionMode.Manual)]
    public class SG_SheetIcon : IExternalCommand  
    {
       
        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            SheetUi ui = new SheetUi(doc);
            ui.Show();


           return Result.Succeeded;
        }
    }
}
