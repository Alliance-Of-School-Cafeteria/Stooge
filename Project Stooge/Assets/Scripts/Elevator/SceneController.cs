using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public enum SceneType { One, Two };

    /* ---------------- 인스펙터 --------------- */
    [Header("설정")]
    [SerializeField]
    private string nextSceneName;

    public SceneType sceneType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchScene();
        }
    }

    private void SwitchScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}