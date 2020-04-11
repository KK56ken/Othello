using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAY_MODE { single, multi }
public enum TURN { play_first, draw_first }
public class System_manager : MonoBehaviour
{
    private PLAY_MODE ptype;
    //最初に選んだターン
    private TURN turn;
    //現在のターン
    private TURN now_turn = TURN.play_first;

    public board b;

    //選んだモードを割り当てる
    public void set_play_mode(PLAY_MODE ptype)
    {
        this.ptype = ptype;
    }
    //選んだモードを取得する
    public PLAY_MODE get_play_mode()
    {
        return this.ptype;
    }
    // Start is called before the first frame update
    void Start()
    {
    }
    //オセロのシステム起動
    public void Game_start(PLAY_MODE mode)
    {
        if (mode == PLAY_MODE.single)
        {
            //シングルプレイの処理
            single_play_start();
        }
        else if (mode == PLAY_MODE.multi)
        {
            //マルチプレイの処理
            multi_play();
        }
        else
        {
            Debug.LogError("mode選びミスってませんか？");
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
    //シングルプレイの初回の処理
    public void single_play_start()
    {
        b.board_start();

        //ループ終了処理（両者打つところがなくなる or全部の面を埋まったら)
        please_input();
    }
    //ターンを切り替えるときの処理
    public void turn_change()
    {
        if (now_turn == TURN.draw_first)
            now_turn = TURN.play_first;
        else if (now_turn == TURN.play_first)
            now_turn = TURN.draw_first;
        else Debug.LogError("now_turn:値は不正です");
        
        Debug.Log("<COLOR=YELLOW>ターン変わったよ" + this.turn + "</COLOR>");

        please_input();
    }
    public void please_input()
    {
        if (!end_check())
        {
            if (now_turn == turn)
            {
                b.can_set(now_turn);
                Debug.Log("<COLOR=YELLOW>プレイヤー入力待ち</COLOR>");
            }
            else
            {
                //cpuの配置
                b.can_set(now_turn);
                Debug.Log("<COLOR=YELLOW>CPU入力待ち</COLOR>");
            }
        }
    }
    public void multi_play()
    {
    }

    //終了;true
    public bool end_check()
    {
        bool end_flag = true;
        //すべてのマスに駒が置いてないか判定
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (KOMA_TYPE.None == b.get_koma_type(i, j))
                {
                    end_flag = false;
                    break;
                }
            }
        }
        //どちらの駒もおけないか判定
        if (!b.can_not_revers())
        {
            Debug.Log("<COLOR=RED>両者パス</COLOR>");
            end_flag = true;
        }
        return end_flag;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
