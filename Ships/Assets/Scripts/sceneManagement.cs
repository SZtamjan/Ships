using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManagement : MonoBehaviour
{

    public void changeScene()
    {
        SceneManager.LoadScene(1);

    }
    public void GameScene()
    {
        SceneManager.LoadScene(3);

    }
    public void Exit()
    {
        Application.Quit();
    }
}
