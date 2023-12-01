using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void OpenScene(int index)
    {
        SceneManager.LoadScene (index);
        if(index==0)
        {
            GameObject lvlobj = GameObject.FindGameObjectWithTag("LevelManager");
            Destroy(lvlobj);
            GameObject backgroundMusic = GameObject.FindGameObjectWithTag("backGroundMusic");
            Destroy(backgroundMusic);
        }
    }
}