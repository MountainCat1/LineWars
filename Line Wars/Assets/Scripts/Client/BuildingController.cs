using System;
using Extensions;
using Managers;
using Server;
using Server.Exceptions;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Client
{
    public class BuildingController : NetworkBehaviour
    {
        [Inject] private IBuildingManager _buildingManager;

        private void Start()
        {
            this.Inject();
        }

        public void Build(Building building, Vector2 position)
        {
            if (!IsOwner)
                throw new MustBeAnOwnerException();

            BuildServerRpc(building.name, position);
        }

        #region Server Logic

        [ServerRpc]
        public void BuildServerRpc(string buildingName, Vector2 position)
        {
            _buildingManager.Build(Vector2Int.RoundToInt(position), buildingName, OwnerClientId);
        }

        #endregion
    }
}