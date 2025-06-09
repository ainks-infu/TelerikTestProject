using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TelerikTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RadDataGridPage : Page
    {
        public ObservableRangeCollection<TestObject> TestObjects { get; } = [];

        public RadDataGridPage()
        {
            TestObjects.Add(new() { Name = "Test", Code = "TEST001" });
            TestObjects.Add(new() { Name = "Test2", Code = "TEST002" });

            this.InitializeComponent();
        }
    }
}
