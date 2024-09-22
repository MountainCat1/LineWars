using UnityEngine;
using Zenject;

namespace Extensions
{
    public static class MonoBehaviourExtensions
    {
        public static T Inject<T>(this T script) where T : UnityEngine.MonoBehaviour
        {
            var container = Object.FindObjectOfType<SceneContext>().Container;
            
            container.Inject(script);
            
            return script;
        } 
    }
}