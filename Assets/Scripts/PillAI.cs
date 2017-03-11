using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class PillAi : MonoBehaviour
    {
        private List<PillAi> _friends;
        public NavMeshAgent NavAgent { get; set; }
        public List<NeedFullFiller> KnownNeedFullFillers { get; set; }
        public Dictionary<NeedType, double> Needs { get; set; }
        public float UpdateTimeoutValue;
        public double UpdateNeedValue;

        public double RefillTriggerValue;

        private void Start ()
        {
            KnownNeedFullFillers = new List<NeedFullFiller>();
            _friends = new List<PillAi>();
            NavAgent = GetComponent<NavMeshAgent>();
            InvokeRepeating("UpdateNeeds", UpdateTimeoutValue, UpdateTimeoutValue);
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

        public void AddNewFriend(PillAi friend)
        {
            if (!_friends.Contains(friend))
            {
                _friends.Add(friend);
                if (!friend._friends.Contains(this))
                {
                    friend._friends.Add(this);
                }
                friend.Call("Im here!",transform.position);
                ShareKnowledge(friend);
            }
        }

        private void ShareKnowledge(PillAi friend)
        {
            foreach (var knownNeedFullFiller in KnownNeedFullFillers)
            {
                friend.UpdateNeedFillers(knownNeedFullFiller);
            }

            foreach (var friendKnownNeedFullFiller in friend.KnownNeedFullFillers)
            {
                UpdateNeedFillers(friendKnownNeedFullFiller);
            }
        }

        private void Call(string message, Vector3 transformPosition)
        {
            Debug.Log(message);
            //transform.LookAt(transformPosition);
        }

        public void UpdateNeedFillers(NeedFullFiller needFullFiller)
        {
            if (!KnownNeedFullFillers.Contains(needFullFiller))
            {
                Debug.Log("Adding " + needFullFiller);
                KnownNeedFullFillers.Add(needFullFiller);
                foreach (var friend in _friends)
                {
                    friend.UpdateNeedFillers(needFullFiller);
                }
            }

        }
    }
}
