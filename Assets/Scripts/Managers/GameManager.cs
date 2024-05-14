using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System;
public class GameManager : MonoBehaviour
{
    public enum OrderButtonsBy
    {
        Ids,
        Alphabetic,
        AlphabeticDescending,
        MostColorBlocksPainted,
       
    }
    public static GameManager instance;
    public List<ButtonPaletteBehaviour> AllButtons = new List<ButtonPaletteBehaviour>();
    public List<ButtonPaletteBehaviour> AvailableButtons = new List<ButtonPaletteBehaviour>();
    public LevelSettings currentLevelSettings;
    public List<ListOfColorsBlock> CurrentAllBlocks = new List<ListOfColorsBlock>();
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
           
           
            CurrentAllBlocks.Add(new ListOfColorsBlock());
        }

        
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CorroutineLateStart(currentLevelSettings));

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetProperButtonsPos(AvailableButtons, _buttonsPositions, CurrentAllBlocks, "MostBlockPainted", currentLevelSettings);
        }
    }
    void SetAllColorOptions(TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();
        dropdown.ClearOptions();
        dropdown.options.Add(new TMP_Dropdown.OptionData(text: "All blocks", null));
        dropdown.options.Add(new TMP_Dropdown.OptionData(text: "All painted blocks", null));
        dropdown.options.Add(new TMP_Dropdown.OptionData(text: "All unpainted blocks", null));
        dropdown.options.Add(new TMP_Dropdown.OptionData(text: "All primary colors block left", null));

        for (int i = 0; i < currentLevelSettings.levelStuff.Palette.Count ; i++)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(text: currentLevelSettings.levelStuff.Palette[i].color_Name, null));
        }

        dropdown.RefreshShownValue();
    }

    // Update is called once per frame
   
    IEnumerator CorroutineLateStart(LevelSettings lvS)
    {
        yield return new WaitForSeconds(0.03f);
        SetButtonsColors(AllButtons, lvS.levelStuff.Palette);

        var availableButtons = AllButtons.TakeWhile(x => x.idButton <= lvS.levelStuff.Palette.Count).ToList();

        AvailableButtons = availableButtons;

        var UnAvailableButtons = AllButtons.SkipWhile(x => x.idButton <= lvS.levelStuff.Palette.Count).ToList();

        foreach (var button in UnAvailableButtons)
        {
            button.gameObject.SetActive(false);
        }
        /*for (int i = 0; i < AllButtons.Count; i++)
        {
            if (i < currentLevelSettings.levelStuff.Palette.Count)
            {
                AvailableButtons.Add(AllButtons[i]);
            }
            else AllButtons[i].gameObject.SetActive(false);


        }*/
        SetAllColorOptions(dropdownOptions);
        SetMethodDictionary(showMethod);
        yield return new WaitForSeconds(0.03f);

        //SetProperButtonsPos(OrderByColorName(AvailableButtons, true), _buttonsPositions, OrderButtonsBy.Ids);

    }
    void SetButtonsColors(List<ButtonPaletteBehaviour> buttonsP, List<ColorBlock> ColorsList)
    {
        var list = IDS2(buttonsP);

        for (int i = 0; i < ColorsList.Count; i++)
        {
            
            Debug.Log(i);
            //buttonsP[i].currentColor = lvS.levelStuff.Palette[i].color;
            buttonsP[i].currentColor = ColorsList[i].color;
            buttonsP[i].img.color = ColorsList[i].color;
            buttonsP[i].idButton = i;
            buttonsP[i].belongingColorBlock = ColorsList[i];
            buttonsP[i].gameObject.transform.name = i.ToString();
            buttonsP[i].idText.text = i.ToString();

        }


    }



    List<int>IDS(List<ColorBlock> colorsList)
    {
        var list = colorsList.Select(x => x.ID).ToList();

        return list;
    }

    

    List<int> IDS2(List<ButtonPaletteBehaviour> buttonsp)
    {
        var list = buttonsp.Select(x => x.idButton).ToList();

        return list;
    }



    

    void SetProperButtonsPos(List<ButtonPaletteBehaviour> buttonsP, List<Vector3> positions, List<ListOfColorsBlock> colorsBlock , string order, LevelSettings lvs)
    {

        //int ButtonsAmount = buttonsP.Count();
        //El take está pq seguramente dsp hagamos que se pueda filtrar la paleta por colores tono tipo rojo, azul amarillo etc
        //var col = buttonsP.Take(ButtonsAmount).ToList();
        // var colu = buttonsP;

        if (order == "Ids")
        {
            var col = buttonsP.OrderBy(x => x.idButton).ToList();

            RTsTransform(col, positions);
        }
        else if (order == "Alphabetic")
        {
            var col = buttonsP.OrderBy(x => x.belongingColorBlock.color_Name).ToList();
            RTsTransform(col, positions);

        }
        else if (order == "AlphabeticD")
        {
            var col = buttonsP.OrderByDescending(x => x.belongingColorBlock.color_Name).ToList();
            RTsTransform(col, positions);

        }
        else if (order == "MostBlockPainted")
        {
            //La tupla fue necesaria para poder enlazar el botón con su respectiva lista de bloques de su mismo color
            var TupleList = new List<Tuple<ButtonPaletteBehaviour, List<ColorBlock>>>();
            for (int i = 0; i < lvs.levelStuff.Palette.Count; i++)
            {
                var myTuple = Tuple.Create(buttonsP[i], colorsBlock[i].colorList);

                TupleList.Add(myTuple);
            }

            //var col = colorsBlock.Where(x => x.colorList.Any(x => x.isPainted));
            //Poner then by
            //var col = TupleList.Where(x => x.Item2.Any(x => !x.isPainted)).OrderBy(x => x.Item2.Where(x => !x.isPainted).Count()).ToList();
            var col = TupleList.Where(x => x.Item2.Any(x => !x.isPainted)).OrderBy(x => x.Item2.Where(x => !x.isPainted).Count()).ThenBy(x => x.Item1.idButton).ToList();
            var finalList = new List<ButtonPaletteBehaviour>();
            for (int i = 0; i < col.Count; i++)
            {
                finalList.Add(col[i].Item1);
               

            }
            RTsTransform(finalList, positions);
            //var botonesAMostrar = buttonsP.Where(x => x.idButton == col.SelectMany(x => x.colorList).Any(x => x.ID));
        }
        else if (order == "WarmToCold")
        {
            var TupleList = new List<Tuple<ButtonPaletteBehaviour, ColorBlock>>();

            for (int i = 0; i < lvs.levelStuff.Palette.Count; i++)
            {
                var myTuple = Tuple.Create(buttonsP[i], buttonsP[i].belongingColorBlock);

                TupleList.Add(myTuple);
            }

            var warmColors = TupleList.OrderBy(x => x.Item2.howWarmColdIsIt).Where(x => x.Item2 is IWarm);
            var ColdColors = TupleList.OrderBy(x => x.Item2.howWarmColdIsIt).Where(x => x.Item2 is ICold);
            var AllTogetherInOrder = warmColors.Concat(ColdColors).ToList();

            var finalList = new List<ButtonPaletteBehaviour>();
            for (int i = 0; i < AllTogetherInOrder.Count; i++)
            {
                finalList.Add(AllTogetherInOrder[i].Item1);
            }

            RTsTransform(finalList, positions);

            //var WarmColors = buttonsP.Where(x => x.belongingColorBlock).OfType<IWarm>();
            //var test = buttonsP.Select(b => Tuple.Create<>)

        }



        #region Probar con switch
        /* switch (order)
        {
            case OrderButtonsBy.Ids:

                var col = buttonsP.OrderBy(x => x.idButton).ToList();

                RTsTransform(col, positions);

                break;
            case OrderButtonsBy.Alphabetic:

                var col2 = buttonsP.OrderBy(x => x.belongingColorBlock.color_Name).ToList();
               
                RTsTransform(col2, positions);

                break;
            case OrderButtonsBy.AlphabeticDescending:

                var col3 = buttonsP.OrderByDescending(x => x.belongingColorBlock.color_Name).ToList();
                
                RTsTransform(col3, positions);

                break;
            case OrderButtonsBy.MostColorBlocksPainted:

                var TupleList = new List<Tuple<ButtonPaletteBehaviour, List<ColorBlock>>>();
                for (int i = 0; i < lvs.levelStuff.Palette.Count; i++)
                {
                    var myTuple = Tuple.Create(buttonsP[i], colorsBlock[i].colorList);

                    TupleList.Add(myTuple);
                }

                var col4 = TupleList.Where(x => x.Item2.Any(x => !x.isPainted)).OrderBy(x => x.Item2.Where(x => !x.isPainted).Count()).ToList();
                var finalList = new List<ButtonPaletteBehaviour>();
                for (int i = 0; i < col4.Count; i++)
                {
                    finalList.Add(col4[i].Item1);

                }
                RTsTransform(finalList, positions);


                break;
            default:
                break;
        }*/
        #endregion



    }
    
    void RTsTransform(List<ButtonPaletteBehaviour> col, List<Vector3> positions)
    {
        for (int i = 0; i < col.Count(); i++)
        {
            var Rt = col[i].GetComponent<RectTransform>();


            Rt.localPosition = positions[i];

        }
    }

    public void HighlightAllSelected(int ID, List<ListOfColorsBlock> listofblocks)
    {
        /*foreach (var Block in CurrentAllBlocks[ID]) MANERA VIEJAAAAAAAA
        {
            //Acordarse de devolverlo al color anterior al elegir otro color
            //Fijarse si se puede reemplazar esto por una filtración con Where bool is false y que busque por su ID o algo así LOL
            Block.HighlightBlockSelected();
        }*/

        //RECORDAR QUITAR SIDE EFFECTS O NO CUENTAAA
        var col = CurrentAllBlocks.SelectMany(x => x.colorList).Where(x => x.ID == ID && !x.isPainted).Select(x => x.spr);
        //Se puede recorrer con for/for each?
        foreach (var block in col)
        {
            block.color = GobalData.instance.highlightedColor;
        }
        foreach (var Block in CurrentAllBlocks[ID].colorList) 
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

    
    public void SetMethodDictionary(Dictionary<int , Action<List<ListOfColorsBlock>>> showMethod)
    {
        showMethod.Add(0, BlocksInTotal);
        showMethod.Add(1, BlocksPainted);
        showMethod.Add(2, BlocksUnpainted);
        showMethod.Add(3, HowManyPrimaryColorsBlocksLeft);
       
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

    void RedBlocks(List<ListOfColorsBlock> listofBlocks)
    {
        var redBlocks = listofBlocks.SelectMany(x => x.colorList).Where(x => !x.isPainted).OfType<Block_Red>().Count();

        

        Debug.Log(redBlocks);
    }

    void BlueBlocks(List<ListOfColorsBlock> listofBlocks)
    {
        var blueBlocks = listofBlocks.SelectMany(x => x.colorList).Where(x => !x.isPainted).OfType<Block_Blue>().Count();

        Debug.Log(blueBlocks);
    }


    void YellowBlocks(List<ListOfColorsBlock> listofBlocks)
    {
        var yellowBlocks = listofBlocks.SelectMany(x => x.colorList).Where(x=>!x.isPainted).OfType<Block_Yellow>().Count();

        Debug.Log(yellowBlocks);
    }

    void HowManyPrimaryColorsBlocksLeft(List<ListOfColorsBlock> listofBlocks)
    {
        var yellowBlocks = listofBlocks.SelectMany(x => x.colorList).Where(x => !x.isPainted).OfType<Block_Yellow>().Count();
        var blueBlocks = listofBlocks.SelectMany(x => x.colorList).Where(x => !x.isPainted).OfType<Block_Blue>().Count();
        var redBlocks = listofBlocks.SelectMany(x => x.colorList).Where(x => !x.isPainted).OfType<Block_Red>().Count();

        int allPrimaryBlocks = yellowBlocks + blueBlocks + redBlocks;
       //Text = allprimaryBlocks "in total" + yellowBlocks + " yellow blocks missing" + blueblocks etc etc


    }


    public void ShowChosenBlocks(List<ListOfColorsBlock> listofblocks, int Option)
    {
        //showMethod[Option]();
        showMethod[Option](listofblocks);
    }

    public void PaintAutomatically(int idToPaint, List<ListOfColorsBlock> listofblocks, LevelSettings lvS, int BlocksamountToPaint)
    {

        if (BlocksamountToPaint == 1)
        {

            var col = listofblocks.SelectMany(x => x.colorList)
                .Where(x => x.GetType() == lvS.levelStuff.Palette[idToPaint].GetType() && !x.isPainted)
                .FirstOrDefault();
            col.Paint();
        }
        else
        {
            var col = listofblocks.SelectMany(x => x.colorList)
                .Where(x => x.GetType() == lvS.levelStuff.Palette[idToPaint].GetType() && !x.isPainted)
                .Take(BlocksamountToPaint);
            foreach (var Block in col)
            {
                Block.Paint();
            }
        }
        

    }
}
