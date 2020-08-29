using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public Main main;
    public Sprite Winner;
   private void OnTriggerEnter2D(Collider2D collision)
   {
       if (collision.gameObject.tag == "Player")
       {
        GetComponent<SpriteRenderer>().sprite = Winner;
        main.Win();
       }
   }
}
