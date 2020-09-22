namespace Orion
{
    public interface IProvider
    {
        object GetInstance();
    }
    public interface IProvider<out T> : IProvider
    {
        T GetInstance();
    }
}