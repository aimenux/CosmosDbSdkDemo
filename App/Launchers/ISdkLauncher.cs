using System.Threading.Tasks;

namespace App.Launchers
{
    public interface ISdkLauncher
    {
        string Name { get; }
        Task LaunchAsync(string[] args);
    }
}
