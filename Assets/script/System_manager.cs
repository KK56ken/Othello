using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAY_MODE { single, multi}
public enum TURN { play_first, draw_first}
public class System_manager : MonoBehaviour
{
    private PLAY_MODE ptype;
    //最初に選んだターン
    private TURN turn;
    //現在のターン
    private TURN now_turn;
    //
    private bool put_dummy;
    public void set_play_mode(PLAY_MODE ptype)
    {
        this.ptype = ptype;
    }
    public PLAY_MODE get_play_mode()
    {
        return this.ptype;
    }
    // Start is called before the first frame update
    void Start()
    {
    }
    public void Game_start()
    {
        //ここにモードを入力
        play_mode(PLAY_MODE.single);
        Debug.Log("ターン変わったよ"+ this.turn);
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
    public void put_check()
    {
        put_dummy = true;
    }
    public void single_play()
    {
        board b = GameObject.Find("Board").GetComponent<board>();
        b.board_start();

        if (this.turn == TURN.play_first)
        {
            //ループ終了処理（両者打つところがなくなる or全部の面を埋まったら)
            if(end() == true)
            {
                //自分のターン
                if (get_now_turn() == TURN.play_first) {
                    b.can_set(b.get_thisX(), b.get_thisZ(), b.get_space_obj(), get_now_turn());
                    //ダミー押したら
                    if(put_dummy == true)
                    {
                        //置くこまをシロにする
                        put_dummy = false;
                    }
                }
                //ターンを変更する
                turn_chenge(get_now_turn());
                //相手のターン
                if (get_now_turn() == TURN.draw_first) {
                    b.can_set(b.get_thisX(), b.get_thisZ(), b.get_space_obj(), get_now_turn());
                }
            }
        }
        else if (this.turn == TURN.draw_first)
        {
            //ループ終了処理（両者打つところがなくなる　or　全部の面を埋まったら)
            if(end() == false)
            {
                //相手のターン
                if (get_now_turn() == TURN.draw_first)
                {
                    b.can_set(b.get_thisX(), b.get_thisZ(), b.get_space_obj(), get_now_turn());
                }

                //ターンを変更する
                turn_chenge(get_now_turn());
                //自分のターン
                if (get_now_turn() == TURN.play_first)
                {
                    b.can_set(b.get_thisX(), b.get_thisZ(), b.get_space_obj(), get_now_turn());
                    //ダミー押したら
                    if (put_dummy == true)
                    {
                        //置くこまをシロにする
                        put_dummy = false;
                    }
                }
            }
        }
        else
        {
            Debug.Log("ターンのとこでみすってるよ");
        }

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
        board board = GameObject.Find("Board").GetComponent<board>();
        bool not_end = false;
        //すべてのマスに駒が置いてあるか判定
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if(KOMA_TYPE.None == board.get_koma_type(i,j))
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
        if (board.can_not_revers())
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
