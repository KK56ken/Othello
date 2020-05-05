using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board : MonoBehaviour
{

    GameObject[,] komaArray = new GameObject[8, 8];
    public GameObject[,] dummy_array = new GameObject[8, 8];
    private float thisX;
    private float thisZ;
    private float space_obj;
    public System_manager system_manager;
    public void board_start()
    {
        setStartPosition();
    }
    // Start is called before the first frame update
    void Start()
    {
        //ボードのテクスチャを設定
        Texture2D m_texture;
        m_texture = new Texture2D(128, 128, TextureFormat.ARGB32, false);

        float space_texture = m_texture.width / 8;
        for (int y = 0; y < m_texture.height; y++)
        {
            for (int x = 0; x < m_texture.width; x++)
            {
                if (x % space_texture == 0 || y % space_texture == 0)
                    m_texture.SetPixel(x, y, Color.black);
                else
                    m_texture.SetPixel(x, y, Color.green);
            }
        }
        m_texture.Apply();
        GetComponent<Renderer>().material.mainTexture = m_texture;

    }
    void setStartPosition()
    {
        float width = this.transform.localScale.x;
        float height = this.transform.localScale.z;
        thisX = this.transform.localPosition.x - (width / 2);//boardの左上X
        thisZ = this.transform.localPosition.z - (height / 2);//boardの左上のZ
        space_obj = width / 8;
        set_thisX(thisX);
        set_thisZ(thisZ);
        set_space_obj(space_obj);

        for (int i = 3; i <= 4; i++)
        {
            for (int j = 3; j <= 4; j++)
            {
                GameObject obj = (GameObject)Resources.Load(@"koma_pare");
                float objWidth = obj.transform.localScale.x;
                float objHeight = obj.transform.localScale.z;
                float vecX = thisX + space_obj * i + (objWidth / 2);
                float vecZ = thisZ + space_obj * j + (objHeight / 2);
                komaArray[i, j] = Instantiate(obj, new Vector3(vecX, obj.transform.localPosition.y, vecZ), Quaternion.identity);
                if ((i == 3 && j == 4) || (i == 4 && j == 3))
                {
                    //コマを白にする
                    Debug.Log("白のコマを配置");
                    komaArray[i, j].GetComponent<komaScript>().type = KOMA_TYPE.White;
                    komaArray[i, j].GetComponent<komaScript>().x = i;
                    komaArray[i, j].GetComponent<komaScript>().y = j;
                    komaArray[i, j].GetComponent<komaScript>().setTyoe(get_koma_type(i, j));
                }
                else
                {
                    //コマを黒にする
                    Debug.Log("黒のコマを配置");
                    komaArray[i, j].GetComponent<komaScript>().type = KOMA_TYPE.Black;
                    komaArray[i, j].GetComponent<komaScript>().x = i;
                    komaArray[i, j].GetComponent<komaScript>().y = j;
                }
            }
        }
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                dummy_array[i, j] = setDummy(i, j, KOMA_TYPE.None);
            }
        }
    }
    public GameObject setDummy(int x, int y, KOMA_TYPE type)
    {
        float width = this.transform.localScale.x;
        float height = this.transform.localScale.z;
        float thisX = this.transform.localPosition.x - (width / 2);//boardの左上X
        float thisZ = this.transform.localPosition.z - (height / 2);//boardの左上のZ
        float space_obj = width / 8;

        GameObject dummy = (GameObject)Resources.Load(@"dummy");
        float dummyWidth = dummy.transform.localScale.x;
        float dummyHeight = dummy.transform.localScale.z;
        float vecZ = thisZ + space_obj * y + (dummyHeight / 2);
        float vecX = thisX + space_obj * x + (dummyWidth / 2);
        dummy.GetComponent<DummyScript>().x = x;
        dummy.GetComponent<DummyScript>().y = y;
        dummy.GetComponent<DummyScript>().koma_type = type;
        dummy.GetComponent<DummyScript>().system_manager = system_manager;
        dummy.SetActive(false);
        Debug.Log("<COLOR=blue>ダミーを生成</COLOR>");

        return Instantiate(dummy, new Vector3(vecX, dummy.transform.localPosition.y, vecZ), Quaternion.identity);
    }
    public void set_thisX(float thisX)
    {
        this.thisX = thisX;
    }
    public float get_thisX()
    {
        return this.thisX;
    }
    public void set_thisZ(float thisZ)
    {
        this.thisZ = thisZ;
    }
    public float get_thisZ()
    {
        return this.thisZ;
    }
    public void set_space_obj(float space_obj)
    {
        this.space_obj = space_obj;
    }
    public float get_space_obj()
    {
        return this.space_obj;
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void can_set(TURN now_turn)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                dummy_array[i, j].SetActive(false);
            }
        }

        //相手のターンを取得する

        KOMA_TYPE now_type = KOMA_TYPE.None;
        KOMA_TYPE opp_type = KOMA_TYPE.None;
        if (now_turn == TURN.play_first)
        {
            now_type = KOMA_TYPE.Black;
            opp_type = KOMA_TYPE.White;
        }
        else if (now_turn == TURN.draw_first)
        {
            now_type = KOMA_TYPE.White;
            opp_type = KOMA_TYPE.Black;
        }
        else
        {
            Debug.LogError("TURNの値が不正です");
        }


        Debug.Log("<COLOR=RED>" + now_type + "</COLOR><COLOR=YELLOW>の打てる場所探索</COLOR>");

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (get_koma_type(i, j) == KOMA_TYPE.None)
                {
                    for (int dir_x = -1; dir_x <= 1; dir_x++)
                    {
                        for (int dir_y = -1; dir_y <= 1; dir_y++)
                        {
                            //探索先が探索元のマスの場合スルー
                            if (i + dir_x == 0 && j + dir_y == 0)
                                continue;
                            //周囲8マスに相手のマスがあったら
                            if (get_koma_type((i + dir_x), (j + dir_y)) == opp_type)
                            {
                                //Debug.Log("<COLOR=YELLOW>y:" + i + " x:" + j + "から" + dir_y + "," + dir_x + "方向へ探索</COLOR>");
                                //延長線上探索
                                int cnt = 2;
                                while (true)
                                {
                                    //範囲外:ループを抜ける
                                    if (i + (dir_x * cnt) < 0 || i + (dir_x * cnt) > 7
                                        || j + (dir_y * cnt) < 0 || j + (dir_y * cnt) > 7)
                                    {
                                        break;
                                    }
                                    //探索先にコマが置かれていないorダミーが置かれている:ループを抜ける
                                    if (get_koma_type(i + (dir_x * cnt), j + (dir_y * cnt)) == KOMA_TYPE.None)
                                    {
                                        break;
                                    }
                                    //探索先に自分のマス:ダミー表示,ループを抜ける
                                    if (get_koma_type(i + (dir_x * cnt), j + (dir_y * cnt)) == now_type)
                                    {
                                        Debug.Log("<COLOR=blue>ダミー[" + i + "," + j + "]:true</COLOR>");
                                        dummy_array[i, j].SetActive(true);
                                        dummy_array[i, j].GetComponent<DummyScript>().koma_type = turnToKomaType(now_turn);
                                        break;
                                    }
                                    if (cnt > 8)
                                    {
                                        Debug.LogError("whileが規定回数以上回りました");
                                        return;
                                    }
                                    cnt++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    KOMA_TYPE turnToKomaType(TURN turn)
    {
        KOMA_TYPE type;

        if (turn == TURN.play_first)
        {
            type = KOMA_TYPE.Black;
        }
        else
        {
            type = KOMA_TYPE.White;
        }
        return type;
    }
    public void SetKoma(int x, int y, KOMA_TYPE type)
    {
        float width = this.transform.localScale.x;
        float height = this.transform.localScale.z;
        float thisX = this.transform.localPosition.x - (width / 2);//boardの左上X
        float thisZ = this.transform.localPosition.z - (height / 2);//boardの左上のZ
        float space_obj = width / 8;

        GameObject obj = (GameObject)Resources.Load(@"koma_pare");
        float objWidth = obj.transform.localScale.x;
        float objHeight = obj.transform.localScale.z;
        float vecX = thisX + space_obj * x + (objWidth / 2);
        float vecZ = thisZ + space_obj * y + (objHeight / 2);
        komaArray[x, y] = Instantiate(obj, new Vector3(vecX, obj.transform.localPosition.y, vecZ), Quaternion.identity);
        komaArray[x, y].GetComponent<komaScript>().x = x;
        komaArray[x, y].GetComponent<komaScript>().y = y;
        komaArray[x, y].GetComponent<komaScript>().type = type;

        komaArray[x, y].GetComponent<komaScript>().setTyoe(get_koma_type(x,y));

        revers(x, y);
    }

    public void revers(int x, int y)
    {
        //置いた色
        KOMA_TYPE set_type = get_koma_type(x, y);
        //相手の色
        KOMA_TYPE opp_type = KOMA_TYPE.None;
        if (set_type == KOMA_TYPE.Black)
            opp_type = KOMA_TYPE.White;
        else
            opp_type = KOMA_TYPE.Black;


        Debug.Log("==========================================================================\n"+
                  "==========================================================================\n"+
                  "<color=blue>置いた場所 x:" + x + " y:" + y + "</ color >\n"+
                  "<color=blue>置いた色:" + set_type + "</color>\n"+
                  "<color=blue>相手の色" + opp_type + "</color>");
        //ひっくり返す処理
        for (int dir_x = -1; dir_x <= 1; dir_x++)
        {
            for (int dir_y = -1; dir_y <= 1; dir_y++)
            {
                //周囲の座標を取得
                if (get_koma_type(x + dir_x, y + dir_y) == opp_type)
                {
                    Debug.Log("<COLOR=GREEN>" + dir_x + "," + dir_y + "方向に" + opp_type + "を確認</COLOR>");
                    int cnt = 2;
                    while (true)
                    {
                        //範囲外:ループを抜ける
                        if (y + (dir_y * cnt) < 0 || y + (dir_y * cnt) > 7
                            || x + (dir_x * cnt) < 0 || x + (dir_x * cnt) > 7)
                        {
                            break;
                        }
                        //探索先にコマが置かれていないorダミーが置かれている:ループを抜ける
                        if (get_koma_type(x + (dir_x * cnt), y + (dir_y * cnt)) == KOMA_TYPE.None)
                        {
                            break;
                        }
                        //探索先に自分のマス:返しながら戻る
                        if (get_koma_type(x + (dir_x * cnt), y + (dir_y * cnt)) == set_type)
                        {
                            Debug.Log("<COLOR=GREEN>" + (x + (dir_x * cnt)) + "," + (y + (dir_y * cnt)) + "に" + set_type + "を確認</COLOR>");
                            cnt--;
                            //打てる
                            while (cnt > 0)
                            {
                                Debug.Log("<COLOR=GREEN>"+(x + (dir_x * cnt)) + ", " + (y + (dir_y * cnt))+"を反転</COLOR>");
                                komaArray[x + (dir_x * cnt), y + (dir_y * cnt)].GetComponent<komaScript>().rotation(true);
                                cnt--;
                            }
                            break;
                        }
                        if (cnt > 8)
                        {
                            Debug.LogError("whileが規定回数以上回りました");
                            return;
                        }
                        cnt++;
                    }
                }
            }
        }
    }
    public KOMA_TYPE get_koma_type(int x, int y)
    {
        if (x < 0 || y < 0 || x >= 8 || y >= 8)
        {
            return KOMA_TYPE.None;
        }
        else if (komaArray[x, y] == null)
        {
            return KOMA_TYPE.None;
        }
        else
        {
            return komaArray[x, y].GetComponent<komaScript>().type;
        }
    }
    public int get_koma_kazu(string color)
    {
        int blackcnt = 0;
        int whitecnt = 0;
        for(int i = 0;i < 8; i++)
        {
            for(int j = 0;j < 8; j++)
            {
                if(komaArray[i,j].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                {
                    blackcnt += 1;
                }
                else
                {
                    whitecnt += 1;
                }
            }
        }
        if (color == "Black")
        {
            return blackcnt;
        }
        else if (color == "White")
        {
            return whitecnt;
        }
        else{
            Debug.LogError("get_koma_kazuでおかしくなってるよ");
            return -1;
        }
    }
}
