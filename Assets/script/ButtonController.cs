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
        SceneManager.LoadScene("SampleScene");
    }
    public void onClickEndButton()
    {
        UnityEngine.Application.Quit();
    }
    public void onClickOnButton()
    {
        SceneManager.LoadScene("SampleScene");

    }
    public void onClickRoomCreateButton()
    {

    }
    public void onClickRoomUpdateButton()
    {

    }
    public void onClickRoom(string roomName)
    {

    }
}
