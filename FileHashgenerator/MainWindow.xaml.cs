using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;

namespace FileHashgenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _selectedFilePath;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void BrowseFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _selectedFilePath = openFileDialog.FileName;
                FilePathTextBox.Text = _selectedFilePath;
                HashTextBox.Clear();
                Base64TextBox.Clear();
            }
        }
        private async void CalculateHash_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedFilePath))
            {
                MessageBox.Show("Please select a file first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (FileStream stream = File.OpenRead(_selectedFilePath))
                {
                    HashAlgorithm hashAlgorithm;
                    string selectedAlgorithm = (HashAlgorithmComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                    switch (selectedAlgorithm)
                    {
                        case "MD5":
                            hashAlgorithm = MD5.Create();
                            break;
                        case "SHA1":
                            hashAlgorithm = SHA1.Create();
                            break;
                        case "SHA256":
                        default:
                            hashAlgorithm = SHA256.Create();
                            break;
                    }

                    using (hashAlgorithm)
                    {
                        byte[] hash = hashAlgorithm.ComputeHash(stream);
                        HashTextBox.Text = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating hash: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ConvertToBase64_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedFilePath))
            {
                MessageBox.Show("Please select a file first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                byte[] fileBytes = File.ReadAllBytes(_selectedFilePath);
                Base64TextBox.Text = Convert.ToBase64String(fileBytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error converting to Base64: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
