using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobalData : MonoBehaviour
{

    //Se usa para variables constantes, así no guardo siempre el mismo color en cada cubo por ej
    public static GobalData instance;
    public Color highlightedColor;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
