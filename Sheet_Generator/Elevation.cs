using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sheet_Generator
{
    [Transaction(TransactionMode.Manual)]
    public class Elevation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;


             var roomCollector = new FilteredElementCollector(doc)
            .OfCategory(BuiltInCategory.OST_Rooms).WhereElementIsNotElementType()
            .ToElements().ToList();
         
             var viewplan = new FilteredElementCollector(doc)
            .OfClass(typeof(View)).WhereElementIsNotElementType()
            .ToElements().Cast<View>().Where(c => c.ViewType == ViewType.FloorPlan & c.IsTemplate ==false).FirstOrDefault();


            var titleBlock = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_TitleBlocks)
            .WhereElementIsElementType().FirstOrDefault();

            var elevationMarker =new FilteredElementCollector(doc)
            .OfClass(typeof(ElevationMarker)).ToList();

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> sheetElements = collector.OfClass(typeof(ViewSheet)).ToElements();

            List<Element> views = new List<Element>();


            foreach(var room in roomCollector) 
            
            {
            
               var roomBox =room.get_BoundingBox(viewplan);
            
            
                   foreach (var item in elevationMarker)
            
                   {
                     var MarkerBox =  item.get_BoundingBox(viewplan);


                     var test =  BoundingBoxesIntersect(roomBox, MarkerBox);

                         using (Transaction trans = new Transaction(doc, "Create Elevation sheets"))
                            {
                             
                             trans.Start();

                              if (test) 
                              {
                                 
                                var elevMarker = item as ElevationMarker;

                                var num =elevMarker.CurrentViewCount;

       
                                var sheet = Autodesk.Revit.DB.ViewSheet.Create(doc, titleBlock.Id);

                                sheet.Name = $"{room.Name}";


                                 for (var i = 0; i < num; i++)
                                 {


                                     var elevID = elevMarker.GetViewId(i);

                                     var element = doc.GetElement(elevID) as View;


                                     var status = element.GetPlacementOnSheetStatus();
                            
                                     if (status == ViewPlacementOnSheetStatus.NotPlaced)
                                     { 
                                



                                       Viewport.Create(doc, sheet.Id, elevID, new XYZ(0, 0, 0));
                                
                                     }

                                 }

                              }

                             trans.Commit();
                         }


                   }



            
            }









                return Result.Succeeded;
        }



        private List<Wall> GetWallsInRoom (Document doc,Autodesk.Revit.DB.Element room)
        {

            




            BoundingBoxXYZ RoomBox = room.get_BoundingBox(null);

            FilteredElementCollector wallCollector = new FilteredElementCollector(doc);
        
            // Filter elements that are of type Wall
            var allWalls = wallCollector
            .OfClass(typeof(Wall))
            .Cast<Wall>()
            .ToList();

            List<Wall> WallsInRoom = new List<Wall>();

             foreach(var wall in allWalls) 
            
             {

                BoundingBoxXYZ WallBox = wall.get_BoundingBox(null);

                bool test = BoundingBoxesIntersect(WallBox, RoomBox);


                if (test) 
                
                {

                    WallsInRoom.Add(wall);
                 
                
                }



             }
 











            return WallsInRoom;

        }



            private  List<View> GetViewsInRoom(Document doc, Autodesk.Revit.DB.Element room)
        {



            BoundingBoxXYZ RoomBox = room.get_BoundingBox(null);

       
       
            List<View> viewsInRoom = new List<View>();

            var viewCollector = new FilteredElementCollector(doc)
              .OfClass(typeof(View)).WhereElementIsNotElementType()
              .ToElements().Cast<View>().Where(c => c.ViewType == ViewType.Elevation).ToList();



       
            var Viewslocation = new List<XYZ>();
       
            foreach (var view in viewCollector)
       
            {
                if (!view.IsTemplate)
                {
                    BoundingBoxXYZ viewBox = view.get_BoundingBox(null);


                    //if (viewBox != null)

                    //{

                    //    XYZ minPoint = viewBox.Min;
                    //    XYZ maxPoint = viewBox.Max;
                    //    XYZ centerPoint = (minPoint + maxPoint) / 2;

                    //}



                    bool test = BoundingBoxesIntersect(RoomBox, viewBox);


                    if (test)
                    {

                        viewsInRoom.Add(view);


                    }




                }



          





            }

                return viewsInRoom;
         
        }
        

        private bool BoundingBoxesIntersect(BoundingBoxXYZ box1, BoundingBoxXYZ box2)
        {
            return !(box1.Max.X < box2.Min.X || box1.Min.X > box2.Max.X ||
                     box1.Max.Y < box2.Min.Y || box1.Min.Y > box2.Max.Y ||
                     box1.Max.Z < box2.Min.Z || box1.Min.Z > box2.Max.Z);
        }






       

    }
}
