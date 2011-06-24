namespace NSpec.Domain.Formatters
{
    public interface IFormatter
    {
        void Write( ContextCollection contexts );
    }
}