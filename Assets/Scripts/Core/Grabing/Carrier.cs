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

        public void Drop()
        {
            Item.IsTaken = false;

            Item = null;
        }
    }
}