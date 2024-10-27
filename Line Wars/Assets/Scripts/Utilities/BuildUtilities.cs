using UnityEngine;

#if UNITY_EDITOR
using ParrelSync;
#endif

namespace Utilities
{
    public static class BuildUtilities
    {
        public static bool IsClientBuild()
        {
            bool clientBuild = !Application.isBatchMode;

#if UNITY_EDITOR
            if (!ClonesManager.IsClone())
                clientBuild = false;
#endif

            return clientBuild;
        }
    }
}