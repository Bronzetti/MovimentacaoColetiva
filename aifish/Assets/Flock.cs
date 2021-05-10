using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{ 

public FlockManager myManager; //Pega o script FlockManager que é meu gerenciador
 //Velocidade do cardume
public float speed;
bool turning = false;

void Start()
{
        //Controle de velocidades
    speed = Random.Range(myManager.minSpeed,
        myManager.maxSpeed);
}

void Update()
{
    Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2); //Limitação de espaço, sendo Bound, um limitador

        RaycastHit hit = new RaycastHit(); //Raycast para pegar o local que deve ser evitada a colisão pelos peixes
        Vector3 direction = myManager.transform.position - transform.position; //

    if (!b.Contains(transform.position)) //O peixe pode chegar até um determinado ponto mas ele precisa voltar para o cardume 
    {
        turning = true; //Construção do uso do Reflect usando Raycast
            direction = myManager.transform.position - transform.position;
    }
    else if(Physics.Raycast(transform.position, this.transform.forward * 50, out hit)) //Hit atribuído ao pilar fazendo com que o peixe evite de atravessar o objeto
        {
            turning = true; 
            direction = Vector3.Reflect(this.transform.forward, hit.normal); //Direção com Reflect
        }    
    else
    {
        turning = false;
    }
    if (turning)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
                             Quaternion.LookRotation(direction),
                             myManager.rotationSpeed * Time.deltaTime);
    }
    else //Caso seja falso
    {
        if (Random.Range(0, 100) < 10) //Se o peixe estiver num cardume menor o peixe tem sua velocidade por aproximação
            speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
        if (Random.Range(0, 100) < 20) //Caso esteja acima dos 20, é aplicado o ApplyRules
            ApplyRules();
    }
    transform.Translate(0, 0, Time.deltaTime * speed); //Aplicação da velocidade

}
void ApplyRules() //Aplica regras de movimentação/aplicação
{   //Array
    GameObject[] gos; 
    gos = myManager.allFish;

    Vector3 vcenter = Vector3.zero; //Ponto base inicial
    Vector3 vavoid = Vector3.zero; //Ponto base inicial
    float gSpeed = 0.01f; //Velocidade pro cardume
    float nDistance; //Distâcia atribuida aos peixes vizinhos
    int groupSize = 0; //Grupo tamanho
    
    foreach (GameObject go in gos) //Loop multiplicando os peixes 
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
                Quaternion.LookRotation(direction),
                myManager.rotationSpeed * Time.deltaTime);

        }
}
}
