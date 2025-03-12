namespace _Project.Core.Services.UpdateService
{
    public interface IUpdatable
    {
        UpdateType UpdateType { get; }
        void Tick();
    }
}