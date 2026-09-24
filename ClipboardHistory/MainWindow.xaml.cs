using System.Collections.ObjectModel;
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
using ClipboardHistory.Models;

namespace ClipboardHistory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ObservableCollection<ClipModel> clips = new ObservableCollection<ClipModel>();
            clips.Add(new ClipModel { Text = "пока" });
            clips.Add(new ClipModel { Text = "член" });
            clips.Add(new ClipModel { Text = "кек" });
            clips.Add(new ClipModel { Text = "привет" });
            clips.Add(new ClipModel { Text = "привет" });

            mainListBox.DataContext = clips;
        }

        private void mainListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var ItemForCopy = mainListBox.SelectedItem as ClipModel;
            if (ItemForCopy != null)
            {
                Clipboard.SetText(ItemForCopy.Text);
            }
        }
    }
}