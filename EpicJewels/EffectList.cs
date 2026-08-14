using Jewelcrafting;
using System.Collections.Generic;

namespace EpicJewels.GemEffects
{
    public static class EffectList
    {
        // Firestarter,
        // Iceheart,
        // Snakebite,
        public enum DmgEffect
        {
            AddBluntDamage,
            AddPierceDamage,
            AddSlashDamage,
            AddSpiritDamage,
            AddLightningDamage,
            AddPickaxeDamage,
            AddChopDamage
        }
        
        // public static IReadOnlyList<T> GetDmgValues<T>() { return (T[])DmgEffect.GetValues(typeof(T)); }

        public static void AddGemEffects()
        {
            // Arguments here are English fallbacks only. Jewelcrafting stores them as the values of
            // jc_effect_<name>, _desc and _desc_detail, and Localization.Localize does a single pass
            // with no rescan - so a $token passed here would render literally. Keep these plain English;
            // the real translations live in Localizations/*.json under those jc_effect_* keys.
            API.AddGemEffect<BluntResistance.Config>("Blunt Resistance", "Increases your blunt resistance.", "Reduces all blunt damage you take by $1%.");
            API.AddGemEffect<PierceResistance.Config>("Pierce Resistance", "Increases your pierce resistance.", "Reduces all pierce damage you take by $1%.");
            API.AddGemEffect<SlashResistance.Config>("Slash Resistance", "Increases your slash resistance.", "Reduces all slash damage you take by $1%.");
            API.AddGemEffect<FireResistance.Config>("Fire Resistance", "Increases your fire resistance.", "Reduces all fire damage you take by $1%.");
            API.AddGemEffect<PoisonResistance.Config>("Poison Resistance", "Increases your poison resistance.", "Reduces all poison damage you take by $1%.");
            API.AddGemEffect<LightningResistance.Config>("Lightning Resistance", "Increases your lightning resistance.", "Reduces all lightning damage you take by $1%.");
            API.AddGemEffect<AddBluntDamage.Config>("Add Blunt Damage", "Additional blunt damage.", "$2% chance to do blunt damage equal to $1% of your base weapons damage.");
            API.AddGemEffect<AddPierceDamage.Config>("Add Pierce Damage", "Additional piercing damage.", "$2% chance to do pierce damage equal to $1% of your base weapons damage.");
            API.AddGemEffect<AddSlashDamage.Config>("Add Slash Damage", "Additional slash damage.", "$2% chance to do slash damage equal to $1% of your base weapons damage.");
            API.AddGemEffect<AddSpiritDamage.Config>("Add Spirit Damage", "Additional spirit damage.", "$2% chance to do spirit damage equal to $1% of your base weapons damage.");
            API.AddGemEffect<AddLightningDamage.Config>("Add Lightning Damage", "Additional lightning damage.", "$2% chance to do lightning damage equal to $1% of your base weapons damage.");
            API.AddGemEffect<AddPickaxeDamage.Config>("Add Pickaxe Damage", "Additional pickaxe damage.", "$2% chance to do pickaxe damage equal to $1% of your base tools damage.");
            API.AddGemEffect<AddChopDamage.Config>("Add Chop Damage", "Additional woodcutting damage.", "$2% chance to do woodcutting damage equal to $1% of your base tools damage.");
            API.AddGemEffect<Inferno.Config>("Inferno", "Chance to do massive fire damage.", "$2% chance to do a bonus $1% of your total hits damage as fire damage.");
            API.AddGemEffect<IncreaseEitr.Config>("Increase Eitr", "Increases Eitr.", "Increases total eitr by $1%.");
            API.AddGemEffect<IncreaseStamina.Config>("Increase Stamina", "Increases stamina.", "Increases total stamina by $1%.");
            API.AddGemEffect<IncreaseStaminaRegen.Config>("Increase Stamina Regen", "Increases your stamina regeneration.", "Increases your stamina regeneration by $1%.");
            API.AddGemEffect<BlockReduceStamina.Config>("Block Reduce Stamina", "Reduces stamina cost when blocking.", "Reduces the stamina cost for blocking by $1%.");
            API.AddGemEffect<WeaponReducedStamina.Config>("Weapon Reduced Stamina", "Reduces stamina cost when attacking.", "Reduces the stamina cost for attacking by $1%.");
            API.AddGemEffect<CoinGreed.Config>("Coin Greed", "Enemies have a chance to drop coins.", "Enemies have a $2% chance to drop between 1-$1 coins.");
            API.AddGemEffect<CoinHoarder.Config>("Coin Hoarder", "Increases your damage based on carried coins.", "Increase all of your damage by a percentage of the total coins you carry.");
            API.AddGemEffect<WaterResistant.Config>("Water Resistant", "Prevents becoming wet for a period of time.", "Prevents becoming wet for $1 seconds water exposure.");
            API.AddGemEffect<WaterFrenzy.Config>("Water Frenzy", "Increases your damage done when wet.", "Increases your damage dealt by $1% when wet.");
            API.AddGemEffect<WaterSwiftness.Config>("Water Swiftness", "Increases your speed when wet.", "Increases your speed by $1% when wet.");
            API.AddGemEffect<BurningViking.Config>("Burning Viking", "Increases your speed when on fire.", "Increases your speed by $1% when burning.");
            API.AddGemEffect<BurningFrenzy.Config>("Burning Frenzy", "Increases your damage when on fire.", "Increases your damage dealt by $1% when burning.");
            API.AddGemEffect<ExpertFisher.Config>("Expert Fisher", "Increases your fishing skill.", "Your fishing skill is $1 higher. Visible on your skills page.");
            API.AddGemEffect<ExpertMage.Config>("Expert Mage", "Increases your elemental and blood magic skills.", "Your blood and elemental magic skills are $1% higher. Visible on your skills page.");
            API.AddGemEffect<ExpertHarvester.Config>("Expert Harvester", "Increase your chopping and pickaxe skills.", "Your chopping and pickaxe skills are $1% higher. Visible on your skills page.");
            API.AddGemEffect<ExpertBrawler.Config>("Expert Brawler", "Increases your fists skill.", "Your fists skill is $1 higher. Visible on your skills page.");
            API.AddGemEffect<ExpertAcrobat.Config>("Expert Acrobat", "Increases your jump skill.", "Your jump skill is $1 higher. Visible on your skills page.");
            API.AddGemEffect<ExpertDaggers.Config>("Expert Daggers", "Increases your knife skill.", "Your knives skill is $1 higher. Visible on your skills page.");
            API.AddGemEffect<ExpertSwordsman.Config>("Expert Swordsman", "Increases your swords skill.", "Your sword skill is $1 higher. Visible on your skills page.");
            API.AddGemEffect<ExpertSmasher.Config>("Expert Smasher", "Increases your maces skill.", "Your maces skill is $1 higher. Visible on your skills page.");
            API.AddGemEffect<ExpertPolearms.Config>("Expert Polearms", "Increases your polearm skill.", "Your polearms skill is $1 higher. Visible on your skills page.");
            API.AddGemEffect<ExpertSpearmaiden.Config>("Expert Spearmaiden", "Increases your spear skill.", "Your spear skill is $1 higher. Visible on your skills page.");
            API.AddGemEffect<ExpertAxemaster.Config>("Expert Axemaster", "Increases your axes skill.", "Your axes skill is $1 higher. Visible on your skills page.");
            API.AddGemEffect<ExpertSprinter.Config>("Expert Sprinter", "Increases your running skill.", "Your running skill is $1 higher. Visible on your skills page.");
            API.AddGemEffect<ReduceWeight.Config>("Reduce Weight", "Reduces total weight of items.", "Reduces the total weight of everything you carry by $1%.");
            API.AddGemEffect<CoverOfDarkness.Config>("Cover of Darkness", "Chance to spawn friendly bats when fighting.", "$2% chance on hit to spawn a bat which lasts for $1 seconds.");
            API.AddGemEffect<EitrConversion.Config>("Eitr Conversion", "Chance to restore eitr on block.", "$2% chance on block to restore $1% eitr.");
            API.AddGemEffect<Retribution.Config>("Retribution", "Chance to return damage when blocking.", "$2% chance on block to return $1% damage to the attacker.");
            API.AddGemEffect<StaggeringBlock.Config>("Staggering Block", "Blocking can stagger attackers.", "$1% chance to stagger your attacker.");
            API.AddGemEffect<FlamingGuard.Config>("Burning Guard", "On block chance to return fire damage.", "$1% chance to set your attacker on fire for $2% of the blocked damage.");
            API.AddGemEffect<FreezingGuard.Config>("Freezing Guard", "On block chance to return frost damage.", "$1% chance to return frost damage for $2% of the blocked damage.");
            API.AddGemEffect<WetWorker.Config>("Wet Worker", "Reduces stamina usage when wet.", "$1% usage stamina cost reduction when wet.");
            API.AddGemEffect<EitrFused.Config>("Eitr Fused", "Uses eitr to increase damage.", "$1% increase to damage at the cost of $2 eitr per hit.");
            API.AddGemEffect<Farmer.Config>("Farmer", "Chance for bigger harvests, autopick nearby.", "$2% chance to get $1 additional crops when harvesting. Autopicks nearby crops.");
            API.AddGemEffect<Toxifier.Config>("Toxifier", "Gain adrenaline from poison damage around you.", "Gain $1 adrenaline whenever poison damage is applied within $2m.");
            API.AddGemEffect<AdrenalRewire.Config>("Adrenal Rewire", "Gain adrenaline from damage taken.", "Gain a small amount of adrenaline for each point of damage taken.");
            API.AddGemEffect<SoakedFury.Config>("Soaked Fury", "Gain additional adrenaline while wet.", "Gain $1% more adrenaline while you are wet.");
            API.AddGemEffect<BurningAdrenaline.Config>("Burning Adrenaline", "Gain additional adrenaline while on fire.", "Gain $1% more adrenaline while you are on fire.");
            API.AddGemEffect<HarvestAdrenaline.Config>("Harvest Adrenaline", "Chance to gain adrenaline from harvesting.", "$2% chance to gain $1% of damage done as adrenaline.");
            API.AddGemEffect<EitrFeedback.Config>("Eitr Feedback", "Chance to restore adrenaline from eitr use.", "$2% chance to restore $1 adrenaline when consuming eitr.");

            // These are synergies
            API.AddGemEffect<CombatSpirit.Config>("Combat Spirit", "A spirit helps you in combat.", "A spirit aids you in combat for $1 seconds. Returns after a cooldown.");
            API.AddGemEffect<IntenseFire.Config>("Intense Fire", "An affinity for fire.", "You are +$1% fire resistant and have a higher chance to trigger Inferno.");
            API.AddGemEffect<SlipperyWhenWet.Config>("Slippery When Wet", "Water quickens you.", "You are $1% faster when wet.");
            API.AddGemEffect<Waterproof.Config>("Waterproof", "You do not get wet.", "You do not get wet.");
            API.AddGemEffect<WeaponMaster.Config>("Weapon Master", "Experienced with weapons.", "Your skill with all weapons is $1% higher.");
            API.AddGemEffect<Spellsword.Config>("Spellsword", "Use eitr to increase weapon damage.", "$1% increase to damage at the cost of 5 eitr per hit.");
        }
    }
}
