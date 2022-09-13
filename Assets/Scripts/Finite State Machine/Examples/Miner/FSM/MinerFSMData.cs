using System;
using UnityEngine;

namespace IA.Examples
{
    [Serializable]
    public class MinerFSMData : FSM.FSMData
    {
        [SerializeField] Transform gold;
        [SerializeField] Transform deposit;
        [SerializeField] float moveSpeed;
        [SerializeField] float mineSpeed;

        public Transform pGold { get { return gold; } }
        public Transform pDeposit { get { return deposit; } }
        public float pMoveSpeed { get { return moveSpeed; } }
        public float pMineSpeed { get { return mineSpeed; } }
    }
}
