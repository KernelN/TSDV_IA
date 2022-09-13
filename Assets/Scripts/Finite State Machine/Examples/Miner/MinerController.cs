using System.Collections.Generic;
using UnityEngine;

namespace IA.Examples
{
    public class MinerController : MonoBehaviour
    {
        [Header("Set Values")]
        [SerializeField] MinerFSM fsm;
        //[SerializeField] List<FSM.States.State> states;
        //[SerializeField] List<MinerRelation> relations;
        //[SerializeField] MinerFSMData data;
        //[Header("Runtime Values")]
        //[SerializeField] MinerRelation currentRelation;
        //[SerializeField] int index;

        ////Unity Events
        private void Start()
        {
            fsm.SetFSM();
        }
        private void Update()
        {
            fsm.Update();
        }

        ////Methods
        ///// <summary> Get the relation of the current state </summary>
        //void UpdateRelation()
        //{
        //    //Search for in all relations for current state
        //    foreach (var relation in relations)
        //    {
        //        //If relation's source state is not current state, skip
        //        if (relation.source.pIndex != states[index].pIndex) continue;

        //        currentRelation = relation;
        //        return;
        //    }
        //}
    }
}
