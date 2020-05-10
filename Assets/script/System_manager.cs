﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public enum PLAY_MODE { single, multi }
public enum TURN { play_first, draw_first }
public class System_manager : MonobitEngine.MonoBehaviour
{

    public static TURN receiveTurn;
    public static PLAY_MODE play_mode;

    //最初に選んだターン
    private TURN turn;
    //現在のターン
    private TURN now_turn = TURN.play_first;

    public board b;

    public result_text r;

    public turn_text t;

    /** ルーム名. */
    private string roomName = "";

    private int sx, sy;
    private int n;
    private bool o = false;
    public bool endflag = false;

    [SerializeField] GameObject ui_result;

    [SerializeField] GameObject ui_turn;

    [SerializeField] GameObject ui_start;

    // Start is called before the first frame update
    void Start()
    {
        ui_result.SetActive(false);
        ui_turn.SetActive(false);
        ui_start.SetActive(true);
        if (play_mode == PLAY_MODE.multi) {
            ui_start.SetActive(false);
            Game_start(play_mode);
        }

    }
    //オセロのシステム起動
    public void Game_start(PLAY_MODE mode)
    {
        ui_turn.SetActive(true);
        if (mode == PLAY_MODE.single)
        {
            //シングルプレイの処理
            single_play_start();
        }
        else if (mode == PLAY_MODE.multi)
        {
            //マルチプレイの処理
            multi_play_start();
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
    //マルチプレイの初回の処理
    public void multi_play_start()
    {
        b.board_start();

        //ループ終了処理（両者打つところがなくなる or全部の面を埋まったら)
        please_input_multi();
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
        if (play_mode == PLAY_MODE.single)
            please_input();
        else if (play_mode == PLAY_MODE.multi)
            please_input_multi();
    }
    public void please_input()
    {
        ui_turn.SetActive(true);
        dummy_array_reset();
        if (now_turn == turn)
        {
            Invoke("call_single_player", 1.0f);
        }
        else
        {
            Invoke("call_cpu", 1.0f);
        }
    }
    public void please_input_multi()
    {
        ui_turn.SetActive(true);
        dummy_array_reset();
        if (now_turn == receiveTurn)
        {
            t.turn_text_change(receiveTurn, this.now_turn);
            b.can_set(now_turn);
            if (pass_check())
            {
                if (endflag == true)
                {
                    ui_turn.SetActive(false);
                    if(TURN.play_first == receiveTurn)
                        r.result_text_change("play_first");
                    else if (TURN.draw_first == receiveTurn)
                        r.result_text_change("draw_first");
                    //終了
                    ui_result.SetActive(true);
                    //Debug.Log("終了処理できてるよ");
                }
                else
                {
                    endflag = true;
                    turn_change();
                    Debug.Log("ターンチェンジできてるよ");
                }
            }
            else
            {
                endflag = false;
            }
        }
        else
        {
            t.turn_text_change(receiveTurn, this.now_turn);
            b.can_set(now_turn);
            if (pass_check())
            {
                if (endflag == true)
                {
                    ui_turn.SetActive(false);
                    if (TURN.play_first == receiveTurn)
                        r.result_text_change("play_first");
                    else if (TURN.draw_first == receiveTurn)
                        r.result_text_change("draw_first");
                    //終了
                    ui_result.SetActive(true);
                    //Debug.Log("終了処理できてるよ");
                }
                else
                {
                    endflag = true;
                    turn_change();
                    Debug.Log("ターンチェンジできてるよ");
                }
            }
            else
            {
                endflag = false;
            }
        }
    }

    public void call_single_player()
    {
        Debug.Log("<COLOR=YELLOW>プレイヤー入力待ち</COLOR>");
        t.turn_text_change(this.turn, this.now_turn);
        b.can_set(now_turn);
        if (pass_check())
        {
            if (endflag == true)
            {
                ui_turn.SetActive(false);
                r.result_text_change("player");
                //終了
                ui_result.SetActive(true);
                //Debug.Log("終了処理できてるよ");
            }
            else
            {
                endflag = true;
                turn_change();
            }
        }
        else
        {
            endflag = false;
        }
    }
    public void call_cpu()
    {
        //cpuの配置
        Debug.Log("<COLOR=YELLOW>CPU入力待ち</COLOR>");
        t.turn_text_change(this.turn, this.now_turn);
        b.can_set(now_turn);
        if (!pass_check())
        {
            endflag = false;
            cpu_set_check();
        }
        else
        {
            if (endflag == true)
            {
                ui_turn.SetActive(false);
                r.result_text_change("cpu");
                //終了処理
                ui_result.SetActive(true);
                //Debug.Log("終了処理できてるよ");

            }
            else
            {
                endflag = true;
                turn_change();
            }
        }
    }
    public void dummy_array_reset()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                b.dummy_array[i, j].SetActive(false);
            }
        }
    }
    //CPUが置く処理
    public void cpu_set_check()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int cnt = 0;
        List<List<int>> list = new List<List<int>>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (b.dummy_array[i, j].activeSelf == true)
                {
                    list.Add(new List<int>(new int[] { i,j }));
                    cnt += 1;
                }
            }
        }
        int random = Random.Range(0, cnt);
        b.dummy_array[list[random][0], list[random][1]].GetComponent<DummyScript>().setKoma();

    }
    public bool pass_check()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                //Debug.Log(dummy_array[i, j].activeSelf);
                //置けるか置けないかを判断する
                if (b.dummy_array[i, j].activeSelf == true)
                {
                    return false;
                }
            }
        }
        return true;
    }
    public void fsend()
    {
        this.o = true;
    }
    public void send(int x, int y, int n)
    {
        this.sx = x;
        this.sy = y;
        this.n = n;

        // 座標を送信する
        monobitView.RPC("RecvCoordinate",
            MonobitTargets.Others,
            //座標
            this.sx,
            this.sy,
            this.n
            );

        Debug.Log("x =" + x + " y =" + y);
    }
    [MunRPC]
    void RecvCoordinate(int sender_x, int sender_y, int n)
    {
        KOMA_TYPE sender_koma_type;
        if (n == 1)
        {
            sender_koma_type = KOMA_TYPE.Black;
        }
        else
        {
            sender_koma_type = KOMA_TYPE.White;
        }
        b.SetKoma(sender_x, sender_y, sender_koma_type);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
