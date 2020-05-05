using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UnityEngine.SceneManagement;

public class MonoScript : MonobitEngine.MonoBehaviour
{
    public static TURN turn;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public static void CreateRoom(string roomName)
    {
        MonobitNetwork.playerName = "host";
        MonobitNetwork.CreateRoom(roomName);
        Debug.Log("ルーム【"+roomName + "】を作成");
    }
    public static string[] getRoomNames()
    {
        List<string> rooms = new List<string>();
        foreach (RoomData room in MonobitNetwork.GetRoomData())
        {
            //1人以下のみ
            if (room.playerCount <= 1)
            {
                rooms.Add(room.name);
                Debug.Log("ルーム【" + room.name + "】を取得");
            }
        }
        return rooms.ToArray();
    }
    public static void LeaveRoom()
    {
        MonobitNetwork.LeaveRoom();
        Debug.Log("退室しました");
    }
    public static void JoinRoom(string roomName)
    {
        MonobitNetwork.playerName = "client";
        MonobitNetwork.JoinRoom(roomName);
        Debug.Log("ルーム【"+roomName + "】に入室しました");
    }
    public static void ConnectServer()
    {
        MonobitNetwork.autoJoinLobby = true;
        MonobitNetwork.ConnectServer("othello");
        Debug.Log("サーバーにアクセスしました");
    }
    public TURN getTurn()
    {
        monobitView.RPC("getHostTurn",MonobitTargets.All);
        Debug.Log(MonoScript.turn);
        return MonoScript.turn;
    }
    public void ready()
    {
        monobitView.RPC("gameStart", MonobitTargets.All);
    }
    [MunRPC]
    public void gameStart()
    {
        System_manager.play_mode = PLAY_MODE.multi;
        System_manager.receiveTurn = MonoScript.turn;
        SceneManager.LoadScene("SampleScene");
    }
    [MunRPC]
    public void getHostTurn()
    {
        if (MonoScript.turn == TURN.draw_first)
            monobitView.RPC("setTurn", MonobitTargets.All, TURN.play_first);
        else if (MonoScript.turn == TURN.play_first)
            monobitView.RPC("setTurn", MonobitTargets.All, TURN.draw_first);
        else
            monobitView.RPC("putError", MonobitTargets.All, "MonoScript.turnに正しい値が入っていません");
    }
    [MunRPC]
    public void setTurn(TURN turn)
    {
        MonoScript.turn = turn;
    }
    [MunRPC]
    public void putError(string str)
    {
        Debug.LogError(str);
    }
}
