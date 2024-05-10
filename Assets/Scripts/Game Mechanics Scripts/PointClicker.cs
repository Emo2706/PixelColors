using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointClicker : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            ClickBlock(PlayerPaletteSelector.instance.CurrentidColor);
        }
    }

    void ClickBlock(int ID)
    {
        Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit;
        if (Physics2D.Raycast(clickPosition, Vector2.zero).collider != null)
        {
            hit = Physics2D.Raycast(clickPosition, Vector2.zero);
            if (hit.collider.TryGetComponent<ColorBlock>(out ColorBlock block))
            {
                if (block.ID == ID)
                {
                    block.Paint();

                }
            }
            Debug.Log(hit.collider.gameObject.name);
        }
        

        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector2 ray2d = new Vector2(ray.origin.x, ray.origin.y);
        RaycastHit2D hit2d;
       

        if (Physics2D.Raycast(ray2d, out hit2d, 100))
        {
            if (hit2d.collider.TryGetComponent<ColorBlock>(out ColorBlock block))
            {
                block.spr.color = Color.black;
            }
        }*/
    }
}
