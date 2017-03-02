using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class PillAI : MonoBehaviour
    {
        public NavMeshAgent NavAgent { get; set; }
        public List<NeedFullFiller> KnownNeedFullFillers { get; set; }
        public Dictionary<NeedType, double> Needs { get; set; }
        public float UpdateTimeoutValue;
        public double UpdateNeedValue;

        public double RefillTriggerValue;

        private void Start ()
        {
            NavAgent = GetComponent<NavMeshAgent>();
            InvokeRepeating("UpdateNeeds", UpdateTimeoutValue, UpdateTimeoutValue);
        }
	
	
        private void Update () {
		
        }

        private void UpdateNeeds()
        {
            List<NeedType> types = Needs.Keys.ToList();
            foreach (var needsKey in types)
            {
                Needs[needsKey] -= UpdateNeedValue;
                Debug.Log(Needs[needsKey]);
            }
        }
    }
}
