using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.DB;
using System.ComponentModel;

namespace Sheet_Generator
{
    /// <summary>
    /// Interaction logic for SheetUi.xaml
    /// </summary>
    public partial class SheetUi : Window

    {

        ExternalEvent EXSheet;
        SG_Sheet Create = new SG_Sheet();
        private Document document;
        ViewModel vm; 
        public SheetUi(Document doc)
        {
            InitializeComponent();
            document = doc;
            ShowSelectedViews();
            ShowSelectedTemplates();


            EXSheet = ExternalEvent.Create(Create);
            vm = new ViewModel(doc);
            DataContext = vm;

            List<FamilySymbol> titleBlockTypes = GetAllTitleBlockTypes(doc);
            titleBlockComboBox.ItemsSource = titleBlockTypes;
        }



        private void createSheetBtn_Click(object sender, RoutedEventArgs e)
        {
           
            EXSheet.Raise();

           
        }

        private void duplicateBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateNewtempBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowSelectedViews()
        {
            ViewsCollector ViewsCollector = new ViewsCollector();
            List<SG_Views> views = ViewsCollector.GetAllRevitViews(document);
            if (views.Count == 0)
            {
                MessageBox.Show("No views found!");
            }
            ViewsDataGrid.ItemsSource = views;
            ViewsDataGrid2.ItemsSource = views;
        }


        private void ShowSelectedTemplates()
        {

            ViewTemplatesCollector viewTemplatesCollector = new ViewTemplatesCollector();
            List<ViewTemplates> templates = viewTemplatesCollector.GetAllRevitViews(document);

            TempDataGrid.ItemsSource = templates;

        }

    

        public List<FamilySymbol> GetAllTitleBlockTypes(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<FamilySymbol> titleBlockTypes = collector
                .OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_TitleBlocks)
                .Cast<FamilySymbol>()
                .ToList();

            return titleBlockTypes;
        }

        private void apply_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        public void titleBlockComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            var SheetTemplate = titleBlockComboBox.SelectedItem;

            Create.TitleBLock = SheetTemplate;

        }

        public void ViewsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            var viewModel = (SG_Views)this.DataContext;

            if (viewModel.IsChecked)
            {
                





            }


            var ss = ViewsDataGrid.SelectedCells.ToList();
            
            foreach (var s in ss) 
            {
                var qw = s.Column.GetCellContent(0);

                  
            
            }

            var SelectedView = ViewsDataGrid.SelectedItem as SG_Views;

            var selectedviews = new List<SG_Views>();

            selectedviews.Add(SelectedView);

            Create.ViewList = selectedviews;



           

        }


        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            var nameText = name.Text;

            Create.SheetName = nameText;

        }

        private void number_TextChanged(object sender, TextChangedEventArgs e)
        {
            var numberText = number.Text;

             Create.SheetNum = numberText;



        }


        private void MaskNumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !TextIsNumeric(e.Text);
        }

        private void MaskNumericPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string input = (string)e.DataObject.GetData(typeof(string));
                if (!TextIsNumeric(input)) e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool TextIsNumeric(string input)
        {
            return input.All(c => Char.IsDigit(c) || Char.IsControl(c));
        }

    }
}