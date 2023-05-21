using Caliburn.Micro.Tutorial.Wpf.Models;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Caliburn.Micro.Tutorial.Wpf.ViewModels
{
    public class CategoryViewModel : Screen
    {
        #region Private fields
        private BindableCollection<CategoryModel> _categoryList = new BindableCollection<CategoryModel>();
        private CategoryModel _selectedCategoryModel;
        private string _categoryName;
        private string _categoryDescription;
        private Visibility visibilityList;
        private string link;
        #endregion Private fields

        #region Public fields
        public string Link
        {
            get { return link; }
            set { link = value; }
        }

        public BindableCollection<CategoryModel> CategoryList
        {
            get
            {
                return _categoryList;
            }
            set
            {
                _categoryList = value;
            }
        }

        public CategoryModel SelectedCategory
        {
            get
            {
                return _selectedCategoryModel;
            }

            set
            {
                _selectedCategoryModel = value;
                NotifyOfPropertyChange(nameof(SelectedCategory));
            }
        }

        public string CategoryName
        {
            get => _categoryName; set
            {
                _categoryName = value;
                NotifyOfPropertyChange(nameof(CategoryName));
            }
        }

        public string CategoryDescription
        {
            get => _categoryDescription; set
            {
                _categoryDescription = value;
                NotifyOfPropertyChange(nameof(CategoryDescription));
            }
        }
        
        public Visibility VisibilityList
        {
            get { return visibilityList; }
            set
            {
                visibilityList = value;
                NotifyOfPropertyChange(nameof(VisibilityList));
            }
        }

        #endregion Public fields

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            VisibilityList = Visibility.Hidden;
        }

        public void Edit()
        {
            CategoryName = SelectedCategory.CategoryName;
            CategoryDescription = SelectedCategory.CountValue;
        }

        public void ImageClicked()
        {
            try
            {
                // Create OpenFileDialog 
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                // Set filter for file extension and default file extension 
                //dlg.DefaultExt = ".png";
                dlg.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";


                // Display OpenFileDialog by calling ShowDialog method 
                Nullable<bool> result = dlg.ShowDialog();


                // Get the selected file name and display in a TextBox 
                if (result == true)
                {
                    // Open document 
                    Link = dlg.FileName;
                    //textBox1.Text = filename;

                    ReadCSV();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Private method
        private void ReadCSV()
        {
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Link);

                bool isHeader = true;
                int column1 = 0;
                int column2 = 0;
                int row = -1;
                using (var reader = Read(sr))
                {
                    do
                    {
                        while (reader.Read())
                        {
                            row++;
                            if (isHeader)
                            {
                                isHeader = false;
                                for (int i = 0; i < 2; i++)
                                {
                                    if (reader.GetValue(i).ToString().Equals("Field"))
                                    {
                                        column1 = i;
                                    }

                                    if (reader.GetValue(i).ToString().Equals("Value"))
                                    {
                                        column2 = i;
                                    }
                                }
                            }
                            else
                            {
                                if (reader.GetValue(column1) != null)
                                {
                                    string field = reader.GetValue(column1).ToString();
                                    string value = reader.GetValue(column2).ToString();
                                    CategoryList.Add(new CategoryModel { CategoryName = field, CountValue = value });
                                }
                            }
                        }
                    }

                    while (reader.NextResult());
                }


                //close the file
                sr.Close();
                Console.ReadLine();

                VisibilityList = Visibility.Visible;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        private IDataReader Read(StreamReader data)
        {

            MemoryStream memoryStream = new MemoryStream();
            data.BaseStream.CopyTo(memoryStream);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            memoryStream.Position = 0;

            return ExcelReaderFactory.CreateReader(memoryStream);
        }
        #endregion Private method
    }
}
