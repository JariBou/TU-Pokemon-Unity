using UnityEditor;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    public class WakeUpThisNoHotel : PokeItem
    {
        public WakeUpThisNoHotel() : base(ItemType.RehabFreeEntry)
        {
            
        }
        
        public override bool UseOn(Character pokemon)
        {
            if (_used) return false;
            if (pokemon.CurrentStatus is null) return false;
            if (pokemon.CurrentStatus.GetType() != typeof(SleepStatus)) return false;
            
            pokemon.RemoveStatusEffect();
            _used = true;
            return true;
        }
    }
}