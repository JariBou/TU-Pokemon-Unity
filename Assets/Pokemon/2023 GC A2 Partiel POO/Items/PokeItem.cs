using System;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    public enum ItemType
    {
        Potion, IceBucket, RehabFreeEntry
    }

    public enum ItemPrefabs
    {
        SmallPotion, MediumPotion, GrandPotion, OceanPotion, AntiPoison, AntiBurn, AntiSleep
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

        public static PokeItem CreateItem(ItemPrefabs prefab)
        {
            switch (prefab)
            {
                case ItemPrefabs.SmallPotion:
                    return new Potion(50);
                case ItemPrefabs.MediumPotion:
                    return new Potion(100);
                case ItemPrefabs.GrandPotion:
                    return new Potion(150);
                case ItemPrefabs.OceanPotion:
                    return new Potion(256);
                case ItemPrefabs.AntiPoison:
                    return new RehabFreeEntry();
                case ItemPrefabs.AntiBurn:
                    return new IceBucket();
                case ItemPrefabs.AntiSleep:
                    return new WakeUpThisNoHotel();
                default:
                    throw new ArgumentOutOfRangeException(nameof(prefab), prefab, null);
            }
        }
    }
}