using UnityEditor;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    public class IceBucket : PokeItem
    {
        public IceBucket() : base(ItemType.IceBucket)
        {
            
        }
        
        public override bool UseOn(Character pokemon)
        {
            if (_used) return false;
            if (pokemon.CurrentStatus is null) return false;
            if (pokemon.CurrentStatus.GetType() != typeof(BurnStatus)) return false;
            
            pokemon.RemoveStatusEffect();
            _used = true;
            return true;
        }
    }
}