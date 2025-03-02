using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] Image inventory;
    [SerializeField] Image itemSpot;
    [SerializeField] Image endScreen;

    [SerializeField] Button endGameButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] TextMeshProUGUI defeatTMP;
    [SerializeField] TextMeshProUGUI winTMP;

    private void Start()
    {
        GameManager.Instance.OnDefeatGame += EndGame;
        GameManager.Instance.OnWinGame += WinGame;
    }

    private void EndGame()
    {
        inventory.enabled = false;
        itemSpot.enabled = false;
        endScreen.gameObject.SetActive(true);
        winTMP.gameObject.SetActive(false);
        endGameButton.onClick.AddListener(GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>().ExitGame);
        mainMenuButton.onClick.AddListener(GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>().SceneChange_TitleScene);
    }

    private void WinGame()
    {
        inventory.enabled = false;
        itemSpot.enabled = false;
        endScreen.gameObject.SetActive(true);
        defeatTMP.gameObject.SetActive(false);
        endGameButton.onClick.AddListener(GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>().ExitGame);
        mainMenuButton.onClick.AddListener(GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>().SceneChange_TitleScene);
    }
}
