using System;

namespace Game.Modules.UI {
    [AttributeUsage(AttributeTargets.Class)]
    public class UIBindAttribute : Attribute {
        public readonly string name;

        public UIBindAttribute(string name) {
            this.name = name;
        }
    }
}
