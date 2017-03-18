using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class PillAi : MonoBehaviour
    {

        public Pill Pill { get; set; }

        public NavMeshAgent NavAgent { get; set; }
        public float UpdateTimeoutValue;
        public double UpdateNeedValue;

        public double RefillTriggerValue;

        private void Start ()
        {
            Pill = new Pill
            {
                KnownNeedFullFillers = new List<NeedFullFiller>(),
                Friends = new List<PillAi>(),
                Needs = new Dictionary<NeedType, double>()
            };
            NeedUtils.InitializeNeeds(this);
            NavAgent = GetComponent<NavMeshAgent>();
            InvokeRepeating("UpdateNeeds", UpdateTimeoutValue, UpdateTimeoutValue);
        }
	
	
        private void Update () {
		
        }

      
        private void UpdateNeeds()
        {
            Debug.Log("Updating needs");
            var types = Pill.Needs.Keys.ToList();
            foreach (var needsKey in types)
            {
                Pill.Needs[needsKey] -= UpdateNeedValue;
                if (Pill.Needs[needsKey] < RefillTriggerValue)
                {
                    GoAndRefill(needsKey);
                }
            }
        }

        private void GoAndRefill(NeedType need)
        {
            foreach (var fullfiller in Pill.KnownNeedFullFillers)
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

        public void AddNewFriend(PillAi friend)
        {
            if (!Pill.Friends.Contains(friend))
            {
                Pill.Friends.Add(friend);
                if (!friend.Pill.Friends.Contains(this))
                {
                    friend.Pill.Friends.Add(this);
                }
                friend.Call("Im here!",transform.position);
                ShareKnowledge(friend);
            }
        }

        private void ShareKnowledge(PillAi friend)
        {
            foreach (var knownNeedFullFiller in Pill.KnownNeedFullFillers)
            {
                friend.UpdateNeedFillers(knownNeedFullFiller);
            }

            foreach (var friendKnownNeedFullFiller in friend.Pill.KnownNeedFullFillers)
            {
                UpdateNeedFillers(friendKnownNeedFullFiller);
            }
        }

        private void Call(string message, Vector3 transformPosition)
        {
            Debug.Log(message);
        }

        public void UpdateNeedFillers(NeedFullFiller needFullFiller)
        {
            if (!Pill.KnownNeedFullFillers.Contains(needFullFiller))
            {
                Debug.Log("Adding " + needFullFiller);
                Pill.KnownNeedFullFillers.Add(needFullFiller);
                foreach (var friend in Pill.Friends)
                {
                    friend.UpdateNeedFillers(needFullFiller);
                }
            }

        }
    }
}
