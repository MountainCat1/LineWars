using UnityEngine;

namespace MainMenuUI
{
    public class MenuPageUI : MonoBehaviour
    {
        public void Show()
        {
            OnShown();
        }
        
        public void Hide()
        {
            OnHidden();
        }

        protected virtual void OnHidden()
        {
        }

        protected virtual void OnShown()
        {
        }
    }
}