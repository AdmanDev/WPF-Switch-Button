using System;
using System.Windows;

namespace TestApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //this.switchBox.Checked = true;
        }

        private void SwitchBox_CheckedChanged(bool isChecked)
        {
            this.Title = "Checked : " + isChecked;
        }

    }
}
