using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onClickOffButton()
    {
        Invoke("ChangeScene", 1.0f);
    }
    public void onClickEndButton()
    {
        UnityEngine.Application.Quit();
    }
    void ChangeScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
