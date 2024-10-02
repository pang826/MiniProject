using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void SceneChange_TitleScene()
    {
        SceneManager.LoadScene(0);
        Debug.Log("Ÿ��Ʋ��");
    }
    public void SceneChange_GameScene()
    {
        SceneManager.LoadScene(1);
        Debug.Log("���Ӿ�");
    }

    public void ExitGame()
    {
        Debug.Log("��������");
        Application.Quit();
    }
}
