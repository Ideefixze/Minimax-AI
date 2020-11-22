using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Player;
using UnityEngine;

namespace BoardGame
{
    public class Player
    {
        public int Id { get; private set; }

        public Player(int id)
        {
            Id = id;
        }

        public static bool operator==(Player a, Player b)
        {
            return a.Id == b.Id;
        }

        public static bool operator!=(Player a, Player b)
        {
            return a.Id != b.Id;
        }
    }
}

