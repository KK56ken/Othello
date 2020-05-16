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
        Debug.Log("ルーム【" + roomName + "】を作成");
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
        MonobitNetwork.playerName = "guest";
        if (MonobitNetwork.JoinRoom(roomName))
        {
            Debug.Log("ルーム【" + roomName + "】に入室しました");
        }
        else
        {
            Debug.LogError("ルーム【" + roomName + "】に入室できませんでした");
        }
    }
    public static void ConnectServer()
    {
        MonobitNetwork.autoJoinLobby = true;
        MonobitNetwork.ConnectServer("othello");
        Debug.Log("サーバーにアクセスしました");
    }
    public static bool isConnect()
    {
        if (MonobitNetwork.isConnect)
        {
            if (MonobitNetwork.inRoom)
                return true;
            else
                Debug.LogError("ルームに入室していません");
        }
        else
            Debug.LogError("サーバーに接続していません");
        return false;
    }
    public void guestReady()
    {
        //相手のゲームを開始する
        monobitView.RPC("hostGameStart", MonobitTargets.Others);
    }
    public void gameStart()
    {
        System_manager.play_mode = PLAY_MODE.multi;
        System_manager.receiveTurn = MonoScript.turn;
        SceneManager.LoadScene("SampleScene");
    }
    [MunRPC]
    public void hostGameStart()
    {
        Debug.Log("hostGameStartが呼び出されました");
        if (MonoScript.turn == TURN.draw_first)
            monobitView.RPC("guestGameStart", MonobitTargets.Others);
        else if (MonoScript.turn == TURN.play_first)
            monobitView.RPC("guestGameStart", MonobitTargets.Others);
        gameStart();
    }
    [MunRPC]
    public void guestGameStart()
    {
        Debug.Log("guestGameStartが呼ばれました");
        gameStart();
    }
}
