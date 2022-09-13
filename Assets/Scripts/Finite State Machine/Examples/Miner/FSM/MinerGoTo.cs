using IA.FSM;
using IA.FSM.States;
using UnityEngine;

namespace IA.Examples
{
    [CreateAssetMenu(fileName = "MinerGoToTarget", menuName = "States/Miner/Miner State - Go To Target")]
    public class MinerGoTo : State_GoToTarget
    {
        public enum Targets { deposit, mine }
        [SerializeField] Targets targetType;
        internal new MinerFSMData fsmData; //DIRTY, MAKE SOMETHING WITH TEMPLATES (<T>)

        public override void SetFSMData(FSMData _fsmData)
        {
            fsmData = (MinerFSMData)_fsmData;
        }
        internal override void SetValues()
        {
            speed = fsmData.pMoveSpeed;

            switch (targetType)
            {
                case Targets.deposit:
                    target = fsmData.pDeposit;
                    break;
                case Targets.mine:
                    target = fsmData.pGold;
                    break;
                default:
                    target = fsmData.pDeposit;
                    break;
            }
        }
        internal override bool ValuesSetted()
        {
            return speed == fsmData.pMoveSpeed;
        }

        internal override void CallFlag()
        {
            FlagCalled.Invoke((int)MinerFlags.OnReachTarget);
        }
    }
}