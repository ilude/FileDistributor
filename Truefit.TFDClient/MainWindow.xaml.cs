using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rewrite.SuperNotifyIcon;
using DragDropEffects = System.Windows.Forms.DragDropEffects;
using DragEventArgs = System.Windows.Forms.DragEventArgs;

namespace Truefit.TFDClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Hide();

            var ni = new SuperNotifyIcon();
            ni.NotifyIcon.Icon = new System.Drawing.Icon("Upload.ico");
            ni.NotifyIcon.Text = "Yo Mama";
            ni.NotifyIcon.Visible = true;

            ni.InitDrop();

            ni.DragEnter += ni_DragEnter;
            ni.DragDrop += ni_DragDrop;

            Closing += (sender, e) => ni.Dispose();


           
        }

        private static void ni_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private static void ni_DragDrop(object sender, DragEventArgs e) {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files) Console.WriteLine(file);
        }
    }
}
