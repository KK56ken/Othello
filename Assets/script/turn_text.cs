using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class turn_text : MonoBehaviour
{
    public Text targetText;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void turn_text_change(TURN turn,TURN nowturn)
    {
        this.targetText = this.GetComponent<Text>();
        if (turn == nowturn)
        {
            this.targetText.text = "あなたのターン";
        }
        else 
        {
            this.targetText.text = "相手のターン";
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
