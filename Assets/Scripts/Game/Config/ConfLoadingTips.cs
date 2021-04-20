/* Auto generated code */

using System.Collections.Generic;
using System.Linq;
using Game.Utils;

namespace Game.Config {
    /// <summary> Generate From LoadingTips.xlsx </summary>
    public class ConfLoadingTips {
        /// <summary> ID </summary>
        public readonly int id;
        /// <summary> 内容 </summary>
        public readonly string content;

        public ConfLoadingTips(int id, string content) {
            this.id = id;
            this.content = content;
        }

        private static Dictionary<int, ConfLoadingTips> _Dict;
        private static ConfLoadingTips[] _Array;

        public static ConfLoadingTips Get(int id) {
            return (_Dict ?? (_Dict = ConfUtil.LoadFromJSON<ConfLoadingTips>())).TryGetValue(id, out var conf) ? conf : null;
        }

        public static ConfLoadingTips[] GetArray() {
            return _Array ?? (_Array = (_Dict ?? (_Dict = ConfUtil.LoadFromJSON<ConfLoadingTips>())).Values.ToArray());
        }
    }
}

/* End of auto generated code */
