using System;

namespace Managers
{
    public class StageManager : IStageManager
    {
        public event Action GameStarted;
        
        public void StartGame()
        {
            GameStarted?.Invoke();
        }
    }

    public interface IStageManager
    {
        event Action GameStarted;
    }
}