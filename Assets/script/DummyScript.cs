using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    public int x;
    public int y;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void setKoma()
    {
        board board = GameObject.Find("Board").GetComponent<board>();
        try
        {
            board.SetKoma(x, y);
        }
        catch { }
        Destroy(gameObject);
    }
}
