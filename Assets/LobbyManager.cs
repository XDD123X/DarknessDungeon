using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Invoke("INITEVERYTHING", 0.1f);
    }

    void INITEVERYTHING()
    {
        ItemManagement.LoadRoundNumber(); 
        ItemManagement.Load(ItemManagement.USER_DATA);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
