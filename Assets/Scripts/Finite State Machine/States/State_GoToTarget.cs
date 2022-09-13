using UnityEngine;

namespace IA.FSM.States
{
    public abstract class State_GoToTarget : State
    {
        internal float speed;
        internal Transform target;

        internal override void DoMainBehaviour()
        {
            if (!ValuesSetted())
            {
                SetValues();
            }

            Move();
        }
        internal abstract void SetValues();
        internal abstract bool ValuesSetted();
        void Move()
        {
            //If there is no target, stop
            if (target == null) return;

            Vector3 pos = fsmData.pAgent.position;
            Vector3 targetPos = target.position;
            Vector3 dir = (target.position - pos).normalized;
            float currentDistance = Vector3.Distance(pos, targetPos);
            float newDistance = Vector3.Distance(pos + dir * speed, targetPos);

            //If current position is closer than next position, don't move
            if (currentDistance < newDistance)
            {
                CallFlag();
                return;
            }

            fsmData.pAgent.position += dir * speed * Time.deltaTime;
        }
    }
}
