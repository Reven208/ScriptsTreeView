using System;
using System.Collections.Generic;
using System.IO;
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

namespace ScriptTreeViewIcon
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileSystemWatcher fsw;
        public MainWindow()
        {
            InitializeComponent();
            try { watch(); } catch { }
        }

        /* Hello, Here Is the Region
         * You can Change the Icons, (Folder PNGS)
         */
        #region TreeViewCode
        private void watch()
        {
            ScriptsView.Items.Clear();
            ScriptsView.Items.Add((object)CreateDirectoryNode(new DirectoryInfo("./Scripts")));
            fsw = new FileSystemWatcher("./Scripts" + "*.*");
        }


        private TreeViewItem GetTreeView(string tag, string text, string imagePath)
        {
            TreeViewItem treeViewItem = new TreeViewItem();
            treeViewItem.Tag = tag;
            treeViewItem.Style = TryFindResource("TreeItem") as Style;
            treeViewItem.IsExpanded = true;

            StackPanel stackPanel = new StackPanel();
            stackPanel.CanHorizontallyScroll = false;
            stackPanel.Orientation = Orientation.Horizontal;
            Image image = new Image();
            image.Source = (ImageSource)new BitmapImage(new Uri("pack://application:,,/Pngs/" + imagePath));
            image.Width = 16.0;
            image.Height = 16.0;
            RenderOptions.SetBitmapScalingMode((DependencyObject)image, BitmapScalingMode.HighQuality);

            Label label = new Label();
            label.Content = text;
            label.Foreground = (Brush)new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FFB4B4B4"));
            label.FontFamily = treeViewItem.FontFamily;
            stackPanel.Children.Add((UIElement)image);
            stackPanel.Children.Add((UIElement)label);
            treeViewItem.Header = stackPanel;
            ToolTipService.SetIsEnabled((DependencyObject)treeViewItem, false);
            return treeViewItem;
        }


        private TreeViewItem CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            TreeViewItem treeView = GetTreeView(directoryInfo.FullName, directoryInfo.Name, "Folder.png");
            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
                treeView.Items.Add(CreateDirectoryNode(directory));
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (file.Extension == ".lua")
                    treeView.Items.Add(GetTreeView(file.FullName, file.Name, "Script.png"));
                else if (file.Extension == ".txt")
                    treeView.Items.Add(GetTreeView(file.FullName, file.Name, "Script.png"));


            }
            return treeView;
        }

        private void SelectedItem(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (ScriptsView.SelectedItem == null)
                    return;
                TreeViewItem selectedItem = ScriptsView.SelectedItem as TreeViewItem;
                string str = selectedItem.Tag.ToString();

                //if is .txt or .lua but no is a folder
                if (str.EndsWith(".txt") || str.EndsWith(".lua") && !selectedItem.ToolTip.ToString().EndsWith("Folder.png"))
                {
                    StreamReader streamReader = new StreamReader(selectedItem.Tag.ToString());
                    //Code for Get Text From Script ->   streamReader.ReadToEnd(), "Script"
                    //example with messagebox
                    MessageBox.Show(streamReader.ReadToEnd(), "Script");
                }

            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.ToString());
            }
        }
        #endregion

    }
}
/* Discord:
 * Reven#1618 */