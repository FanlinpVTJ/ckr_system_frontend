using UnityEngine;
using ValueSystem.Base;
using Zenject;

namespace ValueSystem.Zenject
{
    [CreateAssetMenu(fileName = "Values Installer", menuName = "Installers/Values Installer")]
    public class ValuesInstaller : ScriptableObjectInstaller<ValuesInstaller>
    {
        [SerializeField]
        private ValueData[] _valueDatas;

        public ValueData[] Values => _valueDatas;

        public override void InstallBindings()
        {
            Container.Bind<IValueSystem>().To<ValueSystemLogic>().AsSingle().WithArguments(_valueDatas).NonLazy();
        }
    }
}
