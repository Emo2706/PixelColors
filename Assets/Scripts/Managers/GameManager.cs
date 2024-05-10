using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<ButtonPaletteBehaviour> AllButtons = new List<ButtonPaletteBehaviour>();
    public List<ButtonPaletteBehaviour> AvailableButtons = new List<ButtonPaletteBehaviour>();
    public LevelSettings currentLevelSettings;
    // public ListOfColorsBlock CurrentAllBlocks = new ListOfColorsBlock();
    public List<List<ColorBlock>> CurrentAllBlocks = new List<List<ColorBlock>>();
    public List<ListOfColorsBlock> CurrentAllBlocks2 = new List<ListOfColorsBlock>();
    public TMP_Dropdown dropdownOptions;

   [SerializeField] List<Vector3> _buttonsPositions;

    Dictionary<int, Action<List<ListOfColorsBlock>>> showMethod = new Dictionary<int, Action<List<ListOfColorsBlock>>>();


    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        
        for (int i = 0; i < currentLevelSettings.levelStuff.Palette.Count; i++)
        {
           
            //CurrentAllBlocks.Add(new List<ColorBlock>());
            //CurrentAllBlocks2.col
            CurrentAllBlocks2.Add(new ListOfColorsBlock());
        }

        
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CorroutineLateStart());

        //PARA ACOMODAR LOS BOTONES POR ID SE PUEDE HACER QUE ORDENE DE MENOR A MAYOR LOS ID Y LUEGO QUE LO CONVIERTA EN UNA LISTA, Y DE AHÍ, RECORRA UN FOR DONDE SU TRANSFORM CAMBIE EN BASE A LA I DE UN ARRAY DE TRANSFORMS
    }
    

    void SetAllColorOptions()
    {
        dropdownOptions.ClearOptions();
        dropdownOptions.ClearOptions();
        dropdownOptions.options.Add(new TMP_Dropdown.OptionData(text: "All blocks", null));
        dropdownOptions.options.Add(new TMP_Dropdown.OptionData(text: "All painted blocks", null));
        dropdownOptions.options.Add(new TMP_Dropdown.OptionData(text: "All unpainted blocks", null));

        for (int i = 0; i < currentLevelSettings.levelStuff.Palette.Count ; i++)
        {
            dropdownOptions.options.Add(new TMP_Dropdown.OptionData(text: currentLevelSettings.levelStuff.Palette[i].color_Name, null));
        }

        dropdownOptions.RefreshShownValue();
    }

    // Update is called once per frame
   
    IEnumerator CorroutineLateStart()
    {
        yield return new WaitForSeconds(0.03f);
        SetButtonsColors(AllButtons, currentLevelSettings.levelStuff.Palette);
        for (int i = 0; i < currentLevelSettings.levelStuff.Palette.Count; i++)
        {
            AvailableButtons.Add(AllButtons[i]);
        }
        SetAllColorOptions();
        SetMethodDictionary();
        yield return new WaitForSeconds(0.03f);

        SetProperButtonsPos(OrderByColorName(AvailableButtons, true), _buttonsPositions);

        
    }
    void SetButtonsColors(List<ButtonPaletteBehaviour> buttonsP, List<ColorBlock> ColorsList)
    {
        for (int i = 0; i < ColorsList.Count; i++)
        {
            
            Debug.Log(i);
            //buttonsP[i].currentColor = lvS.levelStuff.Palette[i].color;
            buttonsP[i].currentColor = ColorsList[i].color;
            buttonsP[i].img.color = ColorsList[i].color;
            buttonsP[i].idButton = i;
            buttonsP[i].belongingColorBlock = ColorsList[i];
            buttonsP[i].gameObject.transform.name = i.ToString();


        }


    }

    /*ACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA PREGUNTALEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
     * 
     * 
     * ACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA PREGUNTALEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
     * 
     * ACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA PREGUNTALEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
     */
    IEnumerable<ButtonPaletteBehaviour> OrderByColorName(List<ButtonPaletteBehaviour> buttonsP, bool descending)
    {
        var col = buttonsP.OrderBy(x => x.belongingColorBlock.color_Name);
        if (descending)
        {
            col = buttonsP.OrderByDescending(x => x.belongingColorBlock.color_Name);
        }
        
        
        return col;
    }
    IEnumerable<ButtonPaletteBehaviour> OrderByIds(List<ButtonPaletteBehaviour> buttonsP)
    {
        var col = buttonsP.OrderBy(x => x.idButton);
        return col;
    }
    void SetProperButtonsPos(IEnumerable<ButtonPaletteBehaviour> buttonsP, List<Vector3> positions)
    {
       
        int ButtonsAmount = buttonsP.Count();
        //El take está pq seguramente dsp hagamos que se pueda filtrar la paleta por colores tono tipo rojo, azul amarillo etc
        var col = buttonsP.Take(ButtonsAmount).ToList();
        
        for (int i = 0; i < col.Count; i++)
        {
            var Rt = col[i].GetComponent<RectTransform>();

            
            Rt.localPosition = positions[i];
            
        }

    }
    

    public void HighlightAllSelected(int ID)
    {
        /*foreach (var Block in CurrentAllBlocks[ID]) MANERA VIEJAAAAAAAA
        {
            //Acordarse de devolverlo al color anterior al elegir otro color
            //Fijarse si se puede reemplazar esto por una filtración con Where bool is false y que busque por su ID o algo así LOL
            Block.HighlightBlockSelected();
        }*/

        //RECORDAR QUITAR SIDE EFFECTS
        var col = CurrentAllBlocks2.SelectMany(x => x.colorList).Where(x => x.ID == ID);
        //Se puede recorrer con for/for each?

        foreach (var Block in CurrentAllBlocks2[ID].colorList) 
        {
            //Acordarse de devolverlo al color anterior al elegir otro color
            //Fijarse si se puede reemplazar esto por una filtración con Where bool is false y que busque por su ID o algo así LOL
            Block.HighlightBlockSelected();
        }

    }

    //Es imposible hacer esto sin sideEffects si lo quiero llamar desde un botón
    public void HowManyBlocksLeft(List<ListOfColorsBlock> listofblocks)
    {
        //var SM = listofblocks.SelectMany(x => x.All<ColorBlock>());
        var SelectMany = listofblocks.SelectMany(x => x.colorList).Count();

        Debug.Log(SelectMany);
        
    }

    //Remover SideEffects???
    public void SetMethodDictionary()
    {
        showMethod.Add(0, BlocksInTotal);
        showMethod.Add(1, BlocksPainted);
        showMethod.Add(2, BlocksUnpainted);
    }

    

    void BlocksInTotal(List<ListOfColorsBlock> listofBlocks)
    {
        var SelectMany = listofBlocks.SelectMany(x => x.colorList).Count();

        Debug.Log(SelectMany);
    }

    void BlocksPainted(List<ListOfColorsBlock> listofBlocks)
    {
        var BlocksPaintedInTotal = listofBlocks.SelectMany(x => x.colorList).Where(x => x.isPainted).Count();

        Debug.Log(BlocksPaintedInTotal);

    }
    void BlocksUnpainted(List<ListOfColorsBlock> listofBlocks)
    {
        var BlocksUnPaintedInTotal = listofBlocks.SelectMany(x => x.colorList).Where(x => !x.isPainted).Count();
        Debug.Log(BlocksUnPaintedInTotal);

    }

    public void ShowChosenBlocks(List<ListOfColorsBlock> listofblocks, int Option)
    {
        //showMethod[Option]();
        showMethod[Option](listofblocks);
    }
}
