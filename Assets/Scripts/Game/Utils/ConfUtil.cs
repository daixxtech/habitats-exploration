using Frame.Runtime.Modules;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils {
    public static class ConfUtil {
        public static Dictionary<int, T> LoadFromJSON<T>() {
            TextAsset textAsset = ResourceModule.Instance.LoadRes<TextAsset>(typeof(T).Name + ".json");
            return textAsset ? JsonConvert.DeserializeObject<Dictionary<int, T>>(textAsset.text) : new Dictionary<int, T>();
        }
    }
}
