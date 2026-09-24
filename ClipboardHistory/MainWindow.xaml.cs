using ClipboardHistory.Models;
using H.NotifyIcon.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ClipboardHistory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClipboardHistoryService service = new();
        private ClipboardLogic logic = new();
        private ICollectionView view;
        private string searchText = "";
        private bool isExiting = false;


        public MainWindow()
        {
            InitializeComponent();


            this.DataContext = service.History;
            logic.ClipboardUpdate += text => service.Add(new ClipModel { Text = text });

            view = CollectionViewSource.GetDefaultView(service.History);
            view.Filter = obj =>
            {
                var clip = (ClipModel)obj;

                if (String.IsNullOrEmpty(searchText)) return true;
                else return clip.Text.Contains(searchText, StringComparison.OrdinalIgnoreCase);

            };
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!isExiting)
            {
                e.Cancel = true;  
                Hide();          
            }
            else base.OnClosing(e);

            
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            logic.Attach(this);
        }
        //

        protected override void OnClosed(EventArgs e)
        {
            logic.Detach();
            base.OnClosed(e);
        }

        private void mainListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var ItemForCopy = mainListBox.SelectedItem as ClipModel;
            if (ItemForCopy != null)
            {
                Clipboard.SetText(ItemForCopy.Text);
                copyNotif.IsEnabled = true;

                DoubleAnimation fadeanim = new DoubleAnimation()
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.6),
                    AutoReverse = true,

                };

                copyNotif.BeginAnimation(Border.OpacityProperty, fadeanim);
            }
        }   

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            service.Clear();
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchText = search.Text;
            view.Refresh();
        }

        private void trayIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
        }

        private void showClick(object sender, RoutedEventArgs e)
        {
            trayIcon_TrayMouseDoubleClick(sender, e);
        }

        private void exitClick(object sender, RoutedEventArgs e)
        {
            isExiting = true;
            Close();
        }
    }
}