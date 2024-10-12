using UnityEngine;

namespace Server.Debug
{
    public class DebugGoal : MonoBehaviour
    {
        public static DebugGoal Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
            {
                UnityEngine.Debug.LogError("Multiple instances of DebugGoal found!");
                Destroy(this);
            }
            
            Instance = this;
        }

        
        
    }
}