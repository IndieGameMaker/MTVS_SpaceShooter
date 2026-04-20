using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform _firePos;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private AudioClip _fireSfx;
    [SerializeField] private MeshRenderer _muzzleFlash;

    // 연사속도
    [SerializeField] private float _fireRate = 0.15f;
    // 다음 발사 시각
    private float _nextFire;
    
    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.playOnAwake = false;
        _audio.minDistance = 10.0f;
        _audio.maxDistance = 50.0f;
        
        _firePos = transform.Find("FirePos").transform;
        
        _muzzleFlash = transform.Find("FirePos/MuzzleFlash").GetComponent<MeshRenderer>();
        _muzzleFlash.enabled = false;
    }
    
    void Update()
    {
        FireBullet();
    }

    private void FireBullet()
    {
        // Legacy InputManager 사용한 방법
        if (Input.GetMouseButtonDown(0))
        {
            // 총 발사 로직
            // Instantiate(생성할객체, 위치, 각도, 부모게임오브젝트)
            // Instantiate(_bulletPrefab, _firePos.position, _firePos.rotation);
        }

        // New InputSystem 사용한 방법
        if (Mouse.current.leftButton.isPressed)
        {
            if (Time.time > _nextFire)
            {
                _nextFire = Time.time + _fireRate;
                
                Instantiate(_bulletPrefab, _firePos.position, _firePos.rotation);
                // 음원 재생
                // AudioSource.Play("음원이름");
                // AudioSource.PlayOneShot(AudioClip, 볼륨);
                _audio.PlayOneShot(_fireSfx, 0.8f);
                // 총구 화염 효과
                StartCoroutine(ShowMuzzleFlash());
            }
        }
    }

    /*
     * 동기 방식 (sync)
     * 함수 1 (5초)
     * 함수 2 (1초)
     * 
     * 비동기 방식 (async)
     * 비동기 방식으로 호출 : 함수 1 (5초)
     * 함수 2 (1초)
     *
     * Multi-Thread
     * 1. Thread 프로그래밍
     * 2. async / await / Task
     * 3. Co-routine (코루틴)
     */
    private IEnumerator ShowMuzzleFlash()
    {
        // 텍스처 오프셋 변경
        // Random.Range(정수, 정수)     Random.Range(1, 10)  1 ~ 9
        // Random.Range(실수, 실수)     Random.Range(1.0f, 10.0f) 1.0f ~ 10.0f
        
        // (0, 0) , (0.5, 0), (0.5, 0.5), (0.5, 0)
        Vector2 offset = new Vector2(Random.Range(0,2), Random.Range(0, 2)) * 0.5f;
        _muzzleFlash.material.mainTextureOffset = offset;
        // 크기 조절
        float scale = Random.Range(1.2f, 2.5f);
        _muzzleFlash.transform.localScale = Vector3.one * scale; //new Vector3(scale, scale, scale);
        // 회전 각도 설정
        float angle = Random.Range(0, 360);
        _muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);
        
        _muzzleFlash.enabled = true;
        // Waiting
        yield return new WaitForSeconds(0.2f);
        _muzzleFlash.enabled = false;
    }
}
