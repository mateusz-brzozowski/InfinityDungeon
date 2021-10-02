using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatWindow : MonoBehaviour
{
    private static int isSceneChanged = 0;
    public static int GetIsSceneChanged() { return isSceneChanged; }
    public void BackToMenu()
    {
        isSceneChanged = 1;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Rety()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
