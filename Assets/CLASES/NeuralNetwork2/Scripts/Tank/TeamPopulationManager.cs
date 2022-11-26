using System.Collections.Generic;
using UnityEngine;

namespace IA.NeuralNetworks.Tanks
{
    public class TeamPopulationManager : MonoBehaviour
    {
        public bool isGood;
        
        [SerializeField] Transform TankEmpty;
        [SerializeField] GameObject TankPrefab;

        public PopulationSettings settings { private get; set; }

        GeneticAlgorithm genAlg;

        public List<CustomTank> populationGOs = new List<CustomTank>();
        List<Genome> population = new List<Genome>();
        List<NeuralNetwork> brains = new List<NeuralNetwork>();

        float accumTime = 0;
        bool isRunning = false;

        public int generation;// { get; private set; }

        public float bestFitness;// { get; private set; }

        public float avgFitness;// { get; private set; }

        public float worstFitness;// { get; private set; }

        public float getBestFitness()
        {
            float fitness = 0;
            foreach (Genome g in population)
            {
                if (fitness < g.fitness)
                    fitness = g.fitness;
            }

            return fitness;
        }

        public float getAvgFitness()
        {
            float fitness = 0;
            foreach (Genome g in population)
            {
                fitness += g.fitness;
            }
            
            return fitness / population.Count;
        }

        public float getWorstFitness()
        {
            float fitness = float.MaxValue;
            foreach (Genome g in population)
            {
                if (fitness > g.fitness)
                    fitness = g.fitness;
            }
            
            return fitness;
        }

        public void StartSimulation()
        {
            // Create and confiugre the Genetic Algorithm
            genAlg = new GeneticAlgorithm(settings.EliteCount, settings.MutationChance, settings.MutationRate);

            GenerateInitialPopulation();

            isRunning = true;
        }

        public void PauseSimulation()
        {
            isRunning = !isRunning;
        }

        public void StopSimulation()
        {
            isRunning = false;

            generation = 0;

            // Destroy previous tanks (if there are any)
            DestroyTanks();
        }

        // Generate the random initial population
        public void GenerateInitialPopulation()
        {
            generation = 0;

            // Destroy previous tanks (if there are any)
            DestroyTanks();

            for (int i = 0; i < settings.PopulationCount; i++)
            {
                NeuralNetwork brain = CreateBrain();

                Genome genome = new Genome(brain.GetTotalWeightsCount());

                brain.SetWeights(genome.genome);
                brains.Add(brain);

                population.Add(genome);
                populationGOs.Add(CreateTank(genome, brain));
            }

            accumTime = 0.0f;
        }

        // Creates a new NeuralNetwork
        NeuralNetwork CreateBrain()
        {
            NeuralNetwork brain = new NeuralNetwork();

            // Add first neuron layer that has as many neurons as inputs
            brain.AddFirstNeuronLayer(settings.InputsCount, settings.Bias, settings.P);

            for (int i = 0; i < settings.HiddenLayers; i++)
            {
                // Add each hidden layer with custom neurons count
                brain.AddNeuronLayer(settings.NeuronsCountPerHL, settings.Bias, settings.P);
            }

            // Add the output layer with as many neurons as outputs
            brain.AddNeuronLayer(settings.OutputsCount, settings.Bias, settings.P);

            return brain;
        }

        // Evolve!!!
        public void Epoch()
        {
            // Increment generation counter
            generation++;

            // Calculate best, average and worst fitness
            bestFitness = getBestFitness();
            avgFitness = getAvgFitness();
            worstFitness = getWorstFitness();

            // Evolve each genome and create a new array of genomes
            Genome[] newGenomes = genAlg.Epoch(population.ToArray());

            // Clear current population
            population.Clear();

            // Add new population
            population.AddRange(newGenomes);

            // Set the new genomes as each NeuralNetwork weights
            for (int i = 0; i < settings.PopulationCount; i++)
            {
                NeuralNetwork brain = brains[i];

                brain.SetWeights(newGenomes[i].genome);

                populationGOs[i].SetBrain(newGenomes[i], brain);
                populationGOs[i].transform.position = GetRandomPos();
                populationGOs[i].transform.rotation = GetRandomRot();
            }
        }

        //Restart generation's fitness without evolving
        public void RestartGen()
        {
            // Calculate best, average and worst fitness
            bestFitness = getBestFitness();
            avgFitness = getAvgFitness();
            worstFitness = getWorstFitness();

            // Set the new genomes as each NeuralNetwork weights
            for (int i = 0; i < settings.PopulationCount; i++)
            {
                populationGOs[i].Reset();
                populationGOs[i].transform.position = GetRandomPos();
                populationGOs[i].transform.rotation = GetRandomRot();
            }
        }

        #region Helpers

        CustomTank CreateTank(Genome genome, NeuralNetwork brain)
        {
            Vector3 position = GetRandomPos();
            GameObject go = Instantiate<GameObject>(TankPrefab, position, GetRandomRot(), TankEmpty);
            CustomTank t = go.GetComponent<CustomTank>();
            t.isGood = isGood;
            t.SetBrain(genome, brain);
            return t;
        }

        void DestroyTanks()
        {
            foreach (CustomTank go in populationGOs)
                Destroy(go.gameObject);

            populationGOs.Clear();
            population.Clear();
            brains.Clear();
        }

        Vector3 GetRandomPos()
        {
            float x = Random.value * settings.SceneHalfExtents.x * 2.0f - settings.SceneHalfExtents.x;
            float z = Random.value * settings.SceneHalfExtents.z * 2.0f - settings.SceneHalfExtents.z;
            
            return new Vector3(x, 0.0f, z);
        }

        Quaternion GetRandomRot()
        {
            return Quaternion.AngleAxis(Random.value * 360.0f, Vector3.up);
        }

        #endregion
    }
}