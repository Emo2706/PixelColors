using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ButtonPaletteBehaviour : MonoBehaviour
{
    public TMP_Text idText;
    [HideInInspector] public Color currentColor;
    public int idButton;
    [HideInInspector] public Image img;
     public ColorBlock belongingColorBlock;
    public Slider colorPercentage;
    public List<int> InWhatNumberShouldICalculateMyPercentage = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        idButton = 99;
        GameManager.instance.AllButtons.Add(this);
        idText = gameObject.GetComponentInChildren<TMP_Text>();
        img = gameObject.GetComponent<Image>();
        //img.color = currentColor;
        //InWhatNumberShouldICalculateMyPercentage = GameManager.instance.ReturnCheckPointsofPercentage(belongingColorBlock, GameManager.instance.CurrentAllBlocks, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectColor()
    {
        PlayerPaletteSelector.instance.CurrentidColor = idButton;
        GameManager.instance.HighlightAllSelected(idButton, GameManager.instance.CurrentAllBlocks, GobalData.instance.highlightedColor);
        GameManager.instance.ColorPercentage(GameManager.instance.CurrentAllBlocks, belongingColorBlock);
    }


}
