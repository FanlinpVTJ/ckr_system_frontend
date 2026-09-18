using Cysharp.Threading.Tasks;

namespace FeaturedClicker.Save
{
    public interface ISaveParticipant
    {
        UniTask LoadAsync();
        UniTask SaveAsync();
    }
}
