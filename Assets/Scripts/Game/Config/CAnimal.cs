/* Auto generated code */

using Game.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Game.Config {
    /// <summary> Generate From Animals.xlsx </summary>
    public class CAnimal {
        /// <summary> ID </summary>
        public readonly int id;
        /// <summary> 名称 </summary>
        public readonly string name;
        /// <summary> 图片 </summary>
        public readonly string image;
        /// <summary> 拉丁学名 </summary>
        public readonly string latinName;
        /// <summary> 国家动物保护等级 </summary>
        public readonly string protectionLevel;
        /// <summary> 分布区域 </summary>
        public readonly string distribution;
        /// <summary> 描述 </summary>
        public readonly string description;

        public CAnimal(int id, string name, string image, string latinName, string protectionLevel, string distribution, string description){
            this.id = id;
            this.name = name;
            this.image = image;
            this.latinName = latinName;
            this.protectionLevel = protectionLevel;
            this.distribution = distribution;
            this.description = description;
        }

        private static Dictionary<int, CAnimal> _Dict;
        private static CAnimal[] _Array;

        public static CAnimal Get(int id) {
            return (_Dict ??= ConfUtil.LoadFromJSON<CAnimal>()).TryGetValue(id, out var conf) ? conf : null;
        }

        public static CAnimal[] GetArray() {
            return _Array ??= (_Dict ??= ConfUtil.LoadFromJSON<CAnimal>()).Values.ToArray();
        }
    }
}

/* End of auto generated code */
