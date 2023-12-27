using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{


    public void ShowCanvas(string _canvasName)
    {
        GameObject canvasObj = gameObject.transform.root.Find(_canvasName).gameObject;

        if (canvasObj != null)
            canvasObj.SetActive(true);
    }

    public void HideCanvas(string _canvasName)
    {
        GameObject canvasObj = GameObject.Find(_canvasName);

        if (canvasObj != null)
            canvasObj.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeScene(string _sceneName) 
    {
        SceneManager.LoadScene(_sceneName);
    }
}
