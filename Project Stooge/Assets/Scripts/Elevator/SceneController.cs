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

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        DontDestroyOnLoad(player);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchScene();
        }
    }

    private void SwitchScene()
    {
        if (sceneType == SceneType.One)
        {
            player.transform.position = new Vector3(-8.5f, 88.5f, -10f);
        }
        else if (sceneType == SceneType.Two)
        {
            player.transform.position = new Vector3(10.5f, 12.3f, -5f); 
        }

        SceneManager.LoadScene(nextSceneName);
    }
}