using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed = 10.0f;
    [SerializeField] [Range(2f, 10f)] private float _distance = 10.0f;
    [SerializeField] [Range(-5f, 5f)] private float _height = 3.0f;
    [SerializeField] [Range(0f, 5f)] private float _yOffset = 2.0f;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("PLAYER")?.transform;
        
        // 방어 코드
        if (_target == null) Debug.LogError("No target found");
    }

    private void LateUpdate()
    {
        Vector3 offsetTarget = _target.position + Vector3.up * _yOffset;
        Vector3 pos = _target.position - (_target.forward * _distance) + (Vector3.up * _height);
        transform.position = pos;
        transform.LookAt(offsetTarget);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && this.enabled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_target.position, 0.3f);
            
            // Y Offset 표시
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_target.position + Vector3.up * _yOffset, 0.3f);
            
            Gizmos.DrawLine(transform.position, _target.position + Vector3.up * _yOffset);
        }
    }
    
}
