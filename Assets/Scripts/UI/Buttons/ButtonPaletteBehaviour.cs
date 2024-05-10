using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ButtonPaletteBehaviour : MonoBehaviour
{
    TMP_Text _idText;
    [HideInInspector] public Color currentColor;
    public int idButton;
    [HideInInspector] public Image img;
     public ColorBlock belongingColorBlock;
    // Start is called before the first frame update
    void Start()
    {
        idButton = 99;
        GameManager.instance.AllButtons.Add(this);
        _idText = gameObject.GetComponentInChildren<TMP_Text>();
        img = gameObject.GetComponent<Image>();
        //img.color = currentColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectColor()
    {
        PlayerPaletteSelector.instance.CurrentidColor = idButton;
        GameManager.instance.HighlightAllSelected(idButton);
    }


}
