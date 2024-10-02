using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void SceneChange_TitleScene()
    {
        SceneManager.LoadScene(0);
        Debug.Log("타이틀씬");
    }
    public void SceneChange_GameScene()
    {
        SceneManager.LoadScene(1);
        Debug.Log("게임씬");
    }

    public void ExitGame()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }
}
