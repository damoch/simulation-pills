using UnityEngine;

namespace Assets.Scripts
{
    public class PillView : MonoBehaviour
    {
        private PillAi _owner;

        private void Start()
        {
            _owner = transform.GetComponentInParent<PillAi>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Pill"))
            {
                _owner.AddNewFriend(other.gameObject.GetComponent<PillAi>());
            }
            if (other.tag.Equals("NeedFiller"))
            {
                _owner.UpdateNeedFillers(other.GetComponent<NeedFullFiller>());
            }
      
        }
    }
}
