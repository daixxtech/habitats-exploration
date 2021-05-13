using System;

namespace Frame.Runtime.Modules.Scenes {
    [AttributeUsage(AttributeTargets.Class)]
    public class SceneBindAttribute : Attribute {
        public readonly string name;

        public SceneBindAttribute(string name) {
            this.name = name;
        }
    }
}
