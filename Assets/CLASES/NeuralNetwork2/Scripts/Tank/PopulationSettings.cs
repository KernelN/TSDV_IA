using UnityEngine;

namespace IA.NeuralNetworks.Tanks
{
    public class PopulationSettings
    {
        public int PopulationCount = 40;

        public Vector3 SceneHalfExtents = new Vector3(20.0f, 0.0f, 20.0f);

        public float GenerationDuration = 20.0f;
        public int IterationCount = 1;

        public int EliteCount = 4;
        public float MutationChance = 0.10f;
        public float MutationRate = 0.01f;

        public int InputsCount = 7;
        public int HiddenLayers = 1;
        public int OutputsCount = 2;
        public int NeuronsCountPerHL = 7;
        public float Bias = 1f;
        public float P = 0.5f;
    }
}
