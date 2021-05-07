/* Auto generated code */

using System.Collections.Generic;
using System.Linq;
using Game.Utils;

namespace Game.Config {
    /// <summary> Generate From Scenes.xlsx </summary>
    public enum ESceneDef {
        /// <summary> 开始 </summary>
        Start = 1,
        /// <summary> 大熊猫栖息地 </summary>
        Habitat_Panda = 2,
    }

    /// <summary> Generate From Scenes.xlsx </summary>
    public class ConfSceneDef {
        /// <summary> ID </summary>
        public readonly int id;
        /// <summary> 名称 </summary>
        public readonly string name;

        public ConfSceneDef(int id, string name) {
            this.id = id;
            this.name = name;
        }

        private static Dictionary<int, ConfSceneDef> _Dict;
        private static ConfSceneDef[] _Array;

        public static ConfSceneDef Get(int id) {
            return (_Dict ?? (_Dict = ConfUtil.LoadFromJSON<ConfSceneDef>())).TryGetValue(id, out var conf) ? conf : null;
        }

        public static ConfSceneDef[] GetArray() {
            return _Array ?? (_Array = (_Dict ?? (_Dict = ConfUtil.LoadFromJSON<ConfSceneDef>())).Values.ToArray());
        }
    }
}

/* End of auto generated code */
