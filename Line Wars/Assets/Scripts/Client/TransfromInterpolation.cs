using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;

public class TransfromInterpolation : MonoBehaviour
{
    public event Action<Vector2> Moved; 
    
    [field: SerializeField] public NetworkTransform NetworkTransform { get; set; }
    [SerializeField] private float lerpRate;
    [SerializeField] private float moveInterpolation;
    [SerializeField] private float snapDistance = 0.05f;
    private const float GlobalLerpRateMultiplier = 1f;

    private Transform _transform;

    private void Awake()
    {
        if (!NetworkManager.Singleton.IsClient)
        {
            enabled = false;
            return;
        };
        
        _transform = transform;
    }

    private void Start()
    {
        StartInterpolation();
    }

    public void StartInterpolation()
    {
        if(!NetworkManager.Singleton.IsClient)
            return;
        
        _transform.parent = null;
    }

    private void Update()
    {
        if (NetworkTransform.IsDestroyed())
        {
            Destroy(gameObject);
            return;
        }
        
        var clientPosition = transform.position;
        var serverPosition = NetworkTransform.transform.position;
        
        if(Vector2.Distance(clientPosition, serverPosition) < snapDistance)
        {
            transform.position = serverPosition;
            return;
        }
        
        float t = Mathf.Clamp01(lerpRate * Time.deltaTime * GlobalLerpRateMultiplier);
        var lerpedPosition = Vector3.Lerp(clientPosition, serverPosition, t);
        var lerpedMovedPosition = Vector3.MoveTowards(lerpedPosition, serverPosition, moveInterpolation * Time.deltaTime);
        
        var move = lerpedMovedPosition - clientPosition;
        
        _transform.position = lerpedMovedPosition;
        
        Moved?.Invoke(move);
    }
}