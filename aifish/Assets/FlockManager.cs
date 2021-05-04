using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    //Dispersão dos peixes 
    public GameObject fishPrefab; //Prefab cardume
    public int numFish = 20; //Número de peixes presentes no cardume
    public GameObject[] allFish; //Peixes no total
    public Vector3 swinLimits = new Vector3(5, 5, 5); //Limitação na movimentação
    public Vector3 goalPos;

    //Velocidade e distância do cardume na rota 
    [Header("Configuraçao do Cardume")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;

    void Start()
    {
        allFish = new GameObject[numFish]; //Total de peixes com tamanho e número
        for (int i = 0; i < numFish; i++) //Loop
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), //Muda movimentação e posição 
                Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity); //Pega os peixes no array 
            allFish[i].GetComponent<Flock>().myManager = this;
        }
        goalPos = this.transform.position;
    }

    void Update()
    {
        goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z, swinLimits.z));
    }
}
