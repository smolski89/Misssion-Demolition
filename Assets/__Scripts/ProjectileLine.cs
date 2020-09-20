using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; //Singleton
    [Header("Definiowane w panelu inspkcyjnym")]
    public float minDist = 0.1f;
    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;
    void Awake()
    {
        S = this; //Zdefiniuj singleton
        //uzyskaj referencje do komponentu LineRenderer
        line = GetComponent<LineRenderer>();
        //Miej wyłączony komponent LineRenderer aż do momentu, gdy będzie potrzebny
        line.enabled = false;
        //Zainicjalizuj listę punktów
        points = new List<Vector3>();
    }
    //To jest właściwość (czyli metoda przedstawiająca się jako pole)
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set {
            _poi = value;
            if(_poi != null)
                {
                //Gdy przypiszeny coś do _poi, musimy wszystko zresetować
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    //Użyj tego do bezpośredniego zlikwidowania śladu
    public void CLear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }
    public void AddPoint()
    {
        //Dodajemy punkt do listy
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            //Wróć jeśli nowy punkt nie jest połozony odpowiednio daleko od poprzedniego punktu
            return;
        }
        if (points.Count == 0)
        { // Jeśli jest to miejsce oddawania strzału
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;//Do zdefiniowania 
            //... należy póżniej dodać kod obsługujący celowanie
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            //Ustaw dwa pierwsze punkty
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            //Włącz LineRenderer
            line.enabled = true;
        }
        else
        {
            //Normalny sposób dodawania punktu
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }
    public Vector3 lastPoint
    {
        get {
            if (points == null)
            {
                //Jesli nie ma punktów, zwróć Vecror3.zero
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }
    private void FixedUpdate()
    {
        if (poi == null)
        {
            //Jesli poi nie został zdefiniowany, poszukaj go
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return;//Wróć jeśli nie znaleźlismy poi
                }
            }
            else
            {
                return; // wróć jeśli nie znaleźliśmy poi
            }
        }

        //Jeśli poi nie istnieje, jego położenie jest dodawane w każdym wywołaniu funkcji FixedUpdate
        AddPoint();
        if (FollowCam.POI == null) 
        {
            //Gdy tylko wartość FollowCam.POI jest równa null, przypisz null także do lokalnego poi
            poi = null;
        }
    }
}
