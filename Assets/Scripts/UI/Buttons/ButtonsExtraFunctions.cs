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
        GameManager.instance.ShowChosenBlocks(GameManager.instance.CurrentAllBlocks, currentOption);
    }
    public void PaintAutomatically()
    {
        GameManager.instance.PaintAutomatically(PlayerPaletteSelector.instance.CurrentidColor,
            GameManager.instance.CurrentAllBlocks, GameManager.instance.currentLevelSettings, PlayerPaletteSelector.instance.howManyBlocksToPaintAuto);

        GameManager.instance.ReturnSliderBarOfButton(GameManager.instance.AvailableButtons, PlayerPaletteSelector.instance.CurrentidColor).value =
                        GameManager.instance.ColorPercentage(GameManager.instance.CurrentAllBlocks, GameManager.instance.currentLevelSettings.levelStuff.Palette[PlayerPaletteSelector.instance.CurrentidColor]);
    }
    public void OnOptionChange(TMPro.TMP_Dropdown dropdown)
    {
        int OptionChosen = dropdown.value;
        //_currentMethod = showMethod[OptionChosen];
        currentOption = OptionChosen;
    }

    public void OrderPalette(string orderBy)
    {
        GameManager.instance.SetProperButtonsPos(GameManager.instance.AvailableButtons, 
            GameManager.instance._buttonsPositions,GameManager.instance.CurrentAllBlocks, orderBy, 
            GameManager.instance.currentLevelSettings);

    }
}
