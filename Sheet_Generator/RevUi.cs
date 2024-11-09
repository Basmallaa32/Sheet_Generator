using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace Sheet_Generator
{
    public class RevUi : IExternalApplication
    {
            Assembly assembly = Assembly.GetExecutingAssembly();
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {

            try

            {


                application.CreateRibbonTab("Sheet Generator");


                var About_ = application.CreateRibbonPanel("Sheet Generator", "About Addin");
                var Generator_ = application.CreateRibbonPanel("Sheet Generator", "Generator");


                CreatePushButton("About", "About", typeof(About).FullName, $"{nameof(Sheet_Generator)}.Resources.icons8-about-32.ico", About_, false, "About Sheet Generator");
                CreatePushButton("Sheet", "Sheet", typeof(SG_SheetIcon).FullName, $"{nameof(Sheet_Generator)}.Resources.icons8-about-32.ico", Generator_, false, "Generate sheets by selecting views to be placed on specific sheets.");



                var img1 = img($"{nameof(Sheet_Generator)}.Resources.icons8-about-16.ico");
                var img2 = img($"{nameof(Sheet_Generator)}.Resources.icons8-about-16.ico");

                AddStackedButton(Generator_, img1, img2);


                return Result.Succeeded;

            }

            catch (Exception ex)
            {
                TaskDialog.Show("warning", ex.Message);

                return Result.Failed;
            }


        }



        private void AddStackedButton(RibbonPanel panel, string Source1, string Source2)


        {
            string AssemplyPath = Assembly.GetExecutingAssembly().Location;
            Uri uri = new Uri(Path.Combine(Path.GetDirectoryName(AssemplyPath)));


            BitmapImage img1 = new BitmapImage();
            Stream stream1 = assembly.GetManifestResourceStream(Source1);
            img1.BeginInit();
            img1.StreamSource = stream1;
            img1.EndInit();

            BitmapImage img2 = new BitmapImage();
            Stream stream2 = assembly.GetManifestResourceStream(Source2);
            img2.BeginInit();
            img2.StreamSource = stream2;
            img2.EndInit();

         



            PushButtonData Q = new PushButtonData("Quick Plan", "Quick Plan", AssemplyPath, typeof(Plan).FullName);
            Q.Image = img1;
            Q.ToolTip = "Quick Sheet Generator contains Plans views";
            PushButtonData W = new PushButtonData("Quick Elevation", "Quick Elevation", AssemplyPath, typeof(Elevation).FullName);
            W.Image = img2;
            W.ToolTip = "Quick Sheet Generator contains Elevations views";

          

            IList<RibbonItem> items = panel.AddStackedItems(Q, W);

        }


        private static string img(string imagepath)

        {
            return imagepath;

        }



        private PushButtonData CreatePushButton(string buttonname, string buttontext, string classname, string resource, RibbonPanel panel, bool isSmall, string Description = null)
            {
                PushButtonData pushButtonData = new PushButtonData(buttonname, buttontext, assembly.Location, classname);

                PushButton btn = panel.AddItem(pushButtonData) as PushButton;
                BitmapImage img = new BitmapImage();
                Stream stream = assembly.GetManifestResourceStream(resource);
                img.BeginInit();
                img.StreamSource = stream;
                img.EndInit();
                if (isSmall)
                {
                    btn.Image = img;
                }
                else
                {
                    btn.LargeImage = img;
                }
                if (!String.IsNullOrWhiteSpace(Description))
                {
                    btn.ToolTip = Description;
                }
                return pushButtonData;
            }





    }
}
