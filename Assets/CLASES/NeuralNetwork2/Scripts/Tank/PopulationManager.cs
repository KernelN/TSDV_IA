using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IA.NeuralNetworks.Tanks;

public class PopulationManager : MonoBehaviour
{
    [SerializeField] Transform MineEmpty;
    [SerializeField] GameObject MinePrefab;

    public int PopulationCount = 40;
    public int MinesCount = 50;
    
    public PopulationSettings settings;
    
    public Vector3 SceneHalfExtents = new Vector3 (20.0f, 0.0f, 20.0f);

    public float GenerationDuration = 20.0f;
    public int IterationCount = 1;

    public int EliteCount = 4;
    public float MutationChance = 0.10f;
    public float MutationRate = 0.01f;

    public int InputsCount = 4;
    public int HiddenLayers = 1;
    public int OutputsCount = 2;
    public int NeuronsCountPerHL = 7;
    public float Bias = 1f;
    public float P = 0.5f;


    GeneticAlgorithm genAlg;

    public bool isGoodTeamWinning;
    [SerializeField] TeamPopulationManager goodTeam;
    [SerializeField] TeamPopulationManager badTeam;
    
    List<Tank> populationGOs = new List<Tank>();
    List<Genome> population = new List<Genome>();
    List<NeuralNetwork> brains = new List<NeuralNetwork>();
    List<GameObject> mines = new List<GameObject>();
    List<GameObject> goodMines = new List<GameObject>();
    List<GameObject> badMines = new List<GameObject>();
     
    float accumTime = 0;
    bool isRunning = false;

    public int loops {
        get; private set;
    }
    
    public int goodGens => goodTeam.generation;
    public int badGens => badTeam.generation;

    public float bestFitness 
    {
        get; private set;
    }

    public float avgFitness 
    {
        get; private set;
    }

    public float worstFitness 
    {
        get; private set;
    }

    static PopulationManager instance = null;

    public static PopulationManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PopulationManager>();

            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        settings = new PopulationSettings();
    }

    public void StartSimulation()
    {
        settings.Bias = Bias;
        settings.P = P;
        settings.EliteCount = EliteCount;
        settings.GenerationDuration = GenerationDuration;
        settings.HiddenLayers = HiddenLayers;
        settings.InputsCount = InputsCount;
        settings.IterationCount = IterationCount;
        settings.MutationChance = MutationChance;
        settings.MutationRate = MutationRate;
        settings.OutputsCount = OutputsCount;
        settings.SceneHalfExtents = SceneHalfExtents;
        settings.NeuronsCountPerHL = NeuronsCountPerHL;
        
        goodTeam.settings = settings;
        badTeam.settings = settings;
        
        goodTeam.StartSimulation();
        badTeam.StartSimulation();
        
        loops = goodTeam.generation;

        CreateMines();

        isRunning = true;
    }

    public void PauseSimulation()
    {
        goodTeam.PauseSimulation();
        badTeam.PauseSimulation();
        
        isRunning = !isRunning;
    }

    public void StopSimulation()
    {
        goodTeam.StopSimulation();
        badTeam.StopSimulation();

        // Destroy all mines
        DestroyMines();

        isRunning = false;
    }

    // Update is called once per frame
    void FixedUpdate () 
	{
        if (!isRunning)
            return;
        
        float dt = Time.fixedDeltaTime;

        for (int i = 0; i < Mathf.Clamp((float)(IterationCount / 100.0f) * 50, 1, 50); i++)
        {
            for (int j = 0; j < goodTeam.populationGOs.Count; j++)
            {
                // Get the nearest mine
                GameObject mineG = GetNearestMine(goodTeam.populationGOs[i].transform.position);
                GameObject mineB = GetNearestMine(badTeam.populationGOs[i].transform.position);

                // Set the nearest mine to current tank
                goodTeam.populationGOs[i].SetNearestMine(mineG);
                badTeam.populationGOs[i].SetNearestMine(mineB);

                mineG = GetNearestGoodMine(goodTeam.populationGOs[i].transform.position);
                mineB = GetNearestGoodMine(badTeam.populationGOs[i].transform.position);

                // Set the nearest mine to current tank
                goodTeam.populationGOs[i].SetGoodNearestMine(mineG);
                badTeam.populationGOs[i].SetGoodNearestMine(mineB);

                mineG = GetNearestBadMine(goodTeam.populationGOs[i].transform.position);
                mineB = GetNearestBadMine(badTeam.populationGOs[i].transform.position);

                // Set the nearest mine to current tank
                goodTeam.populationGOs[i].SetBadNearestMine(mineG);
                badTeam.populationGOs[i].SetBadNearestMine(mineB);

                // Think!! 
                goodTeam.populationGOs[i].Think(dt);
                badTeam.populationGOs[i].Think(dt);

                
                ///////////
                // Just adjust good tank position when reaching world extents
                Vector3 pos = goodTeam.populationGOs[i].transform.position;
                if (pos.x > SceneHalfExtents.x)
                    pos.x -= SceneHalfExtents.x * 2;
                else if (pos.x < -SceneHalfExtents.x)
                    pos.x += SceneHalfExtents.x * 2;

                if (pos.z > SceneHalfExtents.z)
                    pos.z -= SceneHalfExtents.z * 2;
                else if (pos.z < -SceneHalfExtents.z)
                    pos.z += SceneHalfExtents.z * 2;

                // Set good tank position
                goodTeam.populationGOs[i].transform.position = pos;
                
                ///////////
                // Just adjust bad tank position when reaching world extents
                pos = badTeam.populationGOs[i].transform.position;
                if (pos.x > SceneHalfExtents.x)
                    pos.x -= SceneHalfExtents.x * 2;
                else if (pos.x < -SceneHalfExtents.x)
                    pos.x += SceneHalfExtents.x * 2;

                if (pos.z > SceneHalfExtents.z)
                    pos.z -= SceneHalfExtents.z * 2;
                else if (pos.z < -SceneHalfExtents.z)
                    pos.z += SceneHalfExtents.z * 2;

                // Set bad tank position
                badTeam.populationGOs[i].transform.position = pos;
            }

            // Check the time to evolve
            accumTime += dt;
            if (accumTime >= GenerationDuration)
            {
                accumTime = 0;
                
                //Check winning team
                float goodTeamFit = goodTeam.getAvgFitness();
                float badTeamFit = badTeam.getAvgFitness();
                isGoodTeamWinning = goodTeamFit > badTeamFit;

                if (isGoodTeamWinning)
                {
                    //Reset Good Team fitness (without evolving)
                    goodTeam.RestartGen();
                    
                    //Evolve bad team (because good evolved enough to win)
                    badTeam.Epoch();
                    
                    bestFitness = goodTeam.bestFitness;
                    avgFitness = goodTeamFit;
                    worstFitness = goodTeam.worstFitness;
                }
                else
                {
                    //Reset Bad Team fitness (without evolving)
                    badTeam.RestartGen();
                    
                    //Evolve good team (because bad evolved enough to win)
                    goodTeam.Epoch();
                    
                    //Set watch values
                    bestFitness = badTeam.bestFitness;
                    avgFitness = badTeamFit;
                    worstFitness = badTeam.worstFitness;
                }
                
                loops++;
                
                break;
            }
        }
	}

#region Helpers
    void DestroyMines()
    {
        foreach (GameObject go in mines)
            Destroy(go);

        mines.Clear();
        goodMines.Clear();
        badMines.Clear();
    }

    void CreateMines()
    {
        // Destroy previous created mines
        DestroyMines();

        for (int i = 0; i < MinesCount; i++)
        {
            Vector3 position = GetRandomPos();
            GameObject go = Instantiate<GameObject>(MinePrefab, position, Quaternion.identity, MineEmpty);

            bool good = Random.Range(-1.0f, 1.0f) >= 0;

            SetMineGood(good, go);

            mines.Add(go);
        }
    }

    void SetMineGood(bool good, GameObject go)
    {
        MineData mine = go.GetComponent<MineData>();

        mine.isGood = good;
        
        if (good)
        {
            go.GetComponent<Renderer>().material.color = Color.green;
            goodMines.Add(go);
        }
        else
        {
            go.GetComponent<Renderer>().material.color = Color.red;
            badMines.Add(go);
        }

    }

    public void RelocateMine(GameObject mine)
    {
        if (goodMines.Contains(mine))
            goodMines.Remove(mine);
        else
            badMines.Remove(mine);

        bool good = Random.Range(-1.0f, 1.0f) >= 0;

        SetMineGood(good, mine);

        mine.transform.position = GetRandomPos();
    }

    Vector3 GetRandomPos()
    {
        return new Vector3(Random.value * SceneHalfExtents.x * 2.0f - SceneHalfExtents.x, 0.0f, Random.value * SceneHalfExtents.z * 2.0f - SceneHalfExtents.z); 
    }

    GameObject GetNearestMine(Vector3 pos)
    {
        GameObject nearest = mines[0];
        float distance = (pos - nearest.transform.position).sqrMagnitude;

        foreach (GameObject go in mines)
        {
            float newDist = (go.transform.position - pos).sqrMagnitude;
            if (newDist < distance)
            {
                nearest = go;
                distance = newDist;
            }
        }

        return nearest;
    }   

    GameObject GetNearestGoodMine(Vector3 pos)
    {
        GameObject nearest = mines[0];
        float distance = (pos - nearest.transform.position).sqrMagnitude;

        foreach (GameObject go in goodMines)
        {
            float newDist = (go.transform.position - pos).sqrMagnitude;
            if (newDist < distance)
            {
                nearest = go;
                distance = newDist;
            }
        }

        return nearest;
    }   

    GameObject GetNearestBadMine(Vector3 pos)
    {
        GameObject nearest = mines[0];
        float distance = (pos - nearest.transform.position).sqrMagnitude;

        foreach (GameObject go in badMines)
        {
            float newDist = (go.transform.position - pos).sqrMagnitude;
            if (newDist < distance)
            {
                nearest = go;
                distance = newDist;
            }
        }

        return nearest;
    }   

#endregion

}
