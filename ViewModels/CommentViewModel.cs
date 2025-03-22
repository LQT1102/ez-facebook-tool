using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EZ.FACEBOOK.TOOL.Models;
using EZ.FACEBOOK.TOOL.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using EZ.FACEBOOK.TOOL.Helpers;

namespace EZ.FACEBOOK.TOOL.ViewModels
{
    public class CommentViewModel : BaseViewModel
    {
        private readonly CommentConfigService _commentConfigService;
        private string _commentText = "";
        private int _delaySeconds = 5;
        private string _groupUrls = "";
        private ObservableCollection<string> _selectedImages;

        public CommentViewModel(CommentConfigService commentConfigService)
        {
            _commentConfigService = commentConfigService;
            _selectedImages = new ObservableCollection<string>();
            
            AddImagesCommand = new RelayCommand(AddImages);
            RemoveImageCommand = new RelayCommand<string>(RemoveImage);
            StartCommand = new RelayCommand(Start);

            LoadConfig();
        }

        public string CommentText
        {
            get => _commentText;
            set
            {
                if (SetProperty(ref _commentText, value))
                {
                    SaveConfig();
                }
            }
        }

        public int DelaySeconds
        {
            get => _delaySeconds;
            set
            {
                if (SetProperty(ref _delaySeconds, value))
                {
                    SaveConfig();
                }
            }
        }

        public string GroupUrls
        {
            get => _groupUrls;
            set
            {
                if (SetProperty(ref _groupUrls, value))
                {
                    SaveConfig();
                }
            }
        }

        public ObservableCollection<string> SelectedImages
        {
            get => _selectedImages;
            set => SetProperty(ref _selectedImages, value);
        }

        public ICommand AddImagesCommand { get; }
        public ICommand RemoveImageCommand { get; }
        public ICommand StartCommand { get; }

        private void AddImages()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif)|*.jpg;*.jpeg;*.png;*.gif"
            };

            if (dialog.ShowDialog() == true)
            {
                foreach (var file in dialog.FileNames)
                {
                    if (!SelectedImages.Contains(file))
                    {
                        SelectedImages.Add(file);
                    }
                }
                SaveConfig();
            }
        }

        private void RemoveImage(string imagePath)
        {
            if (SelectedImages.Remove(imagePath))
            {
                SaveConfig();
            }
        }

        private void Start()
        {
            // TODO: Implement auto-commenting logic
            System.Windows.MessageBox.Show("Bắt đầu tự động bình luận...");
        }

        private async void LoadConfig()
        {
            var config = await _commentConfigService.LoadConfigAsync();
            CommentText = config.CommentText;
            DelaySeconds = config.DelaySeconds;
            GroupUrls = string.Join("\n", config.GroupUrls ?? new List<string>());
            SelectedImages = new ObservableCollection<string>(config.ImagePaths ?? new List<string>());
        }

        private async void SaveConfig()
        {
            var config = new CommentConfig
            {
                CommentText = CommentText,
                DelaySeconds = DelaySeconds,
                GroupUrls = GroupUrls.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList(),
                ImagePaths = SelectedImages.ToList()
            };
            await _commentConfigService.SaveConfigAsync(config);
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public void Execute(object parameter) => _execute();
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute((T)parameter);

        public void Execute(object parameter) => _execute((T)parameter);
    }
} 