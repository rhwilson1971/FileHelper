using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using Microsoft.Win32;
using System.Windows.Input;

namespace FileHelper
{


    public class FileHelperViewModel
    {

        private readonly Dictionary<string, string> _fileData;

      
        public ICommand GetFilesCommand { get; set;  }
        public ICommand GetFolderCommand { get; set; }

        public string FilePath { get; set; }

        public int FileCount { get; set; }

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
