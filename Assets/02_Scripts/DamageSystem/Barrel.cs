using Unity.Cinemachine;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    // 스크립터블오브젝트 참조
    [SerializeField] private BarrelDataSO _barrelData;
    
    // [SerializeField] private GameObject _expEffect;
    // [SerializeField] private AudioClip _expSfx;
    
    private const string TAG_BULLET = "BULLET";
    private int _hitCount;
    
    // 컴포넌트 캐싱
    private Rigidbody _rb;
    private AudioSource _audio;
    private CinemachineImpulseSource _impulseSource;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag(TAG_BULLET))
        {
            //_hitCount++;
            if (++_hitCount == 3)
            {
                // 폭발효과 연출
                ExpBarrel();
            }
        }
    }

    private void ExpBarrel()
    {
        // 폭발 원점
        Vector3 expPos = transform.position + (Random.insideUnitSphere * 2.0f);
        
        // 주변 배럴에 폭발력 전달
        Collider[] barrels = Physics.OverlapSphere(expPos, _barrelData.radius, _barrelData.layerMask); // 2^8 = 256

        foreach (Collider coll in barrels)
        {
            var rb = coll.GetComponent<Rigidbody>();
            
            // 폭발 효과 
            // Rigidbody.AddExplosionForce(폭발력, 폭발원점, 반경, 위로솟구치는힘)
            rb.mass = 1.0f;
            rb.AddExplosionForce(_barrelData.force
                , expPos
                , _barrelData.radius
                , _barrelData.upwardForce);
        }

        // 폭발 파티클 생성
        GameObject effect = Instantiate(_barrelData.expEffect
                                        , transform.position
                                        , transform.rotation);
        Destroy(effect, 5.0f);
        // 폭발 사운드 재생
        _audio.PlayOneShot(_barrelData.expSfx);
        // 충격파 처리
        _impulseSource.GenerateImpulse();
        
        // 드럼통 소멸 처리
        Destroy(gameObject, 3.0f);
    }
}
