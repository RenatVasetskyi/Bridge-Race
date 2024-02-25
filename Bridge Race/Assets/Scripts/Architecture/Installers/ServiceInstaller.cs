using Architecture.Services;
using Architecture.Services.Interfaces;
using Data;
using UnityEngine;
using Zenject;

namespace Architecture.Installers
{
    public class ServiceInstaller : MonoInstaller, ICoroutineRunner
    {
        [SerializeField] private GameSettings _gameSettings;
        
        public override void InstallBindings()
        {
            BindGameSettings();
            BindCoroutineRunner();
            BindSceneLoader();
            BindAssetProvider();
            BindBaseFactory();
            BindUIFactory();
            BindSaveService();
            BindAudioService();
            BindCurrencyService();
            BindGamePauser();
            BindLevelProgressService();
            BindUserDataStorage();
        }
        
        private void BindUserDataStorage()
        {
            Container
                .Bind<IUserDataStorage>()
                .To<UserDataStorage>()
                .AsSingle();
        }
        
        private void BindLevelProgressService()
        {
            Container
                .Bind<ILevelProgressService>()
                .To<LevelProgressService>()
                .AsSingle();
        }
        
        private void BindGamePauser()
        {
            Container
                .Bind<IGamePauser>()
                .To<GamePauser>()
                .AsSingle();
        }
        
        private void BindCurrencyService()
        {
            Container
                .Bind<ICurrencyService>()
                .To<CurrencyService>()
                .AsSingle();
        }

        private void BindGameSettings()
        {
            Container
                .Bind<GameSettings>()
                .FromScriptableObject(_gameSettings)
                .AsSingle();
        }

        private void BindSaveService()
        {
            Container
                .Bind<ISaveService>()
                .To<SaveService>()
                .AsSingle();
        }
        
        private void BindAudioService()
        {
            Container
                .Bind<IAudioService>()
                .To<AudioService>()
                .AsSingle()
                .NonLazy();
        }
        
        private void BindUIFactory()
        {
            Container
                .Bind<IUIFactory>()
                .To<UIFactory>()
                .AsSingle();
        }
        
        private void BindBaseFactory()
        {
            Container
                .Bind<IBaseFactory>()
                .To<BaseFactory>()
                .AsSingle();
        }

        private void BindCoroutineRunner()
        {
            Container
                .BindInterfacesTo<ServiceInstaller>()
                .FromInstance(this)
                .AsSingle()
                .NonLazy();
        }

        private void BindSceneLoader()
        {
            Container
                .Bind<ISceneLoader>()
                .To<SceneLoader>()
                .AsSingle()
                .NonLazy();
        }

		private void BindAssetProvider()
        {
            Container
                .Bind<IAssetProvider>()
                .To<AssetProvider>()
                .AsSingle();
        }
    }
}