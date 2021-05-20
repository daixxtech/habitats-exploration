/* Auto generated code */

using Game.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Game.Config {
    /// <summary> Generate From Scenes.xlsx </summary>
    public class CScene {
        /// <summary> ID </summary>
        public readonly int id;
        /// <summary> 名称 </summary>
        public readonly string name;

        public CScene(int id, string name){
            this.id = id;
            this.name = name;
        }

        private static Dictionary<int, CScene> _Dict;
        private static CScene[] _Array;

        public static CScene Get(int id) {
            return (_Dict ??= ConfUtil.LoadFromJSON<CScene>()).TryGetValue(id, out var conf) ? conf : null;
        }

        public static CScene[] GetArray() {
            return _Array ??= (_Dict ??= ConfUtil.LoadFromJSON<CScene>()).Values.ToArray();
        }
    }
}

/* End of auto generated code */
