/* Auto generated code */

using Game.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Game.Config {
    /// <summary> Generate From Clues.xlsx </summary>
    public class CClue {
        /// <summary> ID </summary>
        public readonly int id;
        /// <summary> 名称 </summary>
        public readonly string name;
        /// <summary> 所属栖息地 ID </summary>
        public readonly int habitatID;
        /// <summary> 线索描述 </summary>
        public readonly string description;

        public CClue(int id, string name, int habitatID, string description){
            this.id = id;
            this.name = name;
            this.habitatID = habitatID;
            this.description = description;
        }

        private static Dictionary<int, CClue> _Dict;
        private static CClue[] _Array;

        public static CClue Get(int id) {
            return (_Dict ??= ConfUtil.LoadFromJSON<CClue>()).TryGetValue(id, out var conf) ? conf : null;
        }

        public static CClue[] GetArray() {
            return _Array ??= (_Dict ??= ConfUtil.LoadFromJSON<CClue>()).Values.ToArray();
        }
    }
}

/* End of auto generated code */
