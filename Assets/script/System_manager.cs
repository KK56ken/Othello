using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAY_MODE { single, multi}
public enum TURN { play_first, draw_first}
public class System_manager : MonoBehaviour
{
    //最初に選んだターン
    public TURN turn;
    //現在のターン
    public TURN now_turn;

    // Start is called before the first frame update
    void Start()
    {   
        //ここにモードを入力
        play_mode(PLAY_MODE.single);
    }
    public void play_mode(PLAY_MODE mode)
    {
        if (mode == PLAY_MODE.single)
        {
            //シングルプレイの処理
            single_play();
        }
        else if (mode == PLAY_MODE.multi)
        {
            //マルチプレイの処理
            multi_play();
        }
        else
        {
            Debug.Log("mode選びミスってませんか？");
        }
    }
    public void set_turn(TURN turn)
    {
        this.turn = turn;
    }
    public TURN get_turn()
    {
        return this.turn;
    }
    public void single_play()
    {
        //turn = get_turn();
        //if(turn == TURN.play_first)
        ////{
        //    //ループ終了処理（両者打つところがなくなる　or　全部の面を埋まったら)
        //    //while (true)
        //    //{
        //        //自分のターン

        //        //ターンを変更する

        //        //相手のターン

        //   // }
        //}
        //else if (turn == TURN.draw_first)
        //{
        //    //ループ終了処理（両者打つところがなくなる　or　全部の面を埋まったら)
        //    //相手の処理終了後ループ
        //    //ターンを変更する
        //    //置く処理
        //}
        //else
        //{
        //    Debug.Log("ターンのとこでみすってるよ");
        //}
        
    }
    public void multi_play()
    {

    }
    public void turn_chenge(TURN turn)
    {
        if(turn == TURN.play_first)
        {
            turn = TURN.draw_first;
        }
        else if(turn == TURN.draw_first){
            turn = TURN.play_first;
        }
        else
        {
            Debug.Log("例外発生　turn_chengeでエラーが起こってるよ");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
