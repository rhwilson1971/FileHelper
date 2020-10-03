using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace FileHelper
{
    public class FileHelperViewModel : INotifyPropertyChanged
    {
        private readonly Dictionary<string, string> _fileData;
        public ICommand GetFilesCommand { get; set;  }
        public ICommand GetFolderCommand { get; set; }

        private string _filePath;
        private int _fileCount;

        public int FileCount
        {
            get { return _fileCount; }
            private set
            {
                _fileCount = value;
                OnPropertyChanged();
            }
        }

        public string FilePath 
        {
            get { return _filePath;  }
            private set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Dictionary<string, string> FileData
        {
            get
            {
                return _fileData;
            }
        }

        public FileHelperViewModel()
        {
            _fileData = new Dictionary<string, string>();

            GetFilesCommand = new RelayCommand(GetFiles, CanGetFiles);

            GetFolderCommand = new RelayCommand(GetFolder, CanGetFolder);

            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private bool CanGetFiles(object parameter) { return true;  }

        public void GetFiles(object parameter)
        {
            if (FilePath.Length == 0)
                return;

            try
            {

                if (Directory.Exists(FilePath))
                {
                    foreach (var file in Directory.EnumerateFiles(FilePath))
                    {
                        _fileData.Add(file, file);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {

            }
        }

        private bool CanGetFolder(object parameter)
        {
            return true;
        }

        private void GetFolder(object parameter)
        {
            var dialog = new CommonOpenFileDialog();


            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.IsFolderPicker = true;

            var result =
                dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                FilePath = dialog.FileName;
            }
        }
    }
}
