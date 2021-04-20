/* Auto generated code */

using System.Collections.Generic;
using System.Linq;
using Game.Utils;

namespace Game.Config {
    /// <summary> Generate From Animals.xlsx </summary>
    public class ConfAnimal {
        /// <summary> ID </summary>
        public readonly int id;
        /// <summary> 名称 </summary>
        public readonly string name;
        /// <summary> 图标 </summary>
        public readonly string icon;
        /// <summary> 拉丁学名 </summary>
        public readonly string latinName;
        /// <summary> 分布区域 </summary>
        public readonly string distribution;
        /// <summary> 描述 </summary>
        public readonly string description;

        public ConfAnimal(int id, string name, string icon, string latinName, string distribution, string description) {
            this.id = id;
            this.name = name;
            this.icon = icon;
            this.latinName = latinName;
            this.distribution = distribution;
            this.description = description;
        }

        private static Dictionary<int, ConfAnimal> _Dict;
        private static ConfAnimal[] _Array;

        public static ConfAnimal Get(int id) {
            return (_Dict ?? (_Dict = ConfUtil.LoadFromJSON<ConfAnimal>())).TryGetValue(id, out var conf) ? conf : null;
        }

        public static ConfAnimal[] GetArray() {
            return _Array ?? (_Array = (_Dict ?? (_Dict = ConfUtil.LoadFromJSON<ConfAnimal>())).Values.ToArray());
        }
    }
}

/* End of auto generated code */
