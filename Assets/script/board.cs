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
                    komaArray[i, j].GetComponent<komaScript>().x = j;
                    komaArray[i, j].GetComponent<komaScript>().y = i;
                }
                else
                {
                    //コマを黒にする
                    komaArray[i, j].GetComponent<komaScript>().type = KOMA_TYPE.Black;
                    komaArray[i, j].GetComponent<komaScript>().x = j;
                    komaArray[i, j].GetComponent<komaScript>().y = i;
                }
            }
        }
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (!(i == 3 && j == 4) && !(i == 4 && j == 3) && !(i == 3 && j == 3) && !(i == 4 && j == 4))
                {
                    GameObject dummy = (GameObject)Resources.Load(@"dummy");
                    float dummyWidth = dummy.transform.localScale.x;
                    float dummyHeight = dummy.transform.localScale.z;
                    float vecX = thisX + space_obj * i + (dummyWidth / 2);
                    float vecZ = thisZ + space_obj * j + (dummyHeight / 2);
                    dummy.GetComponent<DummyScript>().x = j;
                    dummy.GetComponent<DummyScript>().y = i;

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
        float vecX = thisX + space_obj * y + (objWidth / 2);
        float vecZ = thisZ + space_obj * x + (objHeight / 2);
        komaArray[x, y] = Instantiate(obj, new Vector3(vecX, obj.transform.localPosition.y, vecZ), Quaternion.identity);
        revers(x, y);
    }
    public void revers(int x, int y)
    {
        //白をひっくり返す処理
        if (komaArray[x, y].GetComponent<komaScript>().type == KOMA_TYPE.Black)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    //周囲の座標を取得
                    int nx = x - i;
                    int ny = y - j;

                    if (komaArray[nx, ny] != null && komaArray[nx, ny].GetComponent<komaScript>().type == KOMA_TYPE.White)
                    {
                        //置く場所と白の判定がされた位置とのx,yの差をとる
                        int difx = x - nx;
                        int dify = y - ny;
                        int revers_num;
                        if (difx == -1 && dify == -1)
                        {
                            while (komaArray[nx, ny].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                            {
                                //次の場所が黒だった場合にそれまでの白の場所を黒にする
                                if (komaArray[nx - 1, ny - 1].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                                {
                                    revers_num = x - nx;
                                    revers_num = Mathf.Abs(revers_num);
                                    while (revers_num != 0)
                                    {
                                        komaArray[nx + revers_num - 1, ny + revers_num - 1].GetComponent<komaScript>().type = KOMA_TYPE.Black;
                                        revers_num = revers_num - 1;
                                    }
                                    break;
                                }
                                nx = nx - 1;
                                ny = ny - 1;
                            }
                        }
                        if (difx == 1 && dify == 0)
                        {
                            while (komaArray[nx, ny].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                            {
                                //次の場所が黒だった場合にそれまでの白の場所を黒にする
                                if (komaArray[nx - 1, ny].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                                {
                                    revers_num = x - nx;
                                    revers_num = Mathf.Abs(revers_num);
                                    while (revers_num != 0)
                                    {
                                        komaArray[nx + revers_num - 1, ny].GetComponent<komaScript>().type = KOMA_TYPE.Black;
                                        revers_num = revers_num - 1;
                                    }
                                    break;
                                }
                                nx = nx - 1;
                            }
                        }
                        if (difx == 1 && dify == -1)
                        {
                            while (komaArray[nx, ny].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                            {
                                //次の場所が黒だった場合にそれまでの白の場所を黒にする
                                if (komaArray[nx + 1, ny - 1].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                                {
                                    revers_num = x - nx;
                                    revers_num = Mathf.Abs(revers_num);
                                    while (revers_num != 0)
                                    {
                                        komaArray[nx + revers_num - 1, ny + revers_num - 1].GetComponent<komaScript>().type = KOMA_TYPE.Black;
                                        revers_num = revers_num - 1;
                                    }
                                    break;
                                }
                                nx = nx + 1;
                                ny = ny - 1;
                            }
                        }
                        if (difx == 0 && dify == 1)
                        {
                            while (komaArray[nx, ny].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                            {
                                //次の場所が黒だった場合にそれまでの白の場所を黒にする
                                if (komaArray[nx, ny - 1].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                                {
                                    revers_num = y - ny;
                                    revers_num = Mathf.Abs(revers_num);
                                    while (revers_num != 0)
                                    {
                                        komaArray[nx, ny + revers_num - 1].GetComponent<komaScript>().type = KOMA_TYPE.Black;
                                        revers_num = revers_num - 1;
                                    }
                                    break;
                                }
                                ny = ny - 1;
                            }
                        }
                        if (difx == 0 && dify == -1)
                        {
                            while (komaArray[nx, ny].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                            {
                                //次の場所が黒だった場合にそれまでの白の場所を黒にする
                                if (komaArray[nx, ny - 1].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                                {
                                    revers_num = y - ny;
                                    revers_num = Mathf.Abs(revers_num);
                                    while (revers_num != 0)
                                    {
                                        komaArray[nx, ny + revers_num - 1].GetComponent<komaScript>().type = KOMA_TYPE.Black;
                                        revers_num = revers_num - 1;
                                    }
                                    break;
                                }
                                nx = nx;
                                ny = ny - 1;
                            }
                        }
                        if (difx == 1 && dify == -1)
                        {
                            while (komaArray[nx, ny].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                            {
                                //次の場所が黒だった場合にそれまでの白の場所を黒にする
                                if (komaArray[nx, ny - 1].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                                {
                                    revers_num = x - nx;
                                    revers_num = Mathf.Abs(revers_num);
                                    while (revers_num != 0)
                                    {
                                        komaArray[nx, ny + revers_num - 1].GetComponent<komaScript>().type = KOMA_TYPE.Black;
                                        revers_num = revers_num - 1;
                                    }
                                    break;
                                }
                                nx = nx;
                                ny = ny - 1;
                            }
                        }
                        if (difx == 1 && dify == 0)
                        {
                            while (komaArray[nx, ny].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                            {
                                //次の場所が黒だった場合にそれまでの白の場所を黒にする
                                if (komaArray[nx, ny - 1].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                                {
                                    revers_num = y - ny;
                                    revers_num = Mathf.Abs(revers_num);
                                    while (revers_num != 0)
                                    {
                                        komaArray[nx, ny + revers_num - 1].GetComponent<komaScript>().type = KOMA_TYPE.Black;
                                        revers_num = revers_num - 1;
                                    }
                                    break;
                                }
                                nx = nx;
                                ny = ny - 1;
                            }
                        }
                        if (difx == 1 && dify == 1)
                        {
                            while (komaArray[nx, ny].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                            {
                                //次の場所が黒だった場合にそれまでの白の場所を黒にする
                                if (komaArray[nx, ny - 1].GetComponent<komaScript>().type == KOMA_TYPE.Black)
                                {
                                    revers_num = y - ny;
                                    revers_num = Mathf.Abs(revers_num);
                                    while (revers_num != 0)
                                    {
                                        komaArray[nx, ny + revers_num - 1].GetComponent<komaScript>().type = KOMA_TYPE.Black;
                                        revers_num = revers_num - 1;
                                    }
                                    break;
                                }
                                nx = nx;
                                ny = ny - 1;
                            }
                        }
                    }
                }
            }
        }
        //黒をひっくり返す処理
        else if (komaArray[x, y].GetComponent<komaScript>().type == KOMA_TYPE.White)
        {

        }


    }
}
