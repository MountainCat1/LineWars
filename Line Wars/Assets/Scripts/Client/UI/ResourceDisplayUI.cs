using System.Globalization;
using Managers;
using TMPro;
using UnityEngine;
using Zenject;

namespace Client.UI
{
    public class ResourceDisplayUI : MonoBehaviour
    {
        [Inject] private IPlayerManager _playerManager;

        [SerializeField] private TextMeshProUGUI goldText;
        
        private void Update()
        {
            if(_playerManager.LocalPlayer == null) return; // TODO: Use events to update the UI
            
            // TODO: Use events to update the UI
            goldText.text = _playerManager.LocalPlayer.GamePlayer.Gold.ToString(CultureInfo.InvariantCulture);
            
        }
    }
}