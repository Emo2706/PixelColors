using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class ColorBlock : MonoBehaviour
{
    public Color color;
     public int ID;
    public string color_Name;
    public int brightnessLevel;
    [HideInInspector] public bool isPainted;

    [HideInInspector] public SpriteRenderer spr;
    [SerializeField] TMP_Text ShowingDisplayText;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        spr = gameObject.GetComponent<SpriteRenderer>();
        spr.color = Color.white;
        addToGMList();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //TESTEO BORRAR DSP
       
    }
    //Como evitar sideEffects acá???
    public virtual void Paint()
    {
        isPainted = true;
        spr.color = color;

    }
    //Preguntar si todo esto cuenta como side effects, por que querría que todo sea con parámetros???
    public void HighlightBlockSelected()
    {
        if (isPainted) return;
        spr.color = GobalData.instance.highlightedColor;

        //Preguntarle a emi si conviene hacer una coleccion acá que sea un where, donde 
    }

    protected void addToGMList()
    {
        for (int i = 0; i < GameManager.instance.currentLevelSettings.levelStuff.Palette.Count; i++)
        {
            if (this.GetType() == GameManager.instance.currentLevelSettings.levelStuff.Palette[i].GetType())
            {

                //GameManager.instance.CurrentAllBlocks.colorsList[i].block.Add(this);

                //GameManager.instance.CurrentAllBlocks[i].Add(this);
                GameManager.instance.CurrentAllBlocks2[i].colorList.Add(this);
                ID = i;
                ShowingDisplayText.text = ID.ToString();
            }
        }
    }
}
