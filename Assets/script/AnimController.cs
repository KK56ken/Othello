using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    float defaultY;
    // Start is called before the first frame update
    void Start()
    {
        defaultY = transform.position.y;
        Debug.Log("AnimControllerのコンストラクタ");
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void koma_rotation(bool forced)
    {
        if (forced || defaultY == transform.position.y)
        {
            KOMA_TYPE type = GetComponent<komaScript>().type;
            if (type == KOMA_TYPE.Black)
            {
                Debug.Log("BlackToWhite");
                GetComponent<Animator>().Play("BlackToWhite", 0);
                GetComponent<komaScript>().type = KOMA_TYPE.White;
            }
            else
            {
                Debug.Log("WhiteToBlack");
                GetComponent<Animator>().Play("WhiteToBlack", 0);
                GetComponent<komaScript>().type = KOMA_TYPE.Black;
            }
        }
    }
}
