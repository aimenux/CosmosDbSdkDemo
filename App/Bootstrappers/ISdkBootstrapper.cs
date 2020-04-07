using System.Threading.Tasks;

namespace App.Bootstrappers
{
    public interface ISdkBootstrapper
    {
        string Name { get; }
        Task LaunchAsync(string[] args);
    }
}
