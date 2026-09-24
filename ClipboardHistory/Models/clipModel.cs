
namespace ClipboardHistory.Models
{
    public class ClipModel
    {
        public string Text { get; set;}

        public DateTime CopiedAt { get; set; } = DateTime.Now;       
    }
}
