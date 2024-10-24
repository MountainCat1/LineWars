using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Game Server Settings", menuName = "Custom/Game Server Settings")]
public class GameServerSettings : ScriptableObject
{
    [field: SerializeField] public int MaxRounds { get; set; }
    [field: SerializeField] public int MaxPlayers { get; set; } = 3;
    [field: SerializeField] public int DelayToStartGameFromLobby { get; set; }

}