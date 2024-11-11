using Autodesk.Revit.UI;
using System;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.ComTypes;
using Autodesk.Revit.DB.Architecture;

namespace Sheet_Generator
{
    public class SG_Sheet : IExternalEventHandler
    {

        public object TitleBLock { get; set; }

        public List<SG_Views> ViewList { get; set; }

        public string SheetNum { get; set; }    

        public string SheetName { get; set; } 

        public void Execute(UIApplication app)
        {

               UIDocument uidoc =  app.ActiveUIDocument;

               Document doc = uidoc.Document;



                    var TitleBlock = TitleBLock as Element; 
                    var views = ViewList.ToList();
                    var sheetName = SheetName;
                    var sheetnumStr = SheetNum;
                    var sheetnum = int.Parse(sheetnumStr);


                    var sheetview = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewSheet)).ToList() ;

                    var sheetIdx = new List<int>();

                    foreach (var view in sheetview)

                    {
                        var x = view as ViewSheet;


                        var numberStr = x.SheetNumber as string;
                        
                        var numder = int.Parse(numberStr);

                        sheetIdx.Add(numder);
                    }

                    var test  = new List<bool>();    

                    foreach (var index in sheetIdx) 
                    {
                        bool isEqual = index == sheetnum;



                        test.Add(isEqual);  
                    }

                   bool allFalse = test.All(b => !b);

            try
            {
                using (Transaction trans = new Transaction(doc, "Create Sheets"))
                {
                  
                    trans.Start();


                    if (allFalse)
                    {


                      var sheet = Autodesk.Revit.DB.ViewSheet.Create(doc, TitleBlock.Id);

                        sheet.Name = $"{sheetName}";
                        sheet.SheetNumber = sheetnumStr;


                        foreach(var view in views) 
                        
                        {
                             var x =  view ;


                            Viewport.Create(doc, sheet.Id,view.ID, new XYZ(0, 0, 0));
                        
                        
                        }



                    }
                    else 
                    
                    {
                    
                        TaskDialog.Show("Warning", "The Sheet Number is used before please use a unique number");
                    
                    }




















                    trans.Commit();


                }




            }
            catch (Exception  ex)
            {

                TaskDialog.Show("Warning", ex.Message);
            }







        }

        public string GetName()
        {
            return "Create Sheet";
        }
    }
}
