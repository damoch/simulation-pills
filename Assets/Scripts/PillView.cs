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
            var otherTag = other.tag;
            switch (otherTag)
            {
                case "Pill":
                {
                    var friend = other.GetComponent<PillAi>();
                        _owner.AddNewFriend(friend);
                        break;
                }
                case "NeedFiller":
                {
                    var needFiller = other.GetComponent<NeedFullFiller>();
                        _owner.UpdateNeedFillers(needFiller);
                        break;
                }
                case "Item":
                {
                    var otherLocation = other.gameObject.transform.position;
                        _owner.GoForItem(otherLocation);
                    break;
                }
                default:
                {
                    break;
                }
            }

      
        }
    }
}
