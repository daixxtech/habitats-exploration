namespace FrameworkRuntime.Modules.Resource {
    public class BundleInfo {
        public string Name { get; }
        public string[] Assets { get; }
        public string[] Dependencies { get; }

        public BundleInfo(string name, string[] assets, string[] dependencies) {
            Name = name;
            Assets = assets;
            Dependencies = dependencies;
        }
    }
}
