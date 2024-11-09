using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheet_Generator
{
    public class ViewsCollector
    {
        public List<SG_Views> GetAllRevitViews(Document doc)
        {
            var views = new FilteredElementCollector(doc)
            .OfClass(typeof(View))
            .Cast<View>() 
            .Where(view => !view.IsTemplate && view.CanBePrinted && !(view is ViewSheet))
            .ToList();

            List<View> viewsList = new List<View>();      

            foreach (var view in views)
            
            {
  
                var test = view.GetPlacementOnSheetStatus();

                if(test== ViewPlacementOnSheetStatus.NotPlaced)
                
                {

                    viewsList.Add(view);
                  
                }


            }


            return viewsList.Select(view => new SG_Views(view.Name, view.ViewType.ToString(), false,view.Id)).ToList();
           

        }
    }
}
