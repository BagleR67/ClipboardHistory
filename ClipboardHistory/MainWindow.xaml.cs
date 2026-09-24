using ClipboardHistory.Models;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
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




        public MainWindow()
        {
            InitializeComponent();


            this.DataContext = service.History;
            logic.ClipboardUpdate += text => service.Add(new ClipModel { Text = text });
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
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            service.Clear();
        }
    }
}