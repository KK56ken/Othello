using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] GameObject ui_mode_select;
    [SerializeField] GameObject ui_room;
    [SerializeField] GameObject ui_room_name;
    [SerializeField] GameObject ui_room_list;
    [SerializeField] GameObject ui_turn_select;
    [SerializeField] GameObject ui_wait;
    [SerializeField] MonoScript monobit;
    [SerializeField] ToggleGroup toggleGroup;
    //アクセスの試行回数を記録
    int acccess_count = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void onClickOffButton()
    {

        System_manager.play_mode = PLAY_MODE.single;
        SceneManager.LoadScene("SampleScene");
    }
    public void onClickEndButton()
    {
        UnityEngine.Application.Quit();
    }
    public void onClickOnButton()
    {
        ui_mode_select.SetActive(false);
        ui_room.SetActive(true);
        MonoScript.ConnectServer();
        Invoke("roomUpdate", 1.0F);
    }
    public void onClickRoomCreateButton()
    {
        //inputFieldからテキストを取得
        string roomName = ui_room_name.GetComponent<Text>().text;
        MonoScript.CreateRoom(roomName);
        ui_room.SetActive(false);
        ui_wait.SetActive(true);
    }
    public void onClickRoomUpdateButton()
    {
        roomUpdate();
    }
    private void roomUpdate()
    {
        //リスト内のボタンを削除
        foreach (Transform n in ui_room_list.transform)
        {
            GameObject.Destroy(n.gameObject);
        }

        //リソースからボタンを読み込む
        GameObject roomPrefab = (GameObject)Resources.Load("room_select");
        int cnt = 1;
        foreach (string roomName in MonoScript.getRoomNames())
        {
            //リソースからルームボタンを生成
            GameObject room = Instantiate(roomPrefab) as GameObject;

            //テキストをルーム名に変更
            room.transform.GetChild(0).GetComponent<Text>().text = roomName;

            //表示位置を変更
            RectTransform rect = room.GetComponent<RectTransform>();
            rect.localPosition = new Vector3(rect.localPosition.x, -50 * cnt);

            //リストの子に追加
            room.transform.SetParent(ui_room_list.transform, false);

            //クリックイベント追加
            string roomName_e = roomName;
            room.GetComponent<Button>().onClick.AddListener(() => onClickRoom(roomName_e));

            cnt++;
        }
    }
    public void onClickRoom(string roomName)
    {
        MonoScript.JoinRoom(roomName);
        Invoke("checkJoin", 1.0F);
    }
    private void checkJoin()
    {
        if (MonoScript.isConnect())
        {
            ui_room.SetActive(false);
            monobit.guestReady();
        }
        else
        {
            //5回まで再試行する
            if (acccess_count <= 5)
            {
                Invoke("checkJoin", 1.0F);
            }
            else
            {
                acccess_count = 0;
                ui_room.SetActive(true);
            }

        }
    }
    public void onClickBackToModeSelect()
    {
        ui_room.SetActive(false);
        ui_mode_select.SetActive(true);
    }
    public void onClickWaitCancel()
    {
        MonoScript.LeaveRoom();
        ui_wait.SetActive(false);
        ui_room.SetActive(true);
    }
}
