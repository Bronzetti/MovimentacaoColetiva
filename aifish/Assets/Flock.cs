using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{ 

public FlockManager myManager; //Pega o script FlockManager
 //Velocidade do cardume
public float speed;
bool turning = false;

void Start()
{
        //Speed Random
    speed = Random.Range(myManager.minSpeed,
        myManager.maxSpeed);
}

void Update()
{
    Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2); 
    if (!b.Contains(transform.position))
    {
        turning = true;
    }
    else
    {
        turning = false;
    }

    if (turning)
    {
        Vector3 direction = myManager.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(direction),
            myManager.rotationSpeed * Time.deltaTime);
    }
    else
    {
        if (Random.Range(0, 100) < 10)
            speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
        if (Random.Range(0, 100) < 20)
            ApplyRules();
    }
    transform.Translate(0, 0, Time.deltaTime * speed);

}
void ApplyRules() //Aplica regras de movimentação 
{   //Array
    GameObject[] gos; 
    gos = myManager.allFish;

    Vector3 vcenter = Vector3.zero; //Ponto base inicial
    Vector3 vavoid = Vector3.zero; //Ponto base inicial
    float gSpeed = 0.01f; //Velocidade pro cardume
    float nDistance; //Distâcia atribuida aos peixes vizinhos
    int groupSize = 0; //Grupo tamanho
    
    foreach (GameObject go in gos) //Loop
        {
        if (go != this.gameObject) //Condicional caso o objeto não seja esse
            {
            nDistance = Vector3.Distance(go.transform.position, this.transform.position); //Distância do peixe vizinho é a posição dele até o atual
            if (nDistance <= myManager.neighbourDistance) //Condicional se a distância for menor ou igual
            {
                vcenter += go.transform.position; //Direção aumenta até o centro
                groupSize++; //Tamanho do grupo aumenta 


                if (nDistance < 1.0f) //Distância do peixe vizinho for menor que 1
                {
                    vavoid = vavoid + (this.transform.position - go.transform.position); //Desvia do outro peixe
                }
                Flock anotherFlock = go.GetComponent<Flock>();
                gSpeed = gSpeed + anotherFlock.speed; //Velocidade aumenta pro sucesso do desvio 
            }
        }
    }
    
    if (groupSize > 0) //Grupo maior que zero
    {
        vcenter = vcenter / groupSize + (myManager.goalPos - this.transform.position); //Valor do centro dividido pelo tamanho do grupo
        speed = gSpeed / groupSize;//Velocidade dividida pelo grupo

        Vector3 direction = (vcenter + vavoid) - transform.position; //Calcula direção do centro para partir já para a outra rota
        if (direction != Vector3.zero) //Direção diferente de zero
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);

        }
}
}
