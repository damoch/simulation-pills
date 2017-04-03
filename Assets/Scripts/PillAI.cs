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
                Needs = new Dictionary<NeedType, double>(),
                Items = new List<Item>()
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
                if (fullfiller != null && fullfiller.NeedFullFilled.Equals(need))
                {
                    GoToPoint(fullfiller.transform.position);
                    return;
                }

            }
            GoToPoint(GetRandomLocation());
        }

        public void GoForItem(Vector3 itemLocation)
        {
            GoToPoint(itemLocation);
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

        private void OnCollisionEnter(Collision collision)
        {
            var other = collision.collider.gameObject;
            switch (other.tag)
            {
                case "Item":
                    {
                        var item = other.GetComponent<Item>();
                        Pill.Items.Add(item);
                        Destroy(other.gameObject);
                        break;
                    }
                 default:
                    {
                        break;
                    }
            }
            
        }
        private void OnTriggerEnter(Collider other)
        {
            var otherTag = other.tag;
            switch (otherTag)
            {
                case "NeedFiller":
                {
                        var filler = other.gameObject.GetComponent<NeedFullFiller>();
                        filler.Capacity -= filler.OneFullFillmentValue;
                        Pill.Needs[filler.NeedFullFilled] += filler.OneFullFillmentValue;

                        if (filler.Capacity < 0)
                        {
                            Destroy(other.gameObject);
                        }
                        break;
                }

                default:
                {
                    break;
                }
            }
            if (other.tag.Equals("NeedFiller"))
            {

            }
        }

        private Vector3 GetRandomLocation()
        {
            var navMeshData = NavMesh.CalculateTriangulation();
            
            var t = Random.Range(0, navMeshData.indices.Length - 3);
            
            var point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
            Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

            return point;
        }
    }
}
