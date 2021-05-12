using System;
using UnityEngine;

namespace FrameworkRuntime.Utils {
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class InspectorReadOnlyAttribute : PropertyAttribute { }
}
