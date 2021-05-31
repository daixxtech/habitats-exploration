/* Auto generated code */

using Game.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Game.Config {
    /// <summary> Generate From Habitats.xlsx </summary>
    public class CHabitat {
        /// <summary> ID </summary>
        public readonly int id;
        /// <summary> 名称 </summary>
        public readonly string name;
        /// <summary> 图片 </summary>
        public readonly string image;
        /// <summary> 场景 ID </summary>
        public readonly int sceneID;
        /// <summary> 是否可用 </summary>
        public readonly bool isAvailable;
        /// <summary> 小地图图片 </summary>
        public readonly string minimapImg;

        public CHabitat(int id, string name, string image, int sceneID, bool isAvailable, string minimapImg){
            this.id = id;
            this.name = name;
            this.image = image;
            this.sceneID = sceneID;
            this.isAvailable = isAvailable;
            this.minimapImg = minimapImg;
        }

        private static Dictionary<int, CHabitat> _Dict;
        private static CHabitat[] _Array;

        public static CHabitat Get(int id) {
            return (_Dict ??= ConfUtil.LoadFromJSON<CHabitat>()).TryGetValue(id, out var conf) ? conf : null;
        }

        public static CHabitat[] GetArray() {
            return _Array ??= (_Dict ??= ConfUtil.LoadFromJSON<CHabitat>()).Values.ToArray();
        }
    }
}

/* End of auto generated code */
