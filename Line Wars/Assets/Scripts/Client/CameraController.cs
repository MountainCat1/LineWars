using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Client
{
    public class CameraController : MonoBehaviour
    {
        [Inject] private IInputMapper _inputMapper;
        
        [SerializeField] private float cameraSpeed = 1f;
        
        void Start()
        {
            _inputMapper.CameraMoved += OnCameraMoved;
        }

        private void OnCameraMoved(Vector2 move)
        {
            var moveVector = new Vector3(move.x, move.y, 0);
            transform.position += moveVector * cameraSpeed * Time.deltaTime;
        }
    }
}