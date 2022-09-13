using IA.Examples;
using System.Collections.Generic;
using UnityEngine;

namespace IA.FSM
{
    public class TestFSM : MonoBehaviour
    {
        [Header("Set Values")]
        public List<States.State> states;
        [SerializeField] List<MinerRelation> relations;
        [SerializeField] MinerFSMData data;
        [Header("Runtime Values")]
        [SerializeField] MinerRelation currentRelation;
        [SerializeField] int index;

        //Unity Events
        private void Start()
        {
            foreach (var state in states)
            {
                state.SetFSMData(data);
            }
        }
        private void Update()
        {
            states[index].Update();
        }

        //Methods
    }
}
