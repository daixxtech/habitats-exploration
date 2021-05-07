/* Auto generated code */

using System.Collections.Generic;
using System.Linq;
using Game.Utils;

namespace Game.Config {
    /// <summary> Generate From Scenes.xlsx </summary>
    public class ConfScene {
        /// <summary> ID </summary>
        public readonly int id;
        /// <summary> 名称 </summary>
        public readonly string name;
        /// <summary> 场景是否可暂停 </summary>
        public readonly bool canPause;

        public ConfScene(int id, string name, bool canPause) {
            this.id = id;
            this.name = name;
            this.canPause = canPause;
        }

        private static Dictionary<int, ConfScene> _Dict;
        private static ConfScene[] _Array;

        public static ConfScene Get(int id) {
            return (_Dict ?? (_Dict = ConfUtil.LoadFromJSON<ConfScene>())).TryGetValue(id, out var conf) ? conf : null;
        }

        public static ConfScene[] GetArray() {
            return _Array ?? (_Array = (_Dict ?? (_Dict = ConfUtil.LoadFromJSON<ConfScene>())).Values.ToArray());
        }
    }
}

/* End of auto generated code */
