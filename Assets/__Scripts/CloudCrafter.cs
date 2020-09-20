using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Definiowane w panelu inspekcyjnym")]
    public int numClouds = 40; // liczba chmur do wygenerowania
    public GameObject cloudPrefarb; //Prefabrykat chmury
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1; // minimalna wartość skalowania dla kazdej chmury
    public float cloudScaleMax = 3; //Maksymalna wartość skalowania dla kazdej chmury
    public float cloudSpeedMult = 0.5f; //Modyfikuje predkość chmur
    private GameObject[] cloudInstances;
    void Awake()
    {
        //Tablica powinna byc tajk duza,aby mogla przechowywac wszystkie instancje chmur
        cloudInstances = new GameObject[numClouds];
        //odszukaj rodzica obiektu cloudanchor
        GameObject anchor = GameObject.Find("CloudAnchor");
        //chmury wygeneruj w pętli
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            //stworz instancje obiektu cloudprefarb
            cloud = Instantiate<GameObject>(cloudPrefarb);
            //Ustal położenie chmury
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //zdefimiuj rozmiar chmury
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //Mniejsze chmury powinny znajdowac sie bliżej powierzchmi ziemi
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            //Mniejsze chmury powinny znajdowac się dalej
            cPos.z = 100 - 90 * scaleU;
            //Zastosuj transformacje dla chmury
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //Uczyń chmure potomkiem obiektu anchor
            cloud.transform.SetParent(anchor.transform);
            //Dodaj chmure do tablicy
            cloudInstances[i] = cloud;
        }
    }
   

    // Update is called once per frame
    void Update()
    {
        //przetwórz każdą chmure, która została utworzona
        foreach (GameObject cloud in cloudInstances)
        {
            //odczytaj wartość skalowania i położenia dla chmury
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //Niech wieksze chmury poruszają się szybciej
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            //jesli chmura poruszała się zbyt szybko w lewym kierunku
            if(cPos.x <= cloudPosMin.x)
            {
                //przemieśją maksymalnie w prawo
                cPos.x = cloudPosMax.x;
            }
            //zastosuj nowe połozenie dla chmury
            cloud.transform.position = cPos;
        }
    }
}
