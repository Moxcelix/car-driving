namespace Core.Grabing
{
    public class Carrier
    {
        public Item Item { get; private set; }

        public void Take(Item item)
        {
            Item = item;

            item.IsTaken = true;
        }

        public void Take(Place place)
        {
            if (!place.IsPlaced)
            {
                return;
            }

            Take(place.TakeItem());
        }

        public void Drop()
        {
            Item.IsTaken = false;

            Item = null;
        }

        public void Drop(Place place)
        {
            if (place.IsPlaced)
            {
                return;
            }

            place.PlaceItem(Item);

            Item.IsTaken = true;

            Item = null;
        }
    }
}