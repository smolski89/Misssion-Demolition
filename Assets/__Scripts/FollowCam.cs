using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // Statyczne pole reprezentujące punkt docelowy
    [Header("Definiowane w panelu inspekcyjnym")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;
    [Header("Definowane dynamicznie")]
    public float camZ; //Wymagana wartość Z położenia kamery
    void Awake()
    {
        camZ = this.transform.position.z;
    }
    void FixedUpdate()
    {
        //if (POI == null) return; // Wracamy jeśli nie ma punktu docelowego
        // Pobierz położenie punktu docelowego
        //Vector3 destination = POI.transform.position;
        Vector3 destination;
        //Jeśli nie ma POI zwróc P[0,0,0]
        if(POI==null)
        {
            destination = Vector3.zero;
        }
        else
        {
            //pobierz położenie POI
            destination = POI.transform.position;
            //Jesli POI jest pociskiem, sprawdz czy jest nieruchomy
            if(POI.tag == "Projectile")
            {
                //Jesli jest w stanie uspienia
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    //Wróć do domyslnego widoku
                    POI = null;
                    // w nastepnym wywołaniufunkcji Update()
                    return;
                }
            }
        }
        //Ogranicz położenie na osi X i Y do wartości minimalnych
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //Interpoluj od bieżącego położenia kamery do punktu docelowego
        destination = Vector3.Lerp(transform.position, destination, easing);
        //Wymuś wartość destination.z równą camZ, aby kamera znajdowana się odpowiednio daleko
        destination.z = camZ;
        //ustaw położenie kamery równe wartości destination
        transform.position = destination;
        //Ustaw wartość orthographicSize kamery w taki sposob, aby podłoże było widoczne w oknie widoku
        Camera.main.orthographicSize = destination.y + 10;
    }
}
