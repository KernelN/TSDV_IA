using System;
using UnityEngine;
using System.Collections;

namespace IA.NeuralNetworks.Tanks
{
    public class CustomTank : TankBase
    {
        public bool isGood;
        [SerializeField] float fitness = 0;

        public void Reset()
        {
            OnReset();
        }

        protected override void OnReset()
        {
            fitness = 1;
        }

        protected override void OnThink(float dt)
        {
            Vector3 dirToGoodMine = GetDirToMine(goodMine);
            Vector3 dirToBadMine = GetDirToMine(badMine);
            Vector3 dir = this.transform.forward;

            inputs[0] = dirToGoodMine.x;
            inputs[1] = dirToGoodMine.z;
            inputs[2] = dirToBadMine.x;
            inputs[3] = dirToBadMine.z;
            inputs[4] = dir.x;
            inputs[5] = dir.z;
            inputs[6] = isGood ? 1 : -1;

            float[] output = brain.Synapsis(inputs);

            SetForces(output[0], output[1], dt);
        }

        protected override void OnTakeMine(GameObject mine)
        {
            MineData mineData = mine.GetComponent<MineData>();
            
            if (mineData.isGood == isGood)
            {
                fitness *= 2;
                genome.fitness = fitness;
            }
            else
            {
                fitness *= 1.05f;
                genome.fitness = fitness;
            }
        }
    }
}