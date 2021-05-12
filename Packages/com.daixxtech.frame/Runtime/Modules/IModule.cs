namespace Frame.Runtime.Modules {
    public interface IModule {
        bool NeedUpdate { get; }

        void Init();

        void Dispose();

        void Update();
    }
}
