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
    public class Plan : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document document = uiDoc.Document;
            using (Transaction te = new Transaction(document, "Levels and views"))
                try
                {
                    //create levels
                    te.Start();

                    var viewFamilyType = new FilteredElementCollector(document)
                            .OfClass(typeof(View))
                            .Cast<View>()
                            .Where(v => !v.IsTemplate)
                            .Where(v => v.ViewType == ViewType.FloorPlan || v.ViewType == ViewType.CeilingPlan ||
                            v.ViewType == ViewType.Section || v.ViewType == ViewType.Elevation ||
                            v.ViewType == ViewType.ThreeD)
                            .ToList();


                    var titleBlockTypes = new FilteredElementCollector(document)
                       .OfCategory(BuiltInCategory.OST_TitleBlocks)
                       .WhereElementIsElementType().First();
                    if (titleBlockTypes == null)
                    {
                        message = "No title block found in the project.";
                        return Result.Failed;
                    }

                    foreach (View view in viewFamilyType)
                    {
                        // Create a new sheet

                        XYZ sheetposition = new XYZ(0, 0, 0);
                        ViewSheet newSheet = ViewSheet.Create(document, titleBlockTypes.Id);
                        if (newSheet == null)
                        {
                            message = "Failed to create sheet.";
                            return Result.Failed;
                        }

                        // Set the sheet name to the view name
                        newSheet.Name = view.Name + "_Sheet";

                        BoundingBoxUV outline = newSheet.Outline;
                        double sheetWidth = outline.Max.U - outline.Min.U;
                        double sheetHeight = outline.Max.V - outline.Min.V;

                        // Calculate the center position of the sheet
                        XYZ viewPosition = new XYZ(sheetWidth / 2, sheetHeight / 2, 0);

                        // Place the view on the sheet

                        Viewport.Create(document, newSheet.Id, view.Id, viewPosition);
                    }


                    te.Commit();



                    return Result.Succeeded;
                }
                catch (Exception ex)
                {

                    TaskDialog.Show("Warning", ex.Message);
                    te.RollBack();
                    return Result.Failed;

                }
        }
    }
}
