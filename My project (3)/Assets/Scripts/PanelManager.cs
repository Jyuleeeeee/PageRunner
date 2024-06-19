using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PanelManager : MonoBehaviour
{
    public GameManager gameManager;

    public TMP_Text score;

    void Awake()
    {
        transform.gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        score.text = (gameManager.totalPoint + gameManager.stagePoint).ToString();
        transform.gameObject.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    public void GameExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
