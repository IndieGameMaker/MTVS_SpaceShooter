using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    private void OnEnable()
    {
        // 버튼 클릭 이벤트 구독(Subscribe)
        _startButton.onClick.AddListener(OnStartButtonClick);
    }

    private void OnDisable()
    {
        // 이벤트 구독 해지
        _startButton.onClick.RemoveListener(OnStartButtonClick);
    }

    public void OnStartButtonClick()
    {
        // 씬 로딩
        // Debug.Log("시작버튼 클릭됨");
        SceneManager.LoadScene("InGame");  // 씬의 이름을 로딩
        // SceneManager.LoadScene(1);  // 씬의 Index으로 로딩
        
    }
}
