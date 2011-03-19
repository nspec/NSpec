namespace NSpec.Domain
{
    public delegate void before<T>(T me) where T : class, new();
}
