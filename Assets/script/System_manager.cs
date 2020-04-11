using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAY_MODE { single, multi}
public enum TURN { play_first, draw_first}
public class System_manager : MonoBehaviour
{
    //最初に選んだターン
    private TURN turn;
    //現在のターン
    private TURN now_turn;
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
    public void set_now_turn(TURN now_turn)
    {
        this.now_turn = now_turn;
    }
    public TURN get_now_turn()
    {
        return this.now_turn;
    }
    public void single_play()
    {
        //set_turn(TURN.play_first);
        //turn = get_turn();

        //if (turn == TURN.play_first)
        //{
        //    set_now_turn(turn);
        //    //ループ終了処理（両者打つところがなくなる or全部の面を埋まったら)
        //    while (end() == false)
        //    {
        //        //自分のターン

        //        //ターンを変更する
        //        turn_chenge(get_now_turn());
        //       //相手のターン

        //     }
        //}
        //else if (turn == TURN.draw_first)
        //{
        //    set_now_turn(turn);
        //    //ループ終了処理（両者打つところがなくなる　or　全部の面を埋まったら)
        //    while (end() == false)
        //    {
        //        //相手のターン

        //        //ターンを変更する
        //        turn_chenge(get_now_turn());
        //        //自分のターン
        //    }
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
            set_now_turn(TURN.draw_first);
        }
        else if(turn == TURN.draw_first)
        {
            set_now_turn(TURN.play_first);
        }
        else
        {
            Debug.Log("例外発生　turn_chengeでエラーが起こってるよ");
        }
    }
    public bool end()
    {
        board b = GameObject.Find("b").GetComponent<board>();
        bool not_end = false;
        //すべてのマスに駒が置いてあるか判定
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if(KOMA_TYPE.None == b.get_koma_type(i,j))
                {
                    not_end = true;
                }
            }
        }
        if (not_end == false)
        {
            return true;
        }
        //どちらの駒もおけないか判定
        if (b.can_not_revers())
        {
            return true;
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
