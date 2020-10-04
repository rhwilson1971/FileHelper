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
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;

namespace FileHelper
{
    public class NotificationObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class FileHelperData : NotificationObject
    {
        private string _fileName;
        public string FileName {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                OnPropertyChanged();
            }
        }

        private string _oldFilePath;
        public string OldFilePath
        {
            get
            {
                return _oldFilePath;
            }

            set
            {
                _oldFilePath = value;
                OnPropertyChanged();
            }
        }

        private string _newFilePath;
        public string NewFilePath {
            get
            {
                return _newFilePath;
            }

            set
            {
                _newFilePath = value;
                OnPropertyChanged();
            }
        }
    }

    public class FileHelperViewModel : NotificationObject
    {
        public ICommand GetFilesCommand { get; set;  }
        public ICommand GetFolderCommand { get; set; }

        private readonly ObservableCollection<FileHelperData> _fileData;

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


        public ObservableCollection<FileHelperData> FileData
        {
            get
            {
                return _fileData;
            }
        }

        public FileHelperViewModel()
        {
            _fileData = new ObservableCollection<FileHelperData>();

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
                        _fileData.Add(new FileHelperData()
                        {
                            FileName = Path.GetFileName(file),
                            OldFilePath = file,
                            NewFilePath = file
                        });
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
            var dialog = new CommonOpenFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                IsFolderPicker = true
            };

            var result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                FileData.Clear();
                FilePath = dialog.FileName;
            }
        }
    }
}
