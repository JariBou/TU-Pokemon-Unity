
using System;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Définition des types dans le jeu
    /// </summary>
    public enum TYPE { NORMAL, WATER, FIRE, GRASS, ROCK }

    public static class TypeResolver
    {

        /// <summary>
        /// Récupère le facteur multiplicateur pour la résolution des résistances/faiblesses
        /// WATER faible contre GRASS, resiste contre FIRE
        /// FIRE faible contre WATER, resiste contre GRASS
        /// GRASS faible contre FIRE, resiste contre WATER
        /// </summary>
        /// <param name="attacker">Type de l'attaque (le skill)</param>
        /// <param name="receiver">Type de la cible</param>
        /// <returns>
        /// Normal returns 1 if attacker or receiver
        /// 0.8 if resist
        /// 1.0 if same type
        /// 1.2 if vulnerable
        /// </returns>
        public static float GetFactor(TYPE attacker, TYPE receiver)
        {
            return receiver switch
            {
                TYPE.NORMAL => 1f,
                TYPE.WATER => WaterReceiver(attacker),
                TYPE.FIRE => FireReceiver(attacker),
                TYPE.GRASS => GrassReceiver(attacker),
                TYPE.ROCK => RockReceiver(attacker),
                
                _ => throw new ArgumentOutOfRangeException(nameof(receiver))
            };
        }


        private static float FireReceiver(TYPE attacker)
        {
            return attacker switch
            {
                TYPE.WATER or TYPE.ROCK => 1.2f,
                TYPE.GRASS or TYPE.FIRE => 0.8f,
                _ => 1
            };
        }
        
        private static float WaterReceiver(TYPE attacker)
        {
            return attacker switch
            {
                TYPE.WATER or TYPE.FIRE => 0.8f,
                TYPE.GRASS => 1.2f,
                _ => 1
            };
        }
        
        private static float GrassReceiver(TYPE attacker)
        {
            return attacker switch
            {
                TYPE.FIRE => 1.2f,
                TYPE.GRASS or TYPE.WATER => 0.8f,
                _ => 1
            };
        }
        
        private static float RockReceiver(TYPE attacker)
        {
            return attacker switch
            {
                TYPE.WATER or TYPE.GRASS => 1.2f,
                TYPE.FIRE or TYPE.ROCK => 0.8f,
                _ => 1
            };
        }

    }
}
