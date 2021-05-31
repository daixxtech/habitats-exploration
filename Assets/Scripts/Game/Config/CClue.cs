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
        /// <summary> 图片 </summary>
        public readonly string image;
        /// <summary> 所属动物 ID </summary>
        public readonly int animalID;
        /// <summary> 所属栖息地 ID </summary>
        public readonly int habitatID;
        /// <summary> 线索描述 </summary>
        public readonly string description;
        /// <summary> 位置 </summary>
        public readonly float[] position;

        public CClue(int id, string name, string image, int animalID, int habitatID, string description, float[] position){
            this.id = id;
            this.name = name;
            this.image = image;
            this.animalID = animalID;
            this.habitatID = habitatID;
            this.description = description;
            this.position = position;
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
