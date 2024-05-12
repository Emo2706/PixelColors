using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonsExtraFunctions : MonoBehaviour
{
    //Preguntar si así está bien?? si no es imposible no hacer side effects con botones

    // Start is called before the first frame update
    Dictionary<int, Action> showMethod;
    Action _currentMethod;
    int currentOption;

    
    public void HowManyBlocksAreLeft()
    {
        //GameManager.instance.HowManyBlocksLeft(GameManager.instance.CurrentAllBlocks2);
        GameManager.instance.ShowChosenBlocks(GameManager.instance.CurrentAllBlocks2, currentOption);
    }
    public void PaintAutomatically()
    {
        GameManager.instance.PaintAutomatically(PlayerPaletteSelector.instance.CurrentidColor,
            GameManager.instance.CurrentAllBlocks2, GameManager.instance.currentLevelSettings, PlayerPaletteSelector.instance.howManyBlocksToPaintAuto);
    }
    public void OnOptionChange(TMPro.TMP_Dropdown dropdown)
    {
        int OptionChosen = dropdown.value;
        //_currentMethod = showMethod[OptionChosen];
        currentOption = OptionChosen;
    }
}
