using UnityEngine;

namespace Core.Grabing
{
    public class Place : MonoBehaviour
    {
        public Item Item { get; private set; }

        public Item TakeItem()
        {
            var item = Item;

            Item = null;

            return item;
        }

        public bool PlaceItem(Item item)
        {
            if(Item !=  null)
            {
                return false; 
            }

            Item = item;

            return true;
        }
    }
}
