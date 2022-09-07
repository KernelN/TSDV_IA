using System.Collections.Generic;
using UnityEngine;

namespace IA.FSM
{
    public class TestFSM : MonoBehaviour
    {
        [Header("Set Values")]
        public List<States.State> states;
        [SerializeField] Examples.MinerFSMData data;
        [SerializeField] int index;
        //[Header("Runtime Values")]

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
