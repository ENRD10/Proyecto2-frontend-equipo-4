using CommunityToolkit.Mvvm.ComponentModel;
using PosManagerApp.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace PosManagerApp.ViewModels
{
    public partial class ViewModel : ObservableObject
    {
        protected void HandleApiException(ApiException ex, string title)
        {
            Debug.WriteLine(ex);
            MessageBox.Show(
                ex.Message,
                title,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        protected void HandleError(Exception ex, string title)
        {
            Debug.WriteLine(ex);
            var message = string.IsNullOrWhiteSpace(ex.Message) ? "Ocurrió un error inesperado." : ex.Message;
            MessageBox.Show(
                message,
                title,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
