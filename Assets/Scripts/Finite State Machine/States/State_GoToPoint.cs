using UnityEngine;

namespace IA.FSM.States
{
    [CreateAssetMenu(fileName = "StateGoToPoint", menuName = "States/State - Go To Point")]
    public class State_GoToPoint : State
    {
        public Vector3 point;
        public float speed;

        internal override void DoMainBehaviour()
        {
            Move();
        }
        void Move()
        {
            Vector3 pos = fsmData.pAgent.position;
            Vector3 dir = (point - pos).normalized;
            float currentDistance = Vector3.Distance(pos, point);
            float newDistance = Vector3.Distance(pos + dir * speed, point);

            //If current position is closer than next position, don't move
            if (currentDistance < newDistance) return;

            fsmData.pAgent.position += dir * speed * Time.deltaTime;
        }
    }
}