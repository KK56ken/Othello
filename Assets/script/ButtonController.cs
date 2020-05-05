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
        System_manager.receiveMode = PLAY_MODE.single;
        SceneManager.LoadScene("SampleScene");
    }
    public void onClickEndButton()
    {
        UnityEngine.Application.Quit();
    }
    public void onClickOnButton()
    {
        GameObject ui_mode = GameObject.Find("ui_mode_select");
        GameObject ui_room = GameObject.Find("ui_room");
        ui_mode.SetActive(false);
        ui_room.SetActive(true);
        System_manager.receiveMode = PLAY_MODE.multi;
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
