using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Player;
using UnityEngine;

namespace BoardGame
{
    public class Player
    {
        private int _id;
        public int id { get => _id;}

        public Player(int id)
        {
            _id = id;
        }

        public static bool operator==(Player a, Player b)
        {
            return a.id == b.id;
        }

        public static bool operator!=(Player a, Player b)
        {
            return a.id != b.id;
        }
    }
}

