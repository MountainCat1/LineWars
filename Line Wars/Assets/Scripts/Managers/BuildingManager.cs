using System.Collections.Generic;
using Abstractions;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Managers
{
    public interface IBuildingManager
    {
        void Build(Vector2Int position, string buildingName, ulong ownerClientId);
    }

    public class BuildingManager : MonoBehaviour, IBuildingManager
    {
        private readonly List<Building> _buildings = new();

        [SerializeField] private NetworkPrefabsList networkPrefabsList;
        
        [Inject]
        private void Construct()
        {
            var prefabs = networkPrefabsList.PrefabList;
            foreach (var networkPrefab in prefabs)
            {
                var script = networkPrefab.Prefab.gameObject.GetComponent<Building>();
                if (script != null)
                {
                    _buildings.Add(script);
                }
            }
        }

        public void Build(Vector2Int position, string buildingName, ulong ownerClientId)
        {
            var building = _buildings.Find(b => b.name.ToLower().Trim() == buildingName.ToLower().Trim());

            // Round the position to the nearest 0.5 increment
            Vector2 roundedPosition = new Vector2(
                Mathf.Round(position.x * 2f) / 2f, 
                Mathf.Round(position.y * 2f) / 2f
            );

            var instance = Instantiate(building.gameObject, roundedPosition, Quaternion.identity);
            
            var buildingScript = instance.GetComponent<Building>();
            buildingScript.PlayerOwnerId.Value = ownerClientId;
            
            var networkObject = instance.GetComponent<NetworkObject>();
            networkObject.Spawn();
        }
    }
}