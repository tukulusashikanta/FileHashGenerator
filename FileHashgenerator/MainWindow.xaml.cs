using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FileHashgenerator
{
    public partial class MainWindow : Window
    {
        private string _selectedFilePath;

        public MainWindow()
        {
            InitializeComponent();
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            if (version != null)
            {
                VersionText.Text = $"v{version.Major}.{version.Minor}.{version.Build}";
            }
        }

        private void BrowseFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select a file to hash",
                CheckFileExists = true
            };

            if (dialog.ShowDialog() == true)
            {
                SetSelectedFile(dialog.FileName);
            }
        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Copy
                : DragDropEffects.None;
            e.Handled = true;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files == null || files.Length == 0)
            {
                return;
            }

            var path = files[0];
            if (Directory.Exists(path))
            {
                SetStatus("Please drop a file, not a folder.", isError: true);
                return;
            }

            SetSelectedFile(path);
        }

        private void SetSelectedFile(string path)
        {
            _selectedFilePath = path;
            FilePathTextBox.Text = path;
            FilePathTextBox.Visibility = Visibility.Visible;
            FileNameText.Text = Path.GetFileName(path);

            try
            {
                var info = new FileInfo(path);
                FileMetaText.Text = $"{FormatSize(info.Length)}  ·  {info.FullName}";
                SetStatus($"Selected {info.Name}");
            }
            catch
            {
                FileMetaText.Text = path;
                SetStatus("Selected file");
            }

            HashTextBox.Clear();
            Base64TextBox.Clear();
        }

        private async void CalculateHash_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedFilePath))
            {
                SetStatus("Please select a file first.", isError: true);
                return;
            }

            var algorithmName = GetSelectedAlgorithm();
            SetBusy(true);
            SetStatus($"Calculating {algorithmName}…");

            try
            {
                var hash = await Task.Run(() => ComputeHash(_selectedFilePath, algorithmName));
                HashTextBox.Text = hash;
                SetStatus($"{algorithmName} calculated ({hash.Length / 2} bytes)");
            }
            catch (Exception ex)
            {
                SetStatus($"Hash failed: {ex.Message}", isError: true);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void ConvertToBase64_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedFilePath))
            {
                SetStatus("Please select a file first.", isError: true);
                return;
            }

            SetBusy(true);
            SetStatus("Encoding Base64…");

            try
            {
                var path = _selectedFilePath;
                var encoded = await Task.Run(() =>
                {
                    var bytes = File.ReadAllBytes(path);
                    return Convert.ToBase64String(bytes);
                });

                Base64TextBox.Text = encoded;
                SetStatus($"Base64 ready ({encoded.Length:N0} characters)");
            }
            catch (Exception ex)
            {
                SetStatus($"Base64 failed: {ex.Message}", isError: true);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void CopyHash_Click(object sender, RoutedEventArgs e)
        {
            CopyText(HashTextBox.Text, "Hash copied to clipboard");
        }

        private void CopyBase64_Click(object sender, RoutedEventArgs e)
        {
            CopyText(Base64TextBox.Text, "Base64 copied to clipboard");
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _selectedFilePath = null;
            FilePathTextBox.Clear();
            FilePathTextBox.Visibility = Visibility.Collapsed;
            FileNameText.Text = "Drop a file here or browse to select";
            FileMetaText.Text = "No file selected";
            HashTextBox.Clear();
            Base64TextBox.Clear();
            Sha256Radio.IsChecked = true;
            SetStatus("Ready");
        }

        private void CopyText(string value, string successMessage)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                SetStatus("Nothing to copy yet.", isError: true);
                return;
            }

            Clipboard.SetText(value);
            SetStatus(successMessage);
        }

        private string GetSelectedAlgorithm()
        {
            if (Md5Radio.IsChecked == true) return "MD5";
            if (Sha1Radio.IsChecked == true) return "SHA1";
            return "SHA256";
        }

        private static string ComputeHash(string path, string algorithmName)
        {
            using (var stream = File.OpenRead(path))
            using (var algorithm = CreateAlgorithm(algorithmName))
            {
                var hash = algorithm.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        private static HashAlgorithm CreateAlgorithm(string algorithmName)
        {
            switch (algorithmName)
            {
                case "MD5":
                    return MD5.Create();
                case "SHA1":
                    return SHA1.Create();
                default:
                    return SHA256.Create();
            }
        }

        private void SetBusy(bool busy)
        {
            CalculateButton.IsEnabled = !busy;
            Base64Button.IsEnabled = !busy;
            Cursor = busy ? System.Windows.Input.Cursors.Wait : null;
        }

        private void SetStatus(string message, bool isError = false)
        {
            StatusText.Text = message;
            StatusText.Foreground = isError
                ? new SolidColorBrush(Color.FromRgb(0xB4, 0x23, 0x18))
                : (Brush)FindResource("MutedBrush");
        }

        private static string FormatSize(long bytes)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            double size = bytes;
            var unit = 0;
            while (size >= 1024 && unit < units.Length - 1)
            {
                size /= 1024;
                unit++;
            }

            return unit == 0 ? $"{bytes} {units[unit]}" : $"{size:0.##} {units[unit]}";
        }
    }
}
