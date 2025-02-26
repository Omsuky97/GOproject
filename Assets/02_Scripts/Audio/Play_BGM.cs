using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_BGM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Audio_Manager.instance.Get_BGM_Sound();
    }
}
