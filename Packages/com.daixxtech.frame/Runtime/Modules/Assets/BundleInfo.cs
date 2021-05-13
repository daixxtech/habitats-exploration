namespace Frame.Runtime.Modules.Assets {
    public class BundleInfo {
        public readonly string name;
        public readonly string[] assets;
        public readonly string[] dependencies;

        public BundleInfo(string name, string[] assets, string[] dependencies) {
            this.name = name;
            this.assets = assets;
            this.dependencies = dependencies;
        }
    }
}
