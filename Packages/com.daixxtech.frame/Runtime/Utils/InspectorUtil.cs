using System;
using UnityEngine;

namespace Frame.Runtime.Utils {
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class InspectorReadOnlyAttribute : PropertyAttribute { }
}
