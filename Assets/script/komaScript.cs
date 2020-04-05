using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KOMA_TYPE {Black,White,None}
public class komaScript : MonoBehaviour
{
    public KOMA_TYPE type;
    public int x;
    public int y;
    public AnimController anim;


    // Start is called before the first frame update
    void Start()
    {
        Texture2D m_texture;
        m_texture = new Texture2D(128, 128, TextureFormat.ARGB32, false);
        float space = m_texture.height / 2 ;

        for (int y = 0; y < m_texture.height; y++)
        {
            for (int x = 0; x < m_texture.width; x++)
            {
                if ( y > space )
                    m_texture.SetPixel(x, y, Color.black);
                else 
                    m_texture.SetPixel(x, y, Color.white);
            }
        }
        m_texture.Apply();
        GetComponent<Renderer>().material.mainTexture = m_texture;

        anim = transform.parent.gameObject.GetComponent<AnimController>();
    }
    public void rotation()
    {
        anim.koma_rotation();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
