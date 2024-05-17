using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaletteSelector : MonoBehaviour
{
    public static PlayerPaletteSelector instance;

    public int CurrentidColor;
    public List<Transform> ButtonsPlacements = new List<Transform>();
    public List<Color> Colores = new List<Color>();

    public int howManyBlocksToPaintAuto;
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
        howManyBlocksToPaintAuto = 1;
    }

    public void UpOrDown(bool up)
    {
        if (up)
            howManyBlocksToPaintAuto++;
        else
        {
            if(howManyBlocksToPaintAuto >= 1)
                howManyBlocksToPaintAuto--;



        }
        HUD.instance.blocksToPaint.text = howManyBlocksToPaintAuto.ToString();


    }

    public void PaintBlock(ColorBlock block)
    {
        block.isPainted = true;
        var spr = block.GetComponent<SpriteRenderer>();
        spr.color = block.color;
        block.ShowingDisplayText.gameObject.SetActive(false);
        //Acá tmb llamaría a un tmp_text que tiene el bloque, etc etc
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
