using System;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Définition d'un personnage
    /// </summary>
    public class Character
    {
        /// <summary>
        /// Stat de base, HP
        /// </summary>
        int _baseHealth;
        /// <summary>
        /// Stat de base, ATK
        /// </summary>
        int _baseAttack;
        /// <summary>
        /// Stat de base, DEF
        /// </summary>
        int _baseDefense;
        /// <summary>
        /// Stat de base, SPE
        /// </summary>
        int _baseSpeed;
        /// <summary>
        /// Type de base
        /// </summary>
        TYPE _baseType;

        public Character(int baseHealth, int baseAttack, int baseDefense, int baseSpeed, TYPE baseType)
        {
            _baseHealth = baseHealth;
            _baseAttack = baseAttack;
            _baseDefense = baseDefense;
            _baseSpeed = baseSpeed;
            _baseType = baseType;
            CurrentHealth = baseHealth;
        }
        /// <summary>
        /// HP actuel du personnage
        /// </summary>
        public float CurrentHealth { get; private set; }
        public TYPE BaseType => _baseType;

        /// <summary>
        /// HPMax, prendre en compte base et equipement potentiel
        /// </summary>
        public int MaxHealth => _baseHealth + (CurrentEquipment?.BonusHealth ?? 0);

        /// <summary>
        /// ATK, prendre en compte base et equipement potentiel
        /// </summary>
        public int Attack => _baseAttack + (CurrentEquipment?.BonusAttack ?? 0);

        /// <summary>
        /// DEF, prendre en compte base et equipement potentiel
        /// </summary>
        public int Defense => _baseDefense + (CurrentEquipment?.BonusDefense ?? 0);

        /// <summary>
        /// SPE, prendre en compte base et equipement potentiel
        /// </summary>
        public int Speed => _baseSpeed + (CurrentEquipment?.BonusSpeed ?? 0);
        /// <summary>
        /// Equipement unique du personnage
        /// </summary>
        public Equipment CurrentEquipment { get; private set; }
        /// <summary>
        /// null si pas de status
        /// On va dire qu'on ne peut avoir qu'un status a la fois pour l'instant
        /// </summary>
        public StatusEffect CurrentStatus { get; private set; }

        public bool IsAlive => CurrentHealth > 0;


        /// <summary>
        /// Application d'un skill contre le personnage
        /// On pourrait potentiellement avoir besoin de connaitre le personnage attaquant,
        /// Vous pouvez adapter au besoin
        /// </summary>
        /// <param name="s">skill attaquant</param>
        /// <exception cref="NotImplementedException"></exception>
        public void ReceiveAttack(Skill s)
        {
            if (s is null) throw new ArgumentNullException();

            // Je suppose que le multiplicateur du type est pris en compte avant la défense (semble plus logique)
            float damage = s.Power * TypeResolver.GetFactor(s.Type, BaseType) - Defense;
            damage = Math.Clamp(damage, 0, MaxHealth); // In case armor is greater than atk dmg it heals the pok without this lol
            TakeDamage(damage);

            ApplyNewStatusEffect(s.Status);
        }
        /// <summary>
        /// Equipe un objet au personnage
        /// </summary>
        /// <param name="newEquipment">equipement a appliquer</param>
        /// <exception cref="ArgumentNullException">Si equipement est null</exception>
        public void Equip(Equipment newEquipment)
        {
            CurrentEquipment = newEquipment ?? throw new ArgumentNullException();
        }
        /// <summary>
        /// Desequipe l'objet en cours au personnage
        /// </summary>
        public void Unequip()
        {
            CurrentEquipment = null;
        }

        public void Heal(int amount)
        {
            CurrentHealth = Math.Clamp(CurrentHealth + amount, 0, MaxHealth);
        }

        public void EndTurn()
        {
            if (CurrentStatus is not null)
            {
                CurrentStatus.EndTurn();
                if (CurrentStatus.RemainingTurn == 0)
                {
                    CurrentStatus = null;
                }
            }
        }

        public void ApplyStatusEffect(StatusEffect effect)
        {
            CurrentStatus ??= effect;
        }

        public void ApplyNewStatusEffect(StatusPotential status)
        {
            ApplyStatusEffect(StatusEffect.GetNewStatusEffect(status));
        }

        public void TakeDamage(float amount)
        {
            CurrentHealth = Math.Clamp(CurrentHealth - amount, 0, MaxHealth);
        }

        public void RemoveStatusEffect()
        {
            CurrentStatus = null;
        }
    }
}
