/* Auto generated code */

using Game.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Game.Config {
    /// <summary> Generate From LoadingTips.xlsx </summary>
    public class CLoadingTips {
        /// <summary> ID </summary>
        public readonly int id;
        /// <summary> 内容 </summary>
        public readonly string content;

        public CLoadingTips(int id, string content){
            this.id = id;
            this.content = content;
        }

        private static Dictionary<int, CLoadingTips> _Dict;
        private static CLoadingTips[] _Array;

        public static CLoadingTips Get(int id) {
            return (_Dict ??= ConfUtil.LoadFromJSON<CLoadingTips>()).TryGetValue(id, out var conf) ? conf : null;
        }

        public static CLoadingTips[] GetArray() {
            return _Array ??= (_Dict ??= ConfUtil.LoadFromJSON<CLoadingTips>()).Values.ToArray();
        }
    }
}

/* End of auto generated code */
