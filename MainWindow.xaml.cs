using System.Windows;
using System.Windows.Controls;
using EZ.FACEBOOK.TOOL.Views;
using EZ.FACEBOOK.TOOL.ViewModels;
using EZ.FACEBOOK.TOOL.Services;

namespace EZ.FACEBOOK.TOOL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AccountService _accountService;
        private readonly CommentConfigService _commentConfigService;
        private readonly AccountViewModel _accountViewModel;
        private readonly CommentViewModel _commentViewModel;

        public MainWindow()
        {
            InitializeComponent();

            // Khởi tạo services
            _accountService = new AccountService();
            _commentConfigService = new CommentConfigService();

            // Khởi tạo ViewModels
            _accountViewModel = new AccountViewModel(_accountService);
            _commentViewModel = new CommentViewModel(_commentConfigService);

            // Set default view
            MainFrame.Navigate(new CommentView { DataContext = _commentViewModel });
        }

        private void AccountsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AccountView { DataContext = _accountViewModel });
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CommentView { DataContext = _commentViewModel });
        }
    }
}