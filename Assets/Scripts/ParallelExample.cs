using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace IA.Parallels
{
    public class ParallelExample : MonoBehaviour
    {
        //[Header("Set Values")]
        //[Header("Runtime Values")]

        //Unity Events
        void Start()
        {
            List<int> patatas = new List<int>();
            patatas.Add(0);
            patatas.Add(0);
            patatas.Add(0);
            patatas.Add(0);
            patatas.Add(0);
            patatas.Add(0);
            patatas.Add(0);
            patatas.Add(0);

             
            ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = patatas.Count };

            //Only works with a ConcurrentBag (threadSafe list)
            //Parallel.For(0, patatas.Count, (i) => { patatas[i]++; });
            
            //Doesn't work
            Parallel.ForEach(patatas, parallelOptions, (/*int*/patata) =>
            {
                //patata is a value type, so it will copy it's value, not ref the OG
                patata++;
            });

            for (int i = 0; i < patatas.Count; i++)
            {
                Debug.Log(patatas[i]);
            }

        }

        //Methods
    }
}
