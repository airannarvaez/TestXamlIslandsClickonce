using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Toolkit.Forms.UI.XamlHost;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;

namespace WindowsFormsApp11
{
    public partial class Form1 : Form
    {
        NavigationView navigation = null;

        Windows.UI.Xaml.Controls.TreeView treeView = null;

        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(1000, 640);

            CreateCalendar(new Size(150, 40), new Point(50, 50));
            CreateTimePicker(new Size(250, 35), new Point(50, 100));
            CreateTreeview(new Size(250, 600), new Point(0, 0));
            CreateMap();
        }

        private void CreateMap()
        {
            WindowsXamlManager.InitializeForCurrentThread();

            MapControl control = new MapControl
            {
                Name = "map",
                Width = PanelMap.Width,
                Height = PanelMap.Height,
                Style = MapStyle.Road,
                
            };
            control.MapElements.Add(new MapIcon
            {
                Title = "Seattle City Hall",
                Location = new Geopoint(new BasicGeoposition
                {
                    Latitude = 47.603830,
                    Longitude = -122.329900
                })
            });
            control.Center = new Geopoint(new BasicGeoposition
            {
                Latitude = 47.603830,
                Longitude = -122.329900
            });
            control.ZoomLevel = 10;

            WindowsXamlHost myHostControl = new WindowsXamlHost
            {
                Location = new Point(0, 0),
                Size = new Size(PanelMap.Width, PanelMap.Height),
                Name = "host1",
                Child = control
            };

            this.PanelMap.Controls.Add(myHostControl);
        }

        private void CreateNavigation(Size size, Point point)
        {
            WindowsXamlManager.InitializeForCurrentThread();

            navigation = new NavigationView
            {
                Width = size.Width,
                Height = 1500,
            };

            navigation.IsSettingsVisible = false;
            navigation.IsAccessKeyScope = false;
            navigation.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
            navigation.IsDoubleTapEnabled = true;
            navigation.IsPaneOpen = true;
            navigation.CanBeScrollAnchor = true;
            navigation.DoubleTapped += Navigation_DoubleTapped;
            navigation.ItemInvoked += Navigation_ItemInvoked;
            
            navigation.IsTapEnabled = false;
            
            List<NavigationViewItem> items = new List<NavigationViewItem>();
            for (int i = 0; i < 20; i++)
            {
                var item = new NavigationViewItem
                {
                    Name = "item" + i,
                    Content = "Item " + i
                };
                item.Tapped += Item_Tapped;

                if (i == 0 || i == 5 || i == 10 || i == 15 || i == 20)
                {
                    navigation.MenuItems.Add(new NavigationViewItemHeader
                    {
                        Name = "header" + i,
                        Content = "Header " + i
                    });
                }

                navigation.MenuItems.Add(item);
            }

            ScrollViewer scroll = new ScrollViewer
            {
                Name = "scroll",
                Content = navigation,
                Width = navigation.Width,
                Height = size.Height
            };

            try
            {
                WindowsXamlHost myHostControl = new WindowsXamlHost
                {
                    Location = point,
                    Size = size,
                    Name = "myHostControl",
                    Child = scroll
                };

                this.Controls.Add(myHostControl);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateTreeview(Size size, Point point)
        {
            WindowsXamlManager.InitializeForCurrentThread();

            treeView = new Windows.UI.Xaml.Controls.TreeView
            {
                Width = size.Width,
                Height = size.Height,
            };

            List<TreeViewNode> items = new List<TreeViewNode>();
            for (int i = 0; i < 20; i++)
            {
                var item = new TreeViewNode
                {
                    Content = "Item " + i
                };
                for (int c = 0; c < 20; c++)
                {
                    item.Children.Add(new TreeViewNode
                    {
                        Content = "SubItem " + c
                    });
                }
                treeView.RootNodes.Add(item);
            }
            treeView.ItemInvoked += TreeView_ItemInvoked;

            ScrollViewer scroll = new ScrollViewer
            {
                Name = "scroll",
                Content = treeView,
                Width = treeView.Width,
                Height = size.Height
            };

            try
            {
                WindowsXamlHost myHostControl = new WindowsXamlHost
                {
                    Location = point,
                    Size = size,
                    Name = "myHostControl",
                    Child = scroll
                };

                this.Controls.Add(myHostControl);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TreeView_ItemInvoked(Windows.UI.Xaml.Controls.TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            var t = args.InvokedItem as TreeViewNode;
            MessageBox.Show(t.Content.ToString());
        }

        private void Item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            navigation.IsPaneOpen = true;
        }

        private void Navigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            sender.IsPaneOpen = true;
        }

        private void Navigation_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var item = (sender as NavigationView).SelectedItem as NavigationViewItem;
            this.Text = (string)item.Content;
            navigation.IsPaneOpen = true;
        }

        private void CreateTimePicker(Size size, Point point)
        {
            WindowsXamlManager.InitializeForCurrentThread();

            TimePicker control = new TimePicker
            {
                Name = "timePicker",
                Width = size.Width,
                Height = size.Height,
                TabIndex = 0
            };
            control.TimeChanged += Control_TimeChanged;
            control.SelectedTimeChanged += Control_SelectedTimeChanged;

            WindowsXamlHost myHostControl = new WindowsXamlHost
            {
                Location = point,
                Size = size,
                Name = "host1",
                Child = control
            };

            this.PanelContent.Controls.Add(myHostControl);
        }

        private void Control_SelectedTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
        {
            //var timePicker = (TimePicker)sender;
            var message = sender.SelectedTime.Value;
            if (sender.SelectedTime != null)
                this.Text = message.ToString();
                //MessageBox.Show("Time selected: " + message.ToString());
        }

        private void Control_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            //var timePicker = (TimePicker)sender;
            //if (timePicker.SelectedTime != null)
            //    MessageBox.Show("Time selected: " + timePicker.SelectedTime.Value.ToString());
        }

        private void CreateCalendar(Size size, Point point)
        {
            // Initialize the UWP hosting environment.
            WindowsXamlManager.InitializeForCurrentThread();

            // Create a UWP control.
            CalendarDatePicker calendar = new CalendarDatePicker
            {
                Name = "calendar",
                Width = size.Width,
                Height = size.Height,
                TabIndex = 0,
                PlaceholderText = "Select Date",
                //Header = "Test Calendar"
            };
            calendar.Closed += Calendar_Closed;

            // Create a Windows XAML host control.
            WindowsXamlHost myHostControl = new WindowsXamlHost
            {
                Location = point,
                Size = size,
                Name = "host2",
                Child = calendar
            };

            // Make the UWP control appear in the UI.
            // For Windows Forms applications, you might use this.Controls.Add(myHostControl);
            this.PanelContent.Controls.Add(myHostControl);
        }

        private void Calendar_Closed(object sender, object e)
        {
            var calendar = (CalendarDatePicker)sender;
            if (calendar.Date != null)
                MessageBox.Show("Day selected: " + calendar.Date.Value.ToString("dd/MM/yyyy"));
        }

    }
}
