using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadLevel()
    {
        SceneManager.LoadScene("Level1");
    }

    // Update is called once per frame
    
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
