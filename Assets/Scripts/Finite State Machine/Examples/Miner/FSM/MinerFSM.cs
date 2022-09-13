using IA.FSM;
using IA.FSM.States;
using System.Collections.Generic;
using UnityEngine;

namespace IA.Examples
{
    [System.Serializable]
    public class MinerFSM : FSMController
    {
        //DIRTY, MAKE SOMETHING WITH TEMPLATES (<T>)
        [Header("Child Values", order = -1)]
        [Header("Set Values")]
        [SerializeField] /*new*/ List<MinerRelation> relations;
        [SerializeField] /*new*/ MinerFSMData data;
        [Header("Runtime Values")]
        [SerializeField] /*new*/ MinerRelation currentRelation;


        //Methods
        internal override void SetState(State state)
        {
            state.SetFSMData(data);
            state.FlagCalled += OnFlagCalled;
        }
        internal override void SetRelations()
        {
            foreach (var relation in relations)
            {
                relation.SetTriggers(states[relation.source.pIndex], states);
            }
        }
        internal override void UpdateRelation()
        {
            //Search for in all relations for current state
            foreach (var relation in relations)
            {
                //If relation's source state is not current state, skip
                if (relation.source.pIndex != states[currentStateIndex].pIndex) continue;

                currentRelation = relation;
                return;
            }
        }

        //Event Receivers
        void OnFlagCalled(int flag)
        {
            foreach (var trigger in currentRelation.triggers)
            {
                //if trigger's flag is not current flag, skip
                if ((int)trigger.flag != flag) continue;

                //else, change state
                currentStateIndex = trigger.target.pIndex;
                UpdateRelation();
            }
        }
    }
}