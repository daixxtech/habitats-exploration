﻿using System;
using UnityEngine;

namespace Game.Facade {
    public static class Player {
        public static Action<Collider, bool> TriggeredClue;
    }
}