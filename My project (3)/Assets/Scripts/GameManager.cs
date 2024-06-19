using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    AudioSource audio;
    public AudioClip[] audioClips;
    public PlayerManager playerManager;

    public int totalPoint;
    public int stagePoint;

    public int stageIndex;

    public int hp;

    public GameObject[] spawnPoint;

    public Image[] UIhp;
    public Image[] bulletimage;
    public Image chickenItem;
    public Image keyItem;
    public TMP_Text UIpoint;
    public TMP_Text UIstage;
    public TMP_Text UIbullet;

    public PanelManager panel;

    void Awake()
    {
        audio = GetComponent<AudioSource>();

        audio.clip = audioClips[0];
        audio.Play();
    }

    void Update()
    {
        UIpoint.text = (totalPoint + stagePoint).ToString();
        UIbullet.text = playerManager.magazine.ToString();

        if(PlayerManager.jumpItem == true)
            chickenItem.transform.gameObject.SetActive(true);
        else
            chickenItem.transform.gameObject.SetActive (false);

        if (PlayerManager.hasKey ==  true)
            keyItem.transform.gameObject.SetActive(true);
        else
            keyItem.transform.gameObject.SetActive (false);
    }

    public void NextStage()
    {
        if(stageIndex < spawnPoint.Length - 1)
        {
            stageIndex++;

            totalPoint += stagePoint;
            stagePoint = 0;

            UIstage.text = "STAGE " + (stageIndex + 1);
            playerManager.transform.position = spawnPoint[stageIndex].transform.position;

            playerManager.magazine = playerManager.maxBullet;

            audio.clip = audioClips[stageIndex];
            audio.Play();
        }
        else
            panel.ShowPanel();
    }

    public void HpDown()
    {
        if (hp > 1)
        {
            hp--;

            UIhp[hp].color = new Color(1, 1, 1, 0.3f);
        }
        else
        {
            UIhp[0].color = new Color(1, 1, 1, 0.3f);

            playerManager.Die();

            for (int i = 0; i < bulletimage.Length; i++)
                bulletimage[i].color = new Color(1, 1, 1, 0.3f);

            panel.ShowPanel();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player Reposition
            if(hp > 1)
                collision.transform.position = spawnPoint[stageIndex].transform.position;

            HpDown();
        }
    }
}
