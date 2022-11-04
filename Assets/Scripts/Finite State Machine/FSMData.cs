using System;
using UnityEngine;

namespace IA.FSM
{
    [Serializable]
    public class FSMData
    {
        [Header("Set Values")]
        public Transform agent;
        //[Header("Runtime Values")]

        public Transform pAgent { get { return agent; } }
    }
}
