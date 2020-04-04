using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board : MonoBehaviour
{

    GameObject[,] komaArray = new GameObject[8, 8];

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

        System_manager system = GameObject.Find("system").GetComponent<System_manager>();

        float width = this.transform.localScale.x;
        float height = this.transform.localScale.z;
        float thisX = this.transform.localPosition.x - (width / 2);//boardの左上X
        float thisZ = this.transform.localPosition.z - (height / 2);//boardの左上のZ
        float space_obj = width / 8;
        for (int i = 3; i <= 4; i++)
        {
            for (int j = 3; j <= 4; j++)
            {
                GameObject obj = (GameObject)Resources.Load(@"koma");
                float objWidth = obj.transform.localScale.x;
                float objHeight = obj.transform.localScale.z;
                float vecX = thisX + space_obj * i + (objWidth / 2);
                float vecZ = thisZ + space_obj * j + (objHeight / 2);
                komaArray[i, j] = Instantiate(obj, new Vector3(vecX, obj.transform.localPosition.y, vecZ), Quaternion.identity);
                if ((i == 3 && j == 4) || (i == 4 && j == 3))
                {
                    //コマを白にする
                    komaArray[i, j].GetComponent<komaScript>().rotation();
                    komaArray[i, j].GetComponent<komaScript>().type = KOMA_TYPE.White;
                    komaArray[i, j].GetComponent<komaScript>().x = i;
                    komaArray[i, j].GetComponent<komaScript>().y = j;
                 
                }
                else
                {
                    //コマを黒にする
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
                if (!(i == 3 && j == 4) && !(i == 4 && j == 3) && !(i == 3 && j == 3) && !(i == 4 && j == 4))
                {
                    if (system.get_turn() == TURN.play_first)
                    {
                        if (get_koma_type(i, j) == KOMA_TYPE.White)
                        {

                        }
                        else if (get_koma_type(i, j) == KOMA_TYPE.Black)
                        {

                        }
                    }
                    GameObject dummy = (GameObject)Resources.Load(@"dummy");
                    float dummyWidth = dummy.transform.localScale.x;
                    float dummyHeight = dummy.transform.localScale.z;
                    float vecX = thisX + space_obj * i + (dummyWidth / 2);
                    float vecZ = thisZ + space_obj * j + (dummyHeight / 2);
                    dummy.GetComponent<DummyScript>().x = i;
                    dummy.GetComponent<DummyScript>().y = j;

                    Instantiate(dummy, new Vector3(vecX, dummy.transform.localPosition.y, vecZ), Quaternion.identity);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void SetKoma(int x, int y)
    {
        float width = this.transform.localScale.x;
        float height = this.transform.localScale.z;
        float thisX = this.transform.localPosition.x - (width / 2);//boardの左上X
        float thisZ = this.transform.localPosition.z - (height / 2);//boardの左上のZ
        float space_obj = width / 8;

        GameObject obj = (GameObject)Resources.Load(@"koma");
        float objWidth = obj.transform.localScale.x;
        float objHeight = obj.transform.localScale.z;
        float vecX = thisX + space_obj * x + (objWidth / 2);
        float vecZ = thisZ + space_obj * y + (objHeight / 2);
        komaArray[x, y] = Instantiate(obj, new Vector3(vecX, obj.transform.localPosition.y, vecZ), Quaternion.identity);
        revers(x, y);
    }
    public void check_revers(int x, int y)
    {

    }
    public void revers(int x, int y)
    {
        Debug.Log("<color=blue>置いた場所 x:" + x + " y:" + y+"</color>");
        //白をひっくり返す処理
        if (komaArray[x, y].GetComponent<komaScript>().type == KOMA_TYPE.Black)
        {
            
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    //周囲の座標を取得
                    int chenge_x = x + i;
                    int chenge_y = y + j;
                    int cnt = 1;
                    bool revers_t = false;

                    if (get_koma_type(chenge_x,chenge_y) == KOMA_TYPE.White)
                    {
                        Debug.Log("i = " + i + " j = " + j);
                        Debug.Log("chenge_x = " + chenge_x + " chenge_y = " + chenge_y);
              
                        //ひっくり返せるか確認処理
                        while (komaArray[chenge_x + (i * cnt),chenge_y + (j * cnt)].GetComponent<komaScript>().type == KOMA_TYPE.White)
                        { 
                            cnt += 1;
                        }
                        while(cnt != 0)
                        {
                            Debug.Log((x + (i * cnt)) +"," + (y + (j * cnt)));
                            komaArray[x + (i * cnt), y + (j * cnt)].GetComponent<komaScript>().rotation();
                            komaArray[x + (i * cnt), y + (j * cnt)].GetComponent<komaScript>().type = KOMA_TYPE.Black;
                            cnt--;
                        } 
                    }
                }
            }
        }
        //黒をひっくり返す処理
        else if (komaArray[x, y].GetComponent<komaScript>().type == KOMA_TYPE.White)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    //周囲の座標を取得
                    int chenge_x = x + i;
                    int chenge_y = y + j;
                    int cnt = 1;
                    bool revers_t = false;

                    if (get_koma_type(chenge_x, chenge_y) == KOMA_TYPE.Black)
                    {
                        Debug.Log("i = " + i + " j = " + j);
                        Debug.Log("chenge_x = " + chenge_x + " chenge_y = " + chenge_y);

                        //ひっくり返せるか確認処理
                        while (komaArray[chenge_x + (i * cnt), chenge_y + (j * cnt)].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                        {
                            cnt += 1;
                        }
                        while (cnt != 0)
                        {
                            Debug.Log((x + (i * cnt)) + "," + (y + (j * cnt)));
                            komaArray[x + (i * cnt), y + (j * cnt)].GetComponent<komaScript>().rotation();
                            komaArray[x + (i * cnt), y + (j * cnt)].GetComponent<komaScript>().type = KOMA_TYPE.White;
                            cnt--;
                        }
                    }
                }
            }

        }
    }
    public KOMA_TYPE get_koma_type(int x ,int y)
    {
        if (komaArray[x,y] == null)
        {
            return KOMA_TYPE.None;
        }
        else
        {
            return komaArray[x, y].GetComponent<komaScript>().type;
        }
    }
}
