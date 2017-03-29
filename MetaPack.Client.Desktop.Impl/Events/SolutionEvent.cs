using MetaPack.Client.Desktop.Impl.Data;

namespace MetaPack.Client.Desktop.Impl.Events
{
    public enum SolutionEventType
    {
        New,
        Updated,
        Opened,
        Saved
    }

    public class SolutionEvent
    {
        public SolutionEventType EventType { get; set; }
        public MetaPackSolution Item { get; set; }
    }
}
