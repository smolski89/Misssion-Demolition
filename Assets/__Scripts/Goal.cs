using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //Pole statyczne dostepne z dowolnego miejasca kodu
    static public bool goalMet = false;
    void OnTriggerEnter(Collider other)
    {
       //Gdy wyzwalacz zostanie trafiony, sprawdź czy był to pocisk
       if(other.gameObject.tag == "Projectile")
        {
            //Jeśli tak, przypisz polu goalMet wartość true
            Goal.goalMet = true;
            //również zmieńwartość kanału alfa dla koloru, by uzyskać mniejszą przezroczystość
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;

        }
    }
}
