using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheet_Generator
{
    public class ViewTemplates
    {
        public string TemplateName { get; set; }
        
        public bool IsChecked { get; set; }

        public ViewTemplates (string templateName,  bool isChecked)
        {
            TemplateName = templateName;
            
            IsChecked = false;

        }

        
    }
}
