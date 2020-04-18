using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    public int x;
    public int y;
    public KOMA_TYPE koma_type;
    public System_manager system_manager;
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
            board.SetKoma(x, y,koma_type);
            if (koma_type == KOMA_TYPE.Black)
            {
                system_manager.send(x, y, 1);
            }
            else
            {
                system_manager.send(x, y, 0);
            }
            system_manager.fsend();
        }
        catch { }
        system_manager.turn_change();
        
    }
}
