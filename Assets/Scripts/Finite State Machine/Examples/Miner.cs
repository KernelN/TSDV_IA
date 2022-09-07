using UnityEngine;

namespace IA.Examples
{
    public class Miner : MonoBehaviour
    {
        enum MineStates
        {
            Mining,
            GoToMine,
            GoToDeposit,
            Idle,

            _Count
        }

        enum MineFlags
        {
            OnFullInventory,
            OnReachMine,
            OnReachDeposit,
            OnEmpyMine,

            _Count
        }

        enum AlarmStates
        {
            WalkToTC,
            RunToTC,
            DefendTC,
            Idle,

            _Count
        }

        enum AlarmFlags
        {
            OnSafe,
            OnEnemyClose,
            OnReachTC,

            _Count
        }

        enum FSMs
        {
            Miner,
            Alarm
        }

        public Transform mine;
        public Transform deposit;
        public Transform townCenter;

        [SerializeField] float speed = 10.0f;
        [SerializeField] float runMod = 2;
        [SerializeField] float miningTime = 5.0f;
        [SerializeField] float currentMiningTime = 0.0f;
        [SerializeField] int mineUses = 10;
        [SerializeField] bool alarmIsOn;
        [SerializeField] bool enemyIsClose;

        private FSM.FSM currentFSM;
        public FSM.FSM defaultFSM;
        public FSM.FSM alarmFSM;

        void Start()
        {
            currentFSM = defaultFSM;
            SetDefaultFSM();
            SetAlarmFSM();
        }

        void Update()
        {
            if(alarmIsOn)
            {
                if (currentFSM != alarmFSM)
                currentFSM = alarmFSM;

                if (enemyIsClose)
                {
                    currentFSM.SetFlag((int)AlarmFlags.OnEnemyClose);
                }
                else
                {
                    currentFSM.SetFlag((int)AlarmFlags.OnSafe);
                }
            }
            else if(currentFSM != defaultFSM)
            {
                currentFSM = defaultFSM;
            }



            currentFSM.Update();
        }

        void SetDefaultFSM()
        {
            defaultFSM = new FSM.FSM((int)MineStates._Count, (int)MineFlags._Count);
            defaultFSM.ForceCurretState((int)MineStates.GoToMine);

            defaultFSM.SetRelation((int)MineStates.GoToMine, (int)MineFlags.OnReachMine, (int)MineStates.Mining);
            defaultFSM.SetRelation((int)MineStates.Mining, (int)MineFlags.OnFullInventory, (int)MineStates.GoToDeposit);
            defaultFSM.SetRelation((int)MineStates.GoToDeposit, (int)MineFlags.OnReachDeposit, (int)MineStates.GoToMine);
            defaultFSM.SetRelation((int)MineStates.GoToDeposit, (int)MineFlags.OnEmpyMine, (int)MineStates.Idle);

            defaultFSM.AddBehaviour((int)MineStates.Idle, () => { Debug.Log("Idle"); });

            defaultFSM.AddBehaviour((int)MineStates.Mining, () =>
            {
                if (currentMiningTime < miningTime)
                {
                    currentMiningTime += Time.deltaTime;
                }
                else
                {
                    currentMiningTime = 0.0f;
                    defaultFSM.SetFlag((int)MineFlags.OnFullInventory);
                    mineUses--;
                }
            });
            defaultFSM.AddBehaviour((int)MineStates.Mining, () => { Debug.Log("Mining"); });

            defaultFSM.AddBehaviour((int)MineStates.GoToMine, () =>
            {
                Vector2 dir = (mine.position - transform.position).normalized;

                if (Vector2.Distance(mine.position, transform.position) > 1.0f)
                {
                    Vector2 movement = dir * 10.0f * Time.deltaTime;
                    transform.position += new Vector3(movement.x, movement.y);
                }
                else
                {
                    defaultFSM.SetFlag((int)MineFlags.OnReachMine);
                }
            });
            defaultFSM.AddBehaviour((int)MineStates.GoToMine, () => { Debug.Log("GoToMine"); });
            defaultFSM.AddExitBehaviour((int)MineStates.GoToMine, () => { Debug.Log("DropMine"); });

            defaultFSM.AddBehaviour((int)MineStates.GoToDeposit, () =>
            {
                Vector2 dir = (deposit.position - transform.position).normalized;

                if (Vector2.Distance(deposit.position, transform.position) > 1.0f)
                {
                    Vector2 movement = dir * 10.0f * Time.deltaTime;
                    transform.position += new Vector3(movement.x, movement.y);
                }
                else
                {
                    if (mineUses <= 0)
                        defaultFSM.SetFlag((int)MineFlags.OnEmpyMine);
                    else
                        defaultFSM.SetFlag((int)MineFlags.OnReachDeposit);
                }
            });
            defaultFSM.AddBehaviour((int)MineStates.GoToDeposit, () => { Debug.Log("GoToDeposit"); });
        }

        void SetAlarmFSM()
        {
            alarmFSM = new FSM.FSM((int)AlarmStates._Count, (int)AlarmFlags._Count);
            alarmFSM.ForceCurretState((int)AlarmStates.WalkToTC);

            alarmFSM.SetRelation((int)AlarmStates.WalkToTC, (int)AlarmFlags.OnEnemyClose, (int)AlarmStates.RunToTC);
            alarmFSM.SetRelation((int)AlarmStates.RunToTC, (int)AlarmFlags.OnSafe, (int)AlarmStates.WalkToTC);
            alarmFSM.SetRelation((int)AlarmStates.Idle, (int)AlarmFlags.OnEnemyClose, (int)AlarmStates.DefendTC);
            alarmFSM.SetRelation((int)AlarmStates.DefendTC, (int)AlarmFlags.OnSafe, (int)AlarmStates.Idle);
            
            alarmFSM.AddBehaviour((int)AlarmStates.Idle, () => { Debug.Log("Idle"); });
            
            alarmFSM.AddBehaviour((int)AlarmStates.WalkToTC, () =>
            {
                Vector2 dir = GetDir(transform.position, townCenter.position);

                if (!ReachedTarget(townCenter.position))
                {
                    MoveTo(speed, dir);
                }
                else
                {
                    alarmFSM.SetFlag((int)AlarmFlags.OnReachTC);
                }
            });
            alarmFSM.AddBehaviour((int)AlarmStates.WalkToTC, () => { Debug.Log("Walking to Town Center"); });
            
            alarmFSM.AddBehaviour((int)AlarmStates.RunToTC, () =>
            {
                Vector2 dir = GetDir(transform.position, townCenter.position);

                if (!ReachedTarget(townCenter.position))
                {
                    MoveTo(speed * runMod, dir);
                }
                else
                {
                    alarmFSM.SetFlag((int)AlarmFlags.OnReachTC);
                }
            });
            alarmFSM.AddBehaviour((int)AlarmStates.RunToTC, () => { Debug.Log("Running to Town Center"); });
            
            alarmFSM.AddBehaviour((int)AlarmStates.DefendTC, () => { Debug.Log("Shoot Enemies"); });
            alarmFSM.AddBehaviour((int)AlarmStates.DefendTC, () => { Debug.Log("Defending Town Center from Enemies"); });
        }

        Vector3 GetDir(Vector3 origin, Vector3 destination)
        {
            return (destination - origin).normalized;
        }
        void MoveTo(float moveSpeed, Vector2 dir)
        {
            Vector2 movement = dir * moveSpeed * Time.deltaTime;
            transform.position += new Vector3(movement.x, 0, movement.y);
        }
        bool ReachedTarget(Vector2 target)
        {
            return Vector2.Distance(target, transform.position) <= 1.0f;
        }
    }
}