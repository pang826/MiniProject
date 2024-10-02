using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private static SceneChanger instance;
    public static SceneChanger Instance {  get { return instance; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        DontDestroyOnLoad(gameObject);
    }
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
