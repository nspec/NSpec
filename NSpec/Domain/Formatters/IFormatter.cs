namespace NSpec.Domain.Formatters
{
    public interface IFormatter
    {
        void Write(ContextCollection contexts);
    }

    public interface ILiveFormatter
    {
        void Write(Context context);
        void Write(ExampleBase example, int level);
    }

    public class SilentLiveFormatter : ILiveFormatter, IFormatter
    {
        public void Write(Context context) {}

        public void Write(ExampleBase example, int level) { }

        public void Write(ContextCollection contexts) {}
    }
}