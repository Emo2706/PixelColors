using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaletteSelector : MonoBehaviour
{
    public static PlayerPaletteSelector instance;

    public int CurrentidColor;
    public List<Transform> ButtonsPlacements = new List<Transform>();
    public List<Color> Colores = new List<Color>();
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