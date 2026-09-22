using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class BounceController : MonoBehaviour
{

    [HideInInspector] public UnityEvent onBouncedOffGround = new UnityEvent();

    [SerializeField] private int bounces = 0;
    [SerializeField] GameObject impact;

    const string fileRoute = "savedBounces.json";
    const string folderRoute = "BounceScore";
    const String slashForDirectory = "/";

    public int GetBounces()
    {
        return bounces;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bounces++;
        onBouncedOffGround.Invoke();

        impact.SetActive(true);
        Invoke("ImpactEnd", 0.15f);
    }

    private void ImpactEnd()
    {
        impact.SetActive(false);
    }

    [Serializable]
    public class BounceData
    {
        public int bounces;
    }

    public void SaveBounces()
    {
        CreateDirectory(); //asegurar que la carpeta existe

        //instanciar la clase y asignar el valor de bounces
        BounceData scoreData = new BounceData();
        scoreData.bounces = this.bounces; //los bounces de la clase BounceController

        string json = JsonUtility.ToJson(scoreData); //convertir a json (serializar)

        string path = GetPathToFileRoute();
        File.WriteAllText(path, json); //escribir el json en el archivo savedBounces.json

        Debug.Log("Saved Bounces: " + bounces + " to file: " + path);
    }

    public void LoadBounces()
    {
        //asegurar que la carpeta y el archivo existen
        if (!Directory.Exists(GetPathToFolderRoute()) || !File.Exists(GetPathToFileRoute()))
        {
            Debug.LogWarning("No saved bounces");
            return;//no hacemos nada si no existe el archivo
        }

        //leer el contenido del json
        string json = File.ReadAllText(GetPathToFileRoute());

        //deserializar el json a la clase BounceData
        BounceData scoreData = JsonUtility.FromJson<BounceData>(json);

        //asignar el valor de bounces a la clase BounceController (actualizar)
        this.bounces = scoreData.bounces; 

        Debug.Log("Success, loaded bounces: " + bounces);
    }

    private void CreateDirectory()
    {

        if (!Directory.Exists(GetPathToFolderRoute()))
        {
            Directory.CreateDirectory(GetPathToFolderRoute());
            Debug.Log("Created Directory:" + GetPathToFolderRoute());
        }
    }

    public String GetPathToFolderRoute()
    {
        return Application.dataPath + (slashForDirectory + folderRoute);
    }
    public String GetPathToFileRoute()
    {
        return GetPathToFolderRoute() + (slashForDirectory + fileRoute);
    }


    private void Update()
    {
        //Debug.Log(bounces);
    }
}
