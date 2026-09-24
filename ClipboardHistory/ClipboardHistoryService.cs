using ClipboardHistory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Windows;

namespace ClipboardHistory
{
    public class ClipboardHistoryService
    {
        public ObservableCollection<ClipModel> History { get; } = new ObservableCollection<ClipModel>();
        int maxItem = 50;

        public void Add(ClipModel entry)
        {
            if (!History.Any(h => h.Text == entry.Text)) {
                History.Insert(0, entry);
            }

            if (History.Count > maxItem)
            {
                History.RemoveAt(History.Count - 1);
            }
        }

        public void Clear()
        {
            History.Clear();
        }

        public void Copy(ClipModel Entry)
        {
            Clipboard.SetText(Entry.Text);
        }

    }
}
