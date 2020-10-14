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
        private bool _IsDirectory;

        public bool IsDirectory
        {
            get { return _IsDirectory; }
            set
            {
                _IsDirectory = value;
                OnPropertyChanged();
            }
        }

        private string _fileName;
        public string FileName
        {
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
        public string NewFilePath
        {
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
        #region Public Variables
        public ICommand GetFilesCommand { get; set; }
        
        public ICommand GetFolderCommand { get; set; }

        public ICommand ClearCommand { get; set; }

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
            get { return _filePath; }
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
        #endregion

        #region Constructors    
        public FileHelperViewModel()
        {
            _fileData = new ObservableCollection<FileHelperData>();

            GetFilesCommand = new RelayCommand(GetFiles, (o) => { return true; });

            GetFolderCommand = new RelayCommand(GetFolder, (o) => { return true; } );

            ClearCommand = new RelayCommand(Clear, (o) => { return true; });

            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
        #endregion

        #region Command Interface methods
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
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }
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

        private void Clear(object parameter)
        {
            FileData.Clear();
        }

        #endregion
    }
}
