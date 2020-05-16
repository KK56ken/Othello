using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class turnSelect : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public System_manager system_manager;
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void OnClick()
    {
        //Get the label in activated toggles
        string selectedLabel = toggleGroup.ActiveToggles()
            .First().name;

        if (selectedLabel == "toggle_first")
        {
            system_manager.set_turn(TURN.play_first);
            system_manager.set_now_turn(TURN.play_first);
            Debug.Log("先手を選択しました。");
            system_manager.send_turn(2);
        }
        else if (selectedLabel == "toggle_second")
        {
            system_manager.set_turn(TURN.draw_first);
            system_manager.set_now_turn(TURN.play_first);
            Debug.Log("後手を選択しました。");
            system_manager.send_turn(1);
        }
        //シングルモード
        if (System_manager.play_mode == PLAY_MODE.single) 
            system_manager.Game_start(PLAY_MODE.single);
        else if (System_manager.play_mode == PLAY_MODE.multi)
            system_manager.Game_start(PLAY_MODE.multi);
        this.transform.root.gameObject.SetActive(false);
    }
}
