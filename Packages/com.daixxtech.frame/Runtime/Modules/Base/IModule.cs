namespace Frame.Runtime.Modules.Base {
    public interface IModule {
        bool NeedUpdate { get; }

        void Init();

        void Dispose();

        void Update();
    }
}
