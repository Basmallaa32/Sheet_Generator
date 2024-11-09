using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheet_Generator
{
    public class SG_Views
    {
        public string ViewName { get; set; }
        public string ViewCategory { get; set; }
        public bool IsChecked { get; set; }
        internal Autodesk.Revit.DB.ElementId ID { get; set; } 

        public SG_Views(string viewName, string viewCategory, bool isChecked , Autodesk.Revit.DB.ElementId i)
        {
            ViewName = viewName;
            ViewCategory = viewCategory;
            IsChecked = false;
            ID = i;
        }

        
    }
}
