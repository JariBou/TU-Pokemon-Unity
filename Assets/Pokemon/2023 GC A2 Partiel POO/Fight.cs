
using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    public class Fight
    {
        public Fight(Character character1, Character character2)
        {
            if (character1 is null || character2 is null) throw new ArgumentNullException();
            Character1 = character1;
            Character2 = character2;
        }
        
        public Character Character1 { get; }
        public Character Character2 { get; }
        
        /// <summary>
        /// Est-ce la condition de victoire/défaite a été rencontré ?
        /// </summary>
        public bool IsFightFinished => Character1.CurrentHealth == 0 || Character2.CurrentHealth == 0;

        /// <summary>
        /// Jouer l'enchainement des attaques. Attention à bien gérer l'ordre des attaques par apport à la speed des personnages
        /// </summary>
        /// <param name="skillFromCharacter1">L'attaque selectionné par le joueur 1</param>
        /// <param name="skillFromCharacter2">L'attaque selectionné par le joueur 2</param>
        /// <exception cref="ArgumentNullException">si une des deux attaques est null</exception>
        public void ExecuteTurn(Skill skillFromCharacter1, Skill skillFromCharacter2)
        {
            if (skillFromCharacter1 is null || skillFromCharacter2 is null) throw new ArgumentNullException();
            
            // needs to be done every turn since speed can be affected by status
            List<Character> turnOrder = new List<Character>(2) { Character1, Character2 };
            if (Character1.Speed == Character2.Speed)
            {
                turnOrder = turnOrder.OrderBy(x => Random.value).ToList();
            }
            else
            {
                turnOrder.Sort((character, character1) => character1.Speed.CompareTo(character.Speed));
            }

            List<Skill> attacksInOrder = turnOrder[0] == Character1 
                ? new List<Skill>(2) { skillFromCharacter1, skillFromCharacter2 } 
                : new List<Skill>(2) { skillFromCharacter2, skillFromCharacter1 };

            for (int i = 0; i < 2; i++)
            {
                DoTurn(i, turnOrder, attacksInOrder);
                
                if (IsFightFinished) break;
            }
            
        }

        private void DoTurn(int pokeIndex, List<Character> turnOrder, List<Skill> attacksInOrder)
        {
            Character turnChar = turnOrder[pokeIndex];

            StatusEffect currStatus = turnChar.CurrentStatus;
            if (currStatus is not null)
            {

                turnChar.TakeDamage(currStatus.DamageEachTurn);
                
                if (!currStatus.CanAttack)
                {
                    turnChar.TakeDamage(attacksInOrder[pokeIndex].Power * currStatus.DamageOnAttack);
                    
                    turnChar.EndTurn();
                    return;
                }
                
            }
            
            turnOrder[(pokeIndex + 1) % 2].ReceiveAttack(attacksInOrder[pokeIndex]);
            turnChar.EndTurn();
            // 1 tour de status = un tour DU POKEMON, pas du combat
        }
    }
}
