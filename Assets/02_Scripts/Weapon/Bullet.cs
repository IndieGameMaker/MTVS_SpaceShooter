using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _force = 1200.0f;
    
    private TrailRenderer _trail;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _trail = GetComponent<TrailRenderer>();

        
        // GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * _force);
    }

    public void Fire(Vector3 _pos, Quaternion _rot)
    {
        transform.SetPositionAndRotation(_pos, _rot);
        
        _rb.linearVelocity = _rb.angularVelocity = Vector3.zero;
        _rb.rotation = _rot;
        _trail.Clear();
        // 뉴튼(N)
        _rb.AddRelativeForce(Vector3.forward * _force);
    }
}
