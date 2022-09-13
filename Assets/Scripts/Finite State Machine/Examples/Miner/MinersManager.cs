using System.Collections.Concurrent;
using System.Threading.Tasks;
using UnityEngine;

namespace IA.Examples
{
    public class MinersManager : MonoBehaviour
    {
        enum States
        {
            Mining,
            GoToMine,
            GoToDeposit,
            Idle,

            _Count
        }

        enum Flags
        {
            OnFullInventory,
            OnReachMine,
            OnReachDeposit,
            OnEmpyMine,

            _Count
        }

        /*[Header("Set Values")]
        [SerializeField] GameObject mine;
        [SerializeField] GameObject deposit;
        [SerializeField] GameObject minerPrefab;
        [SerializeField] Transform minersEmpty;
        [SerializeField] float speed = 10.0f;
        [SerializeField] float miningTime = 5.0f;
        [SerializeField] ushort mineUses = 10;
        [SerializeField] int miners = 10;
        [Header("Runtime Values")]
        [SerializeField] float currentMiningTime;
        [SerializeField] float deltaTime;
        ConcurrentBag<FSM.FSM> fsms;
        ParallelOptions parallelOptions;

        //Unity Events
        void Start()
        {
            fsms = new ConcurrentBag<FSM.FSM>();
            for (int i = 0; i < miners; i++)
            {
                fsms.Add(new FSM.FSM((int)States._Count, (int)Flags._Count));
            }
            parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = fsms.Count };

            Parallel.ForEach(fsms, parallelOptions, (miner) => { SetMiner(miner); });
        }
        void Update()
        {
            deltaTime = Time.deltaTime;
            Parallel.ForEach(fsms, parallelOptions, (miner) => { ExecuteMinerBehaviour(miner); });
        }

        //Methods
        void SetMiner(FSM.FSM fsm)
        {
            //Instantiate Miner GO
            GameObject GO = Instantiate(minerPrefab, minersEmpty);
            GO.transform.position = transform.position;
            GO.GetComponent<Miner>().defaultFSM = fsm;

            //Force first state
            fsm.ForceCurretState((int)States.GoToMine);

            //Set relations
            fsm.SetRelation((int)States.GoToMine, (int)Flags.OnReachMine, (int)States.Mining);
            fsm.SetRelation((int)States.Mining, (int)Flags.OnFullInventory, (int)States.GoToDeposit);
            fsm.SetRelation((int)States.GoToDeposit, (int)Flags.OnReachDeposit, (int)States.GoToMine);
            fsm.SetRelation((int)States.GoToDeposit, (int)Flags.OnEmpyMine, (int)States.Idle);

            //Add each behaviour
            fsm.AddBehaviour((int)States.Idle, () => { Debug.Log(fsm + ": Idle"); });

            fsm.AddBehaviour((int)States.Mining, () => { MinerMine(fsm); });
            fsm.AddBehaviour((int)States.Mining, () => { Debug.Log(fsm + ": Mining"); });

            fsm.AddBehaviour((int)States.GoToMine, () => { MinerGoToMine(fsm); });
            fsm.AddBehaviour((int)States.GoToMine, () => { Debug.Log(fsm + ": GoToMine"); });

            fsm.AddBehaviour((int)States.GoToDeposit, () => { MinerGoToDep(fsm); });
            fsm.AddBehaviour((int)States.GoToDeposit, () => { Debug.Log(fsm + ": GoToDeposit"); });
        }
        void ExecuteMinerBehaviour(FSM.FSM fsm)
        {
            fsm.Update();
        }
        void MinerMine(FSM.FSM fsm)
        {
            bool mining = currentMiningTime > 0;

            if (mining)
            {
                try
                {
                    mineUses--;
                }
                catch (System.Exception)
                {
                    Debug.Log("Mine runned out of uses");
                    return;
                    throw;
                }
            }

            if (currentMiningTime < miningTime)
            {
                currentMiningTime += deltaTime;
            }
            else
            {
                currentMiningTime = 0.0f;
                fsm.SetFlag((int)Flags.OnFullInventory);
            }
        }
        void MinerGoToMine(FSM.FSM fsm)
        {
            Vector2 dir = (mine.transform.position - transform.position).normalized;

            if (Vector2.Distance(mine.transform.position, transform.position) > 1.0f)
            {
                Vector2 movement = dir * 10.0f * deltaTime;
                transform.position += new Vector3(movement.x, movement.y);
            }
            else
            {
                fsm.SetFlag((int)Flags.OnReachMine);
            }
        }
        void MinerGoToDep(FSM.FSM fsm)
        {
            Vector2 dir = (deposit.transform.position - transform.position).normalized;

            if (Vector2.Distance(deposit.transform.position, transform.position) > 1.0f)
            {
                Vector2 movement = dir * 10.0f * deltaTime;
                transform.position += new Vector3(movement.x, movement.y);
            }
            else
            {
                if (mineUses <= 0)
                    fsm.SetFlag((int)Flags.OnEmpyMine);
                else
                    fsm.SetFlag((int)Flags.OnReachDeposit);
            }
        }*/
    }
}