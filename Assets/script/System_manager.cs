using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public enum PLAY_MODE { single, multi }
public enum TURN { play_first, draw_first }
public class System_manager : MonobitEngine.MonoBehaviour
{
    private PLAY_MODE ptype;
    //最初に選んだターン
    private TURN turn;
    //現在のターン
    private TURN now_turn = TURN.play_first;

    public board b;

    public GameObject ui_turn;

    /** ルーム名. */
    private string roomName = "";

    private int sx, sy;
    private int n;
    private bool o = false;

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
        ui_turn.SetActive(true);
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
        dummy_array_reset();
        if (!end_check())
        {
            if (now_turn == turn)
            {
                Debug.Log("<COLOR=YELLOW>プレイヤー入力待ち</COLOR>");
                b.can_set(now_turn);
                if (pass_check())
                {
                    turn_change();
                }
            }
            else
            {
                Invoke("call_cpu",1.0f);
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
    public void call_cpu()
    {
        //cpuの配置
        Debug.Log("<COLOR=YELLOW>CPU入力待ち</COLOR>");
        b.can_set(now_turn);
        if (!pass_check())
        {
            cpu_set_check();
        }
        else
        {
            turn_change();
        }
    }
    //CPUが置けるか判定
    public void cpu_set_check()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int cnt = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                int random = Random.Range(0, 100);
                //初めにおける場所におく
                if (b.dummy_array[i, j].activeSelf == true && cnt == 0)
                {
                    if (10 > random)
                    {
                        b.dummy_array[i, j].GetComponent<DummyScript>().setKoma();
                        cnt++;
                    }
                }
            }
        }
        //Debug.Log("random成功");
        if (cnt == 0)
        {
            cpu_set_check();
        }
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
    public void please_input_multi()
    {
        if (!end_check())
        {
            if (now_turn == turn)
            {
                b.can_set(now_turn);
                Debug.Log("<COLOR=YELLOW>プレイヤー1入力待ち</COLOR>");
            }
            else
            {
                //プレイヤー2の処理
                Debug.Log("<COLOR=YELLOW>プレイヤー2入力待ち</COLOR>");
            }
        }
    }
    public void multi_play()
    {
        b.board_start();
        //ループ終了処理（両者打つところがなくなる or全部の面を埋まったら)
        please_input_multi();
    }
    void OnGUI()
    {
        Debug.Log("GUI起動");
        // MUNサーバに接続している場合
        if (MonobitNetwork.isConnect)
        {
            // ルームに入室している場合
            if (MonobitNetwork.inRoom)
            {
                // ルーム内のプレイヤー一覧の表示
                GUILayout.BeginHorizontal();
                GUILayout.Label("PlayerList : ");
                foreach (MonobitPlayer player in MonobitNetwork.playerList)
                {
                    GUILayout.Label(player.name + " ");
                }
                GUILayout.EndHorizontal();
                if (o)
                {
                    // 座標を送信する
                    monobitView.RPC("RecvCoordinate",
                        MonobitTargets.Others,
                        //座標
                        this.sx,
                        this.sy,
                        this.n
                        );
                    o = false;
                }
                // ルームからの退室
                if (GUILayout.Button("Leave Room", GUILayout.Width(150)))
                {
                    MonobitNetwork.LeaveRoom();
                }
            }
            // ルームに入室していない場合
            else
            {
                // ルーム名の入力
                GUILayout.BeginHorizontal();
                GUILayout.Label("RoomName : ");
                roomName = GUILayout.TextField(roomName, GUILayout.Width(200));
                GUILayout.EndHorizontal();

                // ルームを作成して入室する
                if (GUILayout.Button("Create Room", GUILayout.Width(150)))
                {
                    MonobitNetwork.CreateRoom(roomName);
                }
                // ルーム一覧を検索
                foreach (RoomData room in MonobitNetwork.GetRoomData())
                {
                    // ルームパラメータの可視化
                    System.String roomParam =
                        System.String.Format(
                            "{0}({1}/{2})",
                            room.name,
                            room.playerCount,
                            ((room.maxPlayers == 0) ? "-" : room.maxPlayers.ToString())
                        );

                    // ルームを選択して入室する
                    if (GUILayout.Button("Enter Room : " + roomParam))
                    {
                        MonobitNetwork.JoinRoom(room.name);
                    }
                }
            }
        }
        // MUNサーバに接続していない場合
        else
        {
            // プレイヤー名の入力
            GUILayout.BeginHorizontal();
            GUILayout.Label("PlayerName : ");
            MonobitNetwork.playerName = GUILayout.TextField(
                (MonobitNetwork.playerName == null) ?
                    "" :
                    MonobitNetwork.playerName, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            // デフォルトロビーへの自動入室を許可する
            MonobitNetwork.autoJoinLobby = true;

            // MUNサーバに接続する
            if (GUILayout.Button("Connect Server", GUILayout.Width(150)))
            {
                MonobitNetwork.ConnectServer("SimpleChat_v1.0");
            }
        }
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
