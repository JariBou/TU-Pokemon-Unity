using UnityEditor;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    public class Potion : PokeItem
    {
        private int _power;
        
        public Potion(int healAmount) : base(ItemType.Potion)
        {
            _power = healAmount;
        }
        
        public override bool UseOn(Character pokemon)
        {
            if (_used) return false;
            pokemon.Heal(_power);
            _used = true;
            return true;
        }
    }
}