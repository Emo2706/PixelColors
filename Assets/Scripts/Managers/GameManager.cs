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
    public List<ListOfColorsBlock> CurrentAllBlocks = new List<ListOfColorsBlock>();
    public TMP_Dropdown dropdownOptions;

   [SerializeField] public List<Vector3> _buttonsPositions;

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
        StartCoroutine(CorroutineLateStart(currentLevelSettings, AllButtons, AvailableButtons));

    }

    private void Update()
    {
       
    }
    void SetAllColorOptions(TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();
        dropdown.ClearOptions();
       

        for (int i = 0; i < currentLevelSettings.levelStuff.Palette.Count ; i++)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(text: currentLevelSettings.levelStuff.Palette[i].color_Name, null));
        }

        dropdown.RefreshShownValue();
    }

    // Update is called once per frame
   
    IEnumerator CorroutineLateStart(LevelSettings lvS, List<ButtonPaletteBehaviour> buttonsP, List<ButtonPaletteBehaviour> avButtons)
    {
        yield return new WaitForSeconds(0.03f);
        SetButtonsColors(buttonsP, lvS.levelStuff.Palette);

        var availableButtons = buttonsP.TakeWhile(x => x.idButton <= lvS.levelStuff.Palette.Count).ToList();

        AvailableButtons = availableButtons;

        var UnAvailableButtons = buttonsP.SkipWhile(x => x.idButton <= lvS.levelStuff.Palette.Count).ToList();

        foreach (var button in UnAvailableButtons)
        {
            button.gameObject.SetActive(false);
        }
       
        SetAllColorOptions(dropdownOptions);
        SetProperButtonsPos(AvailableButtons, _buttonsPositions, CurrentAllBlocks, "Ids", lvS);
        yield return new WaitForSeconds(0.03f);


    }
    void SetButtonsColors(List<ButtonPaletteBehaviour> buttonsP, List<ColorBlock> ColorsList)
    {
        

        for (int i = 0; i < ColorsList.Count; i++)
        {
            
           
            //buttonsP[i].currentColor = lvS.levelStuff.Palette[i].color;
            buttonsP[i].currentColor = ColorsList[i].color;
            buttonsP[i].img.color = ColorsList[i].color;
            buttonsP[i].idButton = i;
            buttonsP[i].belongingColorBlock = ColorsList[i];
            buttonsP[i].gameObject.transform.name = i.ToString();
            buttonsP[i].idText.text = i.ToString();

        }


    }


    public void SetProperButtonsPos(List<ButtonPaletteBehaviour> buttonsP, List<Vector3> positions, List<ListOfColorsBlock> colorsBlock , string order, LevelSettings lvs)
    {

       

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

            
            var col = TupleList.Where(x => x.Item2.Any(x => !x.isPainted)).OrderByDescending(x => x.Item2.Where(x => !x.isPainted).Count()).ThenBy(x => x.Item1.idButton).ToList();
            var finalList = new List<ButtonPaletteBehaviour>();
            for (int i = 0; i < col.Count; i++)
            {
                finalList.Add(col[i].Item1);
                //Debug.Log(col[i].Item1.gameObject.name.ToString() + col[i].Item2.SelectMany(x => x.color_Name).FirstOrDefault().ToString()); ;

            }
            RTsTransform(finalList, positions);
           
        }
        else if (order == "WarmToCold")
        {
            var TupleList = new List<Tuple<ButtonPaletteBehaviour, ColorBlock>>();

            for (int i = 0; i < lvs.levelStuff.Palette.Count; i++)
            {
                var myTuple = Tuple.Create(buttonsP[i], buttonsP[i].belongingColorBlock);

                TupleList.Add(myTuple);
            }
            //Acordarse de ordenar tmb los colores ni calidos ni frios bro
            var warmColors = TupleList.OrderBy(x => x.Item2.howWarmColdIsIt).Where(x => x.Item2 is IWarm);
            var ColdColors = TupleList.OrderBy(x => x.Item2.howWarmColdIsIt).Where(x => x.Item2 is ICold);
            var NeutralColors = TupleList.Where(x => x.Item2 is not ICold && x.Item2 is not IWarm);
            
            var AllTogetherInOrder = warmColors.Concat(ColdColors.Concat(NeutralColors)).ToList();

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

    public void HighlightAllSelected(int ID, List<ListOfColorsBlock> listofblocks, Color colorHightlight)
    {
       
        var col = listofblocks.SelectMany(x => x.colorList).Where(x => x.ID == ID && !x.isPainted).Select(x => x.spr);
       
        foreach (var block in col)
        {
            block.color = colorHightlight;
        }
       

    }

    

    

    public Tuple <int,int,int> HowManyPrimaryColorsBlocksLeft(List<ListOfColorsBlock> listofBlocks)
    {
        var yellowBlocks = listofBlocks.SelectMany(x => x.colorList).Where(x => x.isPainted).OfType<Block_Yellow>().Count();
        var blueBlocks = listofBlocks.SelectMany(x => x.colorList).Where(x => x.isPainted).OfType<Block_Blue>().Count();
        var redBlocks = listofBlocks.SelectMany(x => x.colorList).Where(x => x.isPainted).OfType<Block_Red>().Count();

        return Tuple.Create(yellowBlocks, blueBlocks, redBlocks);
       //Text = allprimaryBlocks "in total" + yellowBlocks + " yellow blocks missing" + blueblocks etc etc


    }
    public Slider ReturnSliderBarOfButton(List<ButtonPaletteBehaviour> buttonsP, int id)
    {
        var slider = buttonsP.Where(x => x.idButton == id).Select(x => x.colorPercentage).FirstOrDefault();
        return slider;
    }
    
    public Tuple<int,int,int> PixelArtStatistics(List<ListOfColorsBlock> listofBlocks)
    {
        
        //var acum = Tuple.Create(0, 0, 0);

        var Statitstics = listofBlocks.SelectMany(x => x.colorList).Aggregate(Tuple.Create(0, 0, 0), (acum, current) =>
         {
             int BlocksInTotal = acum.Item1 + 1;
             int PaintedBlocks = acum.Item2;
             int UnPaintedBlocks = acum.Item3;
             if (current.isPainted)
             {
                 PaintedBlocks++;
             }
             else UnPaintedBlocks++;

             acum = Tuple.Create(BlocksInTotal, PaintedBlocks, UnPaintedBlocks);
             return acum;
         });

        return Statitstics;
       
       
    }
   
    public float ColorPercentage(List<ListOfColorsBlock> listofBlocks, ColorBlock colorChosen)
    {
        var seed = Tuple.Create(0, 0, 0);
        var ColorStatistics = listofBlocks.SelectMany(x => x.colorList).Where(x => x.GetType() == colorChosen.GetType())
            .Aggregate(seed, (acum, current) =>
            {
                int ColorBlocksIntotal = acum.Item1 + 1;
                int PaintedBlocks = acum.Item2;
                int UnPaintedBlocks = acum.Item3;
                if (current.isPainted)
                {
                    PaintedBlocks++;
                }
                else UnPaintedBlocks++;

                acum = Tuple.Create(ColorBlocksIntotal, PaintedBlocks, UnPaintedBlocks);

                return acum;
            });

        float percentageColorDone = ((float)ColorStatistics.Item2 / (float)ColorStatistics.Item1);
        //AGREGARLO A LA UIIIIIIIIIIIIIIII
        return percentageColorDone;
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

          var block = listofblocks.SelectMany(x => x.colorList)
                .Where(x => x.GetType() == lvS.levelStuff.Palette[idToPaint].GetType() && !x.isPainted)
                .FirstOrDefault();
            if (!block) return;
            PlayerPaletteSelector.instance.PaintBlock(block);


           
        }
        else
        {

            var col = listofblocks.SelectMany(x => x.colorList)
                .Where(x => x.GetType() == lvS.levelStuff.Palette[idToPaint].GetType() && !x.isPainted)
                .Take(BlocksamountToPaint);
            foreach (var Block in col)
            {
                if (!Block) return;
                PlayerPaletteSelector.instance.PaintBlock(Block);
            }
        }
        

    }

    public List<int> ReturnCheckPointsofPercentage(ColorBlock colorblock, List<ListOfColorsBlock> listofblocks, int divideInSuchAmount)
    {
        int seed = 0;
        var array = new int[divideInSuchAmount];
        var BlocksOfThistypeAmount = listofblocks.SelectMany(x => x.colorList).Where(x => x.GetType() == colorblock.GetType()).Count();
        /*var finalList = array.Aggregate(seed, (acum, current) =>
         {
             current 

             return acum;
         });
        */
       return array.ToList();

    }
}
