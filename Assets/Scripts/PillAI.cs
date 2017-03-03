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
            KnownNeedFullFillers = GameObject.FindObjectsOfType<NeedFullFiller>()
                .ToList();
        }
	
	
        private void Update () {
		
        }

        private void UpdateNeeds()
        {
            Debug.Log("Updating needs");
            var types = Needs.Keys.ToList();
            foreach (var needsKey in types)
            {
                Needs[needsKey] -= UpdateNeedValue;
                if (Needs[needsKey] < RefillTriggerValue)
                {
                    GoAndRefill(needsKey);
                }

            }
        }

        private void GoAndRefill(NeedType need)
        {
            foreach (var fullfiller in KnownNeedFullFillers)
            {
                if (fullfiller.NeedFullFilled.Equals(need))
                {
                    GoToPoint(fullfiller.transform.position);
                }
            }
        }

        private void GoToPoint(Vector3 transformPosition)
        {
            NavAgent.destination = transformPosition;
        }
    }
}
