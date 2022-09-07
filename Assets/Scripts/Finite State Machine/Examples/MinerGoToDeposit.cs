using IA.FSM.States;
using UnityEngine;

namespace IA.Examples
{
    [CreateAssetMenu(fileName = "MinerGoToTarget", menuName = "States/Miner/Miner State - Go To Target")]
    public class MinerGoToDeposit : State_GoToTarget
    {
        public enum Targets { deposit, mine }
        [SerializeField] Targets targetType;

        internal override void SetValues()
        {
            MinerFSMData data = (MinerFSMData)fsmData;
            speed = data.pMoveSpeed;

            switch (targetType)
            {
                case Targets.deposit:
                    target = data.pDeposit;
                    break;
                case Targets.mine:
                    target = data.pGold;
                    break;
                default:
                    target = data.pDeposit;
                    break;
            }
        }
    }
}
