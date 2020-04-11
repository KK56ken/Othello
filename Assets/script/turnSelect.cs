﻿using System.Collections;
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

        if(selectedLabel == "first_toggle")
            system_manager.set_turn(TURN.play_first);
        else if(selectedLabel == "second_toggle")
            system_manager.set_turn(TURN.draw_first);

        Debug.Log(selectedLabel + "を選択しました。");
    }
}