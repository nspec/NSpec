namespace NSpec.Domain.Formatters
{
    public interface IFormatter
    {
        void Write( ContextCollection contexts );
    }

    public interface ILiveFormatter
    {
        void Write(Context context);
        void Write(Example example, int level);
    }
}