using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public static HUD instance;
    public TMP_Text blocksToPaint;
    [SerializeField] float _appearsEveryInS;
    [SerializeField] float _dissappearsafterInS;
    [SerializeField] TMP_Text statisticsText;
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
        StartCoroutine(StatisticsAppear(_appearsEveryInS, _dissappearsafterInS, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StatisticsAppear(float seconds, float secondstoVanish, int counter)
    {
        statisticsText.gameObject.SetActive(false);
        yield return new WaitForSeconds(seconds);
        statisticsText.gameObject.SetActive(true);
        
        if (counter == 0)
        {
            var statistics = GameManager.instance.PixelArtStatistics(GameManager.instance.CurrentAllBlocks);
            statisticsText.text = "Bloques pintados " + statistics.Item2 + "/" + statistics.Item1 + " faltan pintar " + statistics.Item3 + " bloques";
        }
        else
        {
            var statistics = GameManager.instance.HowManyPrimaryColorsBlocksLeft(GameManager.instance.CurrentAllBlocks);
            statisticsText.text = "AMO LOS COLORES PRIMARIOS: Pintaste " + statistics.Item3 + " bloques rojos, " + statistics.Item2 +
                " azules y " + statistics.Item1 + " amarillos";

        }
        counter++;
        if (counter > 1)
        {
            counter = 0;
        }

        yield return new WaitForSeconds(secondstoVanish);

        StartCoroutine(StatisticsAppear(seconds, secondstoVanish, counter));

    }
}
