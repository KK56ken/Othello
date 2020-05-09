using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class result_text : MonoBehaviour
{
    public Text targetText;
    public board b;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void result_text_change(string player_name)
    {
        int black_kazu = b.get_koma_kazu("Black");
        int white_kazu = b.get_koma_kazu("White");
        this.targetText = this.GetComponent<Text>();
        if (black_kazu > white_kazu) { 
            this.targetText.text = player_name + "(黒)の勝ちです\n\n先手(黒):" + black_kazu + "\n" + "後手(白):" + white_kazu;
        }
        else if (black_kazu < white_kazu)
        { 
            this.targetText.text = player_name + "(白)の勝ちです\n\n先手(黒):" + black_kazu + "\n" + "後手(白):" + white_kazu;
        }
        else if (black_kazu == white_kazu)
        {
            this.targetText.text = "引き分けです\n\n先手(黒):" + black_kazu + "\n" + "後手(白):" + white_kazu;
        }
        else
        {
            Debug.Log("change_textがおかしくない");
        }
    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
