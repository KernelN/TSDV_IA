using UnityEngine;

namespace NAMESPACENAME
{
    public class BirdCustomAI : BirdBase
    {
        //[Header("Set Values")]
        //[SerializeField]
        //[Header("Runtime Values")]

        //Unity Events

        //Methods
        protected override void OnThink(float dt, BirdBehaviour birdBehaviour, Obstacle obstacle)
        {
            float[] inputs = new float[6];
            
            Vector2 birdPos = birdBehaviour.transform.position;
            
            inputs[0] = obstacle.left - birdPos.x;
            inputs[1] = obstacle.right - birdPos.x;
            inputs[2] = obstacle.top - birdPos.y;
            inputs[3] = obstacle.bottom - birdPos.y;
            inputs[4] = obstacle.top - birdPos.y;
            inputs[5] = obstacle.bottom - birdPos.y;
            
            // inputs[0] = obstacle.holeSize.x;
            // inputs[1] = obstacle.holeSize.y;
            // inputs[2] = obstacle.pos.x - birdPos.x;
            // inputs[3] = obstacle.pos.y - birdPos.y;
            // inputs[4] = obstacle.pos.y - birdPos.y;
            // inputs[5] = obstacle.pos.y - birdPos.y;

            float[] outputs;
            outputs = brain.Synapsis(inputs);
            if (outputs[0] > 0.5f)
            {
                birdBehaviour.Flap();
            }

            //if ((obstacle.rightX - birdPos.x) > 0)
            if (Vector3.Distance(obstacle.transform.position, birdPos) <= 1.0f)
            {
                genome.fitness *= 2;
            }

            genome.fitness += (100.0f - Vector3.Distance(obstacle.transform.position, birdBehaviour.transform.position));

        }

        protected override void OnDead()
        {
        }

        protected override void OnReset()
        {
            genome.fitness = 0.0f;
        }
    }
}
