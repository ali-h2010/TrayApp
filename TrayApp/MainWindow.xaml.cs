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
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Interop;
using Microsoft.Toolkit.Uwp.Notifications;
using Notifications.Wpf;
using TrayApp.Models;

namespace TrayApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NotifyIcon TrayIcon = new NotifyIcon();

        public MainWindow()
        {
            InitializeComponent();
            this.Hide_Window();

            var contextMenuStrip = new ContextMenuStrip();

            // Toggle Show Menu Item
            var toggleShowMenuItem = new ToolStripMenuItem
            {
                Text = "Show",
            };

            toggleShowMenuItem.Click += (e, s) =>
            {
                var coor = GetMousePositionWindowsForms();
                this.Show_Window(coor.X);
            };

            // Exit Menu Item
            var exitMenuItem = new ToolStripMenuItem
            {
                Text = "Exit",
            };

            exitMenuItem.Click += (e, s) =>
            {
                System.Windows.Application.Current.Shutdown();
                //test
            };


            contextMenuStrip.Items.Add(toggleShowMenuItem);
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add(exitMenuItem);
            TrayIcon.Icon = new Icon(@"Resources/Icon.ico");
            TrayIcon.Visible = true;
            TrayIcon.Text = "Tray Application";
            TrayIcon.MouseClick += TrayIcon_MouseClick;
            TrayIcon.ContextMenuStrip = contextMenuStrip;



            

            List<User> items = new List<User>();
            items.Add(new User() { Name = "John Doe", Age = 42, Mail = "john@doe-family.com" });
            items.Add(new User() { Name = "Jane Doe", Age = 39, Mail = "jane@doe-family.com" });
            items.Add(new User() { Name = "Sammy Doe", Age = 13, Mail = "sammy.doe@gmail.com" });

            MOMList.ItemsSource = items;
        }

        private void TrayIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var coor = GetMousePositionWindowsForms();
            this.Show_Window(coor.X);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Hide_Window();
        }
        private void Hide_Window()
        {
            this.Hide();
        }
        private void Show_Window(double cursorX)
        {
            this.AdjustWindowPosition(cursorX);
            this.Show();
        }
        private void AdjustWindowPosition(double cursorX)
        {
            Screen sc = Screen.FromHandle(new WindowInteropHelper(this).Handle);
            if (sc.WorkingArea.Top > 0)
            {
                Rect desktopWorkingArea = SystemParameters.WorkArea;
                var middleOfWindow = desktopWorkingArea.Right - (Width / 2);
                var gapToMiddle = middleOfWindow - cursorX;
                if (gapToMiddle < 0) gapToMiddle = 0;
                Left = desktopWorkingArea.Right - Width - gapToMiddle;
                Top = desktopWorkingArea.Top;
            }

            else if ((sc.Bounds.Height - sc.WorkingArea.Height) > 0)
            {
                Rect desktopWorkingArea = SystemParameters.WorkArea;
                var middleOfWindow = desktopWorkingArea.Right - (Width / 2);
                var gapToMiddle = middleOfWindow - cursorX;
                if (gapToMiddle < 0) gapToMiddle = 0;
                Left = desktopWorkingArea.Right - Width - gapToMiddle;
                Top = desktopWorkingArea.Bottom - Height;
            }
            else
            {
                Rect desktopWorkingArea = SystemParameters.WorkArea;
                Left = desktopWorkingArea.Right - Width;
                Top = desktopWorkingArea.Bottom - Height;
            }
        }

        public static System.Windows.Point GetMousePositionWindowsForms()
        {
            var point = System.Windows.Forms.Control.MousePosition;
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            var pixelX = (int)((96 / g.DpiX) * point.X);
            var pixelY = (int)((96 / g.DpiY) * point.X);
            return new System.Windows.Point(pixelX, pixelY);
        }

        private readonly Random _random = new Random();

        private void Button_Click_info(object sender, RoutedEventArgs e)
        {
            var notificationManager = new NotificationManager();

            notificationManager.Show(new NotificationContent
            {
                Title = "Sample notification",
                Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Type = (NotificationType)_random.Next(0, 4)
            });


        }

        private void Button_Click_Native_Notification(object sender, RoutedEventArgs e)
        {
            new ToastContentBuilder()
            .AddArgument("action", "viewConversation")
            .AddArgument("conversationId", 9813)
            .AddText("Andrew sent you a picture")
            .AddText("Check this out, The Enchantments in Washington!")
            .AddHeroImage(new Uri("https://via.placeholder.com/364x180"))
            .AddAppLogoOverride(new Uri("https://lh3.googleusercontent.com/proxy/CuzRQj9bkdiF_reupbs4q6o1x9EVIR1y5FEqTOMQQXTBThtvWniy0mUeP_IXo1aNR1O-1FXC49gt_mlhmu3PiVwMNVaO60Gt9yI"))
            .AddButton("test", ToastActivationType.Foreground, "test")
            .Show();
        }
    }
}
