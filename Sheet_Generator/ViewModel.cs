using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheet_Generator
{
    internal class ViewModel
    {
        public List<SG_Views> Views = new List<SG_Views>();  
        public List<ViewTemplates> ViewTemplates = new List<ViewTemplates>();


        public ViewModel(Document doc) 
        {
            Views = GetAllNonTemplateViews(doc);
            ViewTemplates = GetAllRevitViews(doc);


        }


        public List<SG_Views> GetAllNonTemplateViews (Document doc)
        {
            return new FilteredElementCollector(doc)
            .OfClass(typeof(View))
            .Cast<View>()
            .Where(view => !view.IsTemplate && view.CanBePrinted )
            .Select(view => new SG_Views(view.Name, view.ViewType.ToString(), false , view.Id))
            .ToList();

        }
        public List<ViewTemplates> GetAllRevitViews(Document doc)
        {
            return new FilteredElementCollector(doc)
            .OfClass(typeof(View))
            .Cast<View>()
            .Where(template => template.IsTemplate)
            .Select(template => new ViewTemplates(template.Name, false))
            .ToList();

        }

    }
}

