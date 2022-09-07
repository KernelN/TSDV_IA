using System;
using System.Collections.Generic;
using UnityEngine;

namespace IA.FSM.States
{
    public class State : ScriptableObject
    {
        internal FSMData fsmData;
        internal int index = -1;
        internal List<Action> behaviours;

        public int pIndex { get { return index; } }

        //Setters
        public void SetFSMData(FSMData _fsmData)
        {
            fsmData = _fsmData;
        }
        public void SetIndex(int _index)
        {
            index = _index;
        }
        public void SetBehaviours(Action behaviour)
        {
            behaviours = new List<Action>();
            behaviours.Add(behaviour);
        }
        public void SetBehaviours(List<Action> _behaviours)
        {
            behaviours = _behaviours;
        }
        public void AddBehaviours(Action behaviour)
        {
            behaviours.Add(behaviour);
        }
        public void AddBehaviours(List<Action> _behaviours)
        {
            behaviours.AddRange(_behaviours);
        }

        //Main Behaviour
        public void Update()
        {
            DoMainBehaviour();

            //If there are behaviours, invoke them
            if (behaviours == null) return;
            foreach (var behaviour in behaviours)
            {
                behaviour?.Invoke();
            }
        }
        internal virtual void DoMainBehaviour() {}
    }
}