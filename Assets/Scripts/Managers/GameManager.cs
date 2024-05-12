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
    // public ListOfColorsBlock CurrentAllBlocks = new ListOfColorsBlock();
    //public List<List<ColorBlock>> CurrentAllBlocks = new List<List<ColorBlock>>();
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetProperButtonsPos(AvailableButtons, _buttonsPositions, CurrentAllBlocks2, OrderButtonsBy.MostColorBlocksPainted, currentLevelSettings);
        }
    }
    void SetAllColorOptions(TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();
        dropdown.ClearOptions();
        dropdown.options.Add(new TMP_Dropdown.OptionData(text: "All blocks", null));
        dropdown.options.Add(new TMP_Dropdown.OptionData(text: "All painted blocks", null));
        dropdown.options.Add(new TMP_Dropdown.OptionData(text: "All unpainted blocks", null));

        for (int i = 0; i < currentLevelSettings.levelStuff.Palette.Count ; i++)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(text: currentLevelSettings.levelStuff.Palette[i].color_Name, null));
        }

        dropdown.RefreshShownValue();
    }

    // Update is called once per frame
   
    IEnumerator CorroutineLateStart()
    {
        yield return new WaitForSeconds(0.03f);
        SetButtonsColors(AllButtons, currentLevelSettings.levelStuff.Palette);
        for (int i = 0; i < AllButtons.Count; i++)
        {
            if (i < currentLevelSettings.levelStuff.Palette.Count)
            {
                AvailableButtons.Add(AllButtons[i]);
            }
            else AllButtons[i].gameObject.SetActive(false);


        }
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
            buttonsP[i].idText.text = list[i].ToString();

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



    /*ACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA PREGUNTALEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
     * 
     * 
     * ACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA PREGUNTALEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
     * 
     * ACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA PREGUNTALEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
     */

    void SetProperButtonsPos(List<ButtonPaletteBehaviour> buttonsP, List<Vector3> positions, List<ListOfColorsBlock> colorsBlock , OrderButtonsBy order, LevelSettings lvs)
    {

        //int ButtonsAmount = buttonsP.Count();
        //El take está pq seguramente dsp hagamos que se pueda filtrar la paleta por colores tono tipo rojo, azul amarillo etc
        //var col = buttonsP.Take(ButtonsAmount).ToList();
        // var colu = buttonsP;

        if (order == OrderButtonsBy.Ids)
        {
            var col = buttonsP.OrderBy(x => x.idButton).ToList();

            RTsTransform(col, positions);
        }
        else if (order == OrderButtonsBy.Alphabetic)
        {
            var col = buttonsP.OrderBy(x => x.belongingColorBlock.color_Name).ToList();
            RTsTransform(col, positions);

        }
        else if (order == OrderButtonsBy.AlphabeticDescending)
        {
            var col = buttonsP.OrderByDescending(x => x.belongingColorBlock.color_Name).ToList();
            RTsTransform(col, positions);

        }
        else if (order == OrderButtonsBy.MostColorBlocksPainted)
        {
            //La tupla fue necesaria para poder enlazar el botón con su respectiva lista de bloques de su mismo color
            var TupleList = new List<Tuple<ButtonPaletteBehaviour, List<ColorBlock>>>();
            for (int i = 0; i < lvs.levelStuff.Palette.Count; i++)
            {
                var myTuple = Tuple.Create(buttonsP[i], colorsBlock[i].colorList);

                TupleList.Add(myTuple);
            }
            //GUARDAR Y COMPARAR CON UNA TUPLA UNA TUPLAA
            //var col = colorsBlock.Where(x => x.colorList.Any(x => x.isPainted));
            var col = TupleList.Where(x => x.Item2.Any(x => !x.isPainted)).OrderBy(x => x.Item2.Where(x => !x.isPainted).Count()).ToList();
            var finalList = new List<ButtonPaletteBehaviour>();
            for (int i = 0; i < col.Count; i++)
            {
                finalList.Add(col[i].Item1);

            }
            RTsTransform(finalList, positions);
            //var botonesAMostrar = buttonsP.Where(x => x.idButton == col.SelectMany(x => x.colorList).Any(x => x.ID));
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

    
    public void SetMethodDictionary(Dictionary<int , Action<List<ListOfColorsBlock>>> showMethod)
    {
        showMethod.Add(0, BlocksInTotal);
        showMethod.Add(1, BlocksPainted);
        showMethod.Add(2, BlocksUnpainted);
        showMethod.Add(3, YellowBlocks);
        showMethod.Add(4, BlueBlocks);
        showMethod.Add(5, RedBlocks);
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
