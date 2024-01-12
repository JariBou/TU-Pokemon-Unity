namespace _2023_GC_A2_Partiel_POO.Level_2
{
    public enum ItemType
    {
        Potion, IceBucket, RehabFreeEntry
    }
    
    public abstract class PokeItem
    {
        public ItemType GetItemType { get; }
        protected bool _used;

        public PokeItem(ItemType type)
        {
            GetItemType = type;
        }
        
        public abstract bool UseOn(Character pokemon);
    }
}