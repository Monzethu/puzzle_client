using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverText;
    public GameObject gameClearText;
    public GameObject next;
    public GameObject restart;
    public GameObject home;

    public void GameOver()
    {
        gameOverText.SetActive(true);
        restart.SetActive(true);
        home.SetActive(true);
        //audioSource.PlayOneShot(gameOverSE);

    }

    public void GameClear()
    {
        gameClearText.SetActive(true);
        next.SetActive(true);
        home.SetActive(true);
        //audioSource.PlayOneShot(gameClearSE);
    }
}
