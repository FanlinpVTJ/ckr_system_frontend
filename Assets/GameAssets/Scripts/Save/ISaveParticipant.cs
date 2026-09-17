using Cysharp.Threading.Tasks;

namespace CkrSystem.Save
{
    public interface ISaveParticipant
    {
        UniTask LoadAsync();
        UniTask SaveAsync();
    }
}
