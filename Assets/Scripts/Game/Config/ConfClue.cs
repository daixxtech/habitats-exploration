/* Auto generated code */

using System.Collections.Generic;
using System.Linq;
using Game.Utils;

namespace Game.Config {
    /// <summary> Generate From Clues.xlsx </summary>
    public class ConfClue {
        /// <summary> ID </summary>
        public readonly int id;
        /// <summary> 名称 </summary>
        public readonly string name;
        /// <summary> 动物 ID </summary>
        public readonly int animalID;
        /// <summary> 线索描述 </summary>
        public readonly string description;

        public ConfClue(int id, string name, int animalID, string description) {
            this.id = id;
            this.name = name;
            this.animalID = animalID;
            this.description = description;
        }

        private static Dictionary<int, ConfClue> _Dict;
        private static ConfClue[] _Array;

        public static ConfClue Get(int id) {
            return (_Dict ?? (_Dict = ConfUtil.LoadFromJSON<ConfClue>())).TryGetValue(id, out var conf) ? conf : null;
        }

        public static ConfClue[] GetArray() {
            return _Array ?? (_Array = (_Dict ?? (_Dict = ConfUtil.LoadFromJSON<ConfClue>())).Values.ToArray());
        }
    }
}

/* End of auto generated code */
