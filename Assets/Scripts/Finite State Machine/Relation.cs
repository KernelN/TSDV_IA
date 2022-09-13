using IA.FSM.States;
using System.Collections.Generic;

namespace IA.FSM
{
    [System.Serializable]
    public abstract class Relation
    {
        public State source;

        /// <summary> 
        /// Replaces the local State instantiations by the ones sent by the FSM
        /// </summary>
        /// <param name="source"> only source state of the relation </param>
        /// <param name="states"> all states loaded in FSM</param>
        public virtual void SetTriggers(State source, List<State> states)
        {
            this.source = source;
        }
    }
}