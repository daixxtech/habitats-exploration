using System;
using UnityEngine;

namespace Game.Utils {
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class InspectorReadOnlyAttribute : PropertyAttribute { }
}
