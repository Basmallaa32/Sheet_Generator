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
    public class About : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
          


            TaskDialog.Show("About", "Created by Yousef Altear & Basmalla Ahmed");


            return Result.Succeeded; 




        }
    }
}
