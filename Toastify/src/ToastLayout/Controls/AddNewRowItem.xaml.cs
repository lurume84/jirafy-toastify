using System;
using System.Windows;

namespace Toastify.ToastLayout.Controls
{
    public partial class AddNewRowItem
    {
        public event EventHandler<RoutedEventArgs> BtnAddClicked;

        public AddNewRowItem()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            this.BtnAddClicked?.Invoke(sender, e);
        }
    }
}