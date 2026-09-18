using UnityEngine;
using WindowsManager;
using Zenject;

namespace FeaturedClicker.Dogs
{
    public class DogBreedDetailsWindowInstaller : MonoInstaller
    {
        [SerializeField] private WindowData _dogBreedDetailsWindowData;

        public override void InstallBindings()
        {
            Container.Bind<IDogBreedDetailsWindowService>()
                .To<DogBreedDetailsWindowService>()
                .AsSingle()
                .WithArguments(_dogBreedDetailsWindowData);
        }
    }
}
