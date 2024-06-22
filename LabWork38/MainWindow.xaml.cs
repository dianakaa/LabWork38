using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;

namespace LabWork38
{
    public partial class MainWindow : Window
    {
        private List<FileInfo> _files;
        private int _currentPage = 0;
        private const int _pageSize = 5;
        public MainWindow()
        {
            InitializeComponent();
            // Получение данных
            LoadFiles();
            ShowPage();
            DataContext = this; // Установка контекста данных для привязки
        }

        private void LoadFiles()

        {
            string path = "C:\\Temp\\ispp21";
            DirectoryInfo directory = new DirectoryInfo(path);
            _files = directory.GetFiles().OrderBy(f => f.FullName).ToList();
        }

        private void ShowPage()
        {
            var pageFiles = _files.Skip(_currentPage * PageSize).Take(PageSize).ToList();
            FilesDataGrid.ItemsSource = pageFiles.Select(f => new
            {
                FileName = f.Name,
                Extension = f.Extension,
                FullName = f.FullName,
                Size = f.Length,
                CreationDate = f.CreationTime
            }).ToList();

            // Обновление текста с информацией о количестве показанных записей
            RecordsTextBlock.Text = $"Показано {_currentPage * PageSize + pageFiles.Count} из {_files.Count} записей";
            MoreButton.IsEnabled = _files.Count > (_currentPage + 1) * PageSize;
        }

        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage++;
            ShowPage();
        }
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value > 0)
                {
                    value = _pageSize;
                    _currentPage = 0; // Сброс текущей страницы
                    ShowPage(); // Обновление данных
                }
            }
        }
    }
}