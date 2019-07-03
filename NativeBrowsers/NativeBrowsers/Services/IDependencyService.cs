namespace NativeBrowsers.Services
{
    public interface IDependencyService
    {
        T Get<T>() where T : class;
    }
}
