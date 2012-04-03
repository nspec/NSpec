namespace Contrib.GoogleAnalytics.Models {
    public class SettingsRecord {
        public virtual int Id { get; set; }
        public virtual bool Enable { get; set; }
        public virtual string Script { get; set; }
    }
}