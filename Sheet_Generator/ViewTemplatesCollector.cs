using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheet_Generator
{
    public class ViewTemplatesCollector
    {
        public List<ViewTemplates> GetAllRevitViews(Document doc)
        {
            var views = new FilteredElementCollector(doc)
             .OfClass(typeof(View))
             .Cast<View>()
             .Where(template => template.IsTemplate)
             .Select(template => new ViewTemplates(template.Name, false))
             .ToList();

            return views;   

        }
    }
}
