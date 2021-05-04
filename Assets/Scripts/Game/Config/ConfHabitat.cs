/* Auto generated code */

using System.Collections.Generic;
using System.Linq;
using Game.Utils;

namespace Game.Config {
    /// <summary> Generate From Habitats.xlsx </summary>
    public class ConfHabitat {
        /// <summary> ID </summary>
        public readonly int id;
        /// <summary> 名称 </summary>
        public readonly string name;
        /// <summary> 图标 </summary>
        public readonly string icon;
        /// <summary> 场景名称 </summary>
        public readonly string sceneName;
        /// <summary> 是否可用 </summary>
        public readonly bool isAvailable;

        public ConfHabitat(int id, string name, string icon, string sceneName, bool isAvailable) {
            this.id = id;
            this.name = name;
            this.icon = icon;
            this.sceneName = sceneName;
            this.isAvailable = isAvailable;
        }

        private static Dictionary<int, ConfHabitat> _Dict;
        private static ConfHabitat[] _Array;

        public static ConfHabitat Get(int id) {
            return (_Dict ?? (_Dict = ConfUtil.LoadFromJSON<ConfHabitat>())).TryGetValue(id, out var conf) ? conf : null;
        }

        public static ConfHabitat[] GetArray() {
            return _Array ?? (_Array = (_Dict ?? (_Dict = ConfUtil.LoadFromJSON<ConfHabitat>())).Values.ToArray());
        }
    }
}

/* End of auto generated code */
