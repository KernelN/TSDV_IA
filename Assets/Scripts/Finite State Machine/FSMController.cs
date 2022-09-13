using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace IA.FSM
{
    [System.Serializable]
    public abstract class FSMController
    {
        [Header("Set Values")]
        [SerializeField] internal List<States.State> states;
        //internal List<Relation> relations;
        //internal FSMData data;
        [Header("Runtime Values")]
        [SerializeField] internal int currentStateIndex;
        //internal Relation currentRelation;

        //Methods
        public void SetFSM()
        {
            SetStates();
            SetRelations();
            UpdateRelation();
        }
        public void Update()
        {
            states[currentStateIndex].Update();
        }
        internal abstract void UpdateRelation();
        internal abstract void SetState(States.State state);
        internal abstract void SetRelations();
        void SetStates()
        {
            for (int i = 0; i < states.Count; i++)
            {
                SetState(states[i]);
                states[i].SetIndex(i);
            }
        }
    }
}