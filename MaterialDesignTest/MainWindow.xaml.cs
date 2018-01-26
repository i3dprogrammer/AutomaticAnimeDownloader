﻿using MaterialDesignTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaterialDesignTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SettingsViewModel settingsViewModel;
        private AboutViewModel aboutViewModel;

        public MainWindow()
        {
            InitializeComponent();

            settingsViewModel = new SettingsViewModel(new Models.Settings());
            aboutViewModel = new AboutViewModel(new Models.Todo());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(!leftDrawer.IsLeftDrawerOpen && e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
        }

        private void Configure_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((MainWindowViewModel)DataContext).CurrentViewModel = settingsViewModel;
            leftDrawer.IsLeftDrawerOpen = false;
        }

        private void About_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((MainWindowViewModel)DataContext).CurrentViewModel = aboutViewModel;
            leftDrawer.IsLeftDrawerOpen = false;
        }
    }
}
