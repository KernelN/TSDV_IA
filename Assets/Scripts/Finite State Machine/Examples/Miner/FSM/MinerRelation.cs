using IA.FSM;
using IA.FSM.States;
using System.Collections.Generic;
using UnityEngine;

namespace IA.Examples
{
    [System.Serializable]
    public class MinerRelation : Relation
    {
        [System.Serializable]
        public class Trigger
        {
            public MinerFlags flag;
            public State target;
        }

        public List<Trigger> triggers;

        public override void SetTriggers(State source, List<State> states)
        {
            base.SetTriggers(source, states);

            //Replace all triggers target state instantiations by the ones in FSM
            foreach (var trigger in triggers)
            {
                foreach (var state in states)
                {
                    //If relation's target state is not current state, skip
                    if (trigger.target.pIndex != state.pIndex) continue;

                    trigger.target = state;
                }
            }
        }
    }
}