using Extensions;
using Managers;
using Server.Abstractions;
using Server.Consts;
using UnityEngine;
using Zenject;

namespace Server
{
    public class OwnerDisplay : MonoBehaviour
    {
        [SerializeField] private Entity entity;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private void Start()
        {
            var owner = entity.PlayerOwnerId.Value;
            var color = PlayerColors.Colors[(int)owner % PlayerColors.Colors.Length];
            spriteRenderer.color = color;
        }
    }
}