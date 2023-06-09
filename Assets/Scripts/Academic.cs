using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine.UI;
using System;
using System.Collections.Specialized;

public class Academic : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 random_destination_1;
    public Vector3 random_destination_2;
    public Vector3 random_destination_final;
    public float startTime;
    public float endTime;
    public int counter = 0;
    public float total_distance_1 = 0.0f;
    public float total_distance_2 = 0.0f;
    public string fileLocation = @".\coord_time.csv";
    public string total_agent;
    public Animator anim;
    private NavMeshAgent agent;
    public Text finalcounter;
    public Text textBox;
    private NavMeshPath path;
    public float distanceThreshold = 4.0f;
    public SimulationManager simManager;
    public bool lifesaver=false;
    public int counteracademic=0;
    public bool isClassEvecuated=false;

    public bool isEvacuated = false;

     public void WriteSimulationData()    
    {
        StreamWriter sw = new StreamWriter(fileLocation, true);
        string data = (startPosition.x * 1000).ToString("F0") + "," + (startPosition.z * 1000).ToString("F0") + "," + (startPosition.y * 1000).ToString("F0") + "," + Math.Round((endTime-startTime)).ToString();
        sw.WriteLine(data);
        sw.Flush();
        sw.Close();       
    }
    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        startPosition = gameObject.transform.position;
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("Agent");
        finalcounter.text = ("Kalan aktor sayisi:" + gameObjects.Length.ToString()+"/"+ gameObjects.Length.ToString());
        total_agent = gameObjects.Length.ToString();
        simManager = GameObject.FindWithTag("SimulationManager").GetComponent<SimulationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position[0] >= 114.0 & transform.position[0] <= 114.5 || transform.position[0] >= 217.0 & transform.position[0] <= 217.5)
        {
            
            if (gameObject.tag == "Agent")
            {
                {
                    transform.gameObject.tag = "final";
                    GameObject[] gameObjects;
                    gameObjects = GameObject.FindGameObjectsWithTag("Agent");
                    finalcounter.text = ("Kalan aktor sayisi:"+gameObjects.Length.ToString()+"/"+total_agent);
                    endTime = Time.time;
                    WriteSimulationData();
                }
            }
        }

        if (agent.velocity!=new Vector3(0,0,0) && transform.position!=random_destination_final)
        {
            anim.SetTrigger("isRunning");
        }

        if (isClassEvecuated && counteracademic == 0)
        {
            counteracademic ++;
           
            random_destination_1 = new Vector3(UnityEngine.Random.Range(220.0f, 228.0f), 5, UnityEngine.Random.Range(112.0f, 125.0f));
            random_destination_2 = new Vector3(UnityEngine.Random.Range(109.5f, 97.0f), 1, UnityEngine.Random.Range(60.0f, 52.0f));
                    
            // Add new variables to store the distances between the agent and each destination
            float distance_to_destination_1 = Vector3.Distance(random_destination_1, transform.position);
            float distance_to_destination_2 = Vector3.Distance(random_destination_2, transform.position);

            // If the agent is closer to destination 1, set the final destination to destination 2
            if (distance_to_destination_1 < distance_to_destination_2)
            {
                random_destination_final = random_destination_1;
                NavMesh.CalculatePath(transform.position, random_destination_1, NavMesh.AllAreas, path);
            }
            else if (distance_to_destination_2 < distance_to_destination_1)
            {
                random_destination_final = random_destination_2;
                NavMesh.CalculatePath(transform.position, random_destination_1, NavMesh.AllAreas, path);
            }
            

            agent.SetDestination(random_destination_final);
            startTime = Time.time;
        }

        if (agent.remainingDistance != 0 && agent.remainingDistance < 3 && isEvacuated)
        {
            anim.SetTrigger("isStatic");
            agent.speed = 0;
        }
        
    }

    void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.tag == "classcollider"){
             int agentObjectCount = 0;

            // Collider'a giren tüm nesnelerin tag'lerini kontrol edin.
            foreach (NavigationManager obj in FindObjectsOfType<NavigationManager>()) {
                if (obj.tag == "Agent" && other.bounds.Contains(obj.transform.position)) {
                    agentObjectCount++;
                }
            }
            if(agentObjectCount==1)
            {
                isClassEvecuated = true;
            }
            // Debug.Log("Collider'ın içinde " + agentObjectCount + " tane Agent tag'ine sahip nesne var.");
            }
        }
  
}
