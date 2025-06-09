using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using MvvmHelpers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TelerikTest
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            this.InitializeComponent();

            ContentFrame.Navigate(typeof(HomePage));
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigated += On_Navigated;
            MainNavigationMenu.SelectedItem = MainNavigationMenu.MenuItems[0];
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Parameter != null &&
                e.Parameter is bool firstNav &&
                firstNav &&
                ContentFrame.BackStackDepth > 0)
            {
                ContentFrame.BackStack.RemoveAt(ContentFrame.BackStackDepth - 1);
            }

            MainNavigationMenu.IsBackEnabled = ContentFrame.CanGoBack;

            if (ContentFrame.SourcePageType != null)
            {
                // Use a recursive search function to handle nested MenuItems
                MainNavigationMenu.SelectedItem = FindNavigationViewItemWithTag(ContentFrame.SourcePageType.FullName);
            }
        }

        private NavigationViewItem FindNavigationViewItemWithTag(string pageTypeFullName)
        {
            // Check top-level MenuItems first
            foreach (var item in MainNavigationMenu.MenuItems.OfType<NavigationViewItem>())
            {
                var foundItem = FindItemInMenuItem(item, pageTypeFullName);
                if (foundItem != null) return foundItem;
            }

            // No match found
            return null;
        }

        private NavigationViewItem FindItemInMenuItem(NavigationViewItem item, string pageTypeFullName)
        {
            if (item.Tag?.ToString() == pageTypeFullName) return item;

            // Recursively check nested MenuItems if any
            foreach (var subItem in item.MenuItems.OfType<NavigationViewItem>())
            {
                var foundItem = FindItemInMenuItem(subItem, pageTypeFullName);
                if (foundItem != null) return foundItem;
            }

            return null;
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer != null && args.InvokedItemContainer.Tag != null)
            {
                Type navPageType = Type.GetType(args.InvokedItemContainer.Tag.ToString());
                if (navPageType != null && ContentFrame.CurrentSourcePageType != navPageType)
                {
                    NavigationView_Navigate(navPageType, args.RecommendedNavigationTransitionInfo);
                }
            }
        }

        private void NavigationView_Navigate(Type navPageType, NavigationTransitionInfo transitionInfo, object parameter = null)
        {
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            Type preNavPageType = ContentFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (navPageType is not null && !Type.Equals(preNavPageType, navPageType))
            {
                ContentFrame.Navigate(navPageType, parameter, transitionInfo);
            }
        }

        private void NavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            TryGoBack();
        }

        private bool TryGoBack()
        {
            if (!ContentFrame.CanGoBack)
            {
                MainNavigationMenu.IsBackEnabled = ContentFrame.CanGoBack;
                return false;
            }

            // Don't go back if the nav pane is overlayed.
            if (MainNavigationMenu.IsPaneOpen &&
                (MainNavigationMenu.DisplayMode == NavigationViewDisplayMode.Compact ||
                 MainNavigationMenu.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;

            ContentFrame.GoBack();
            return true;
        }
    }
}
