using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class PillAi : MonoBehaviour
    {
        
        private Pill _pill;
        
        public NavMeshAgent NavAgent { get; set; }
        public float UpdateTimeoutValue;
        public double UpdateNeedValue;

        public double RefillTriggerValue;

        private void Start ()
        {
            _pill = new Pill
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

        public Dictionary<NeedType, double> GetNeedsDictionary()
        {
            return _pill.Needs;
        }

        public void SetNeedsDictionary(Dictionary<NeedType, double> needs)
        {
            _pill.Needs = needs;
        }

        public void AppendNewNeed(KeyValuePair<NeedType, double> item)
        {
            _pill.Needs.Add(item.Key,item.Value);
        }

        private void UpdateNeeds()
        {
            Debug.Log("Updating needs");
            var types = _pill.Needs.Keys.ToList();
            foreach (var needsKey in types)
            {
                _pill.Needs[needsKey] -= UpdateNeedValue;
                if (_pill.Needs[needsKey] < RefillTriggerValue)
                {
                    GoAndRefill(needsKey);
                }
            }
        }

        private void GoAndRefill(NeedType need)
        {
            foreach (var fullfiller in _pill.KnownNeedFullFillers)
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
            if (!_pill.Friends.Contains(friend))
            {
                _pill.Friends.Add(friend);
                if (!friend._pill.Friends.Contains(this))
                {
                    friend._pill.Friends.Add(this);
                }
                friend.Call("Im here!",transform.position);
                ShareKnowledge(friend);
            }
        }

        private void ShareKnowledge(PillAi friend)
        {
            foreach (var knownNeedFullFiller in _pill.KnownNeedFullFillers)
            {
                friend.UpdateNeedFillers(knownNeedFullFiller);
            }

            foreach (var friendKnownNeedFullFiller in friend._pill.KnownNeedFullFillers)
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
            if (!_pill.KnownNeedFullFillers.Contains(needFullFiller))
            {
                Debug.Log("Adding " + needFullFiller);
                _pill.KnownNeedFullFillers.Add(needFullFiller);
                foreach (var friend in _pill.Friends)
                {
                    friend.UpdateNeedFillers(needFullFiller);
                }
            }

        }
    }
}
