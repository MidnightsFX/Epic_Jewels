using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicJewels.EffectHelpers {
    internal static class Utils {

        internal static float GetTotalDamageOptions(this HitData.DamageTypes hitdmg, bool include_poison = false, bool include_spirit = false, bool include_pickaxe_and_chop = false, float modElement = 1f, float modPhysical = 1f) {
            float physical = (hitdmg.m_damage + hitdmg.m_blunt + hitdmg.m_slash + hitdmg.m_pierce) * modPhysical;
            float elemental = (hitdmg.m_fire + hitdmg.m_frost + hitdmg.m_lightning) * modElement;
            float dmg = physical + elemental;
            if (include_poison) { dmg += (hitdmg.m_poison * modElement); }
            if (include_spirit) { dmg += (hitdmg.m_spirit * modElement); }
            if (include_pickaxe_and_chop) { dmg += hitdmg.m_pickaxe + hitdmg.m_chop; }
            //Logger.LogDebug($"Total Damage calc: {dmg} (with modifiers E:{modElement}, P:{modPhysical}) = true:{hitdmg.m_damage} + blunt:{hitdmg.m_blunt} + slash:{hitdmg.m_slash} + pierce:{hitdmg.m_pierce} + fire:{hitdmg.m_fire} + frost:{hitdmg.m_frost} + Lightning:{hitdmg.m_lightning}");
            //Logger.LogDebug($"Optionals: Poison:{hitdmg.m_poison} Spirit:{hitdmg.m_spirit} Pickaxe:{hitdmg.m_pickaxe} Chop:{hitdmg.m_chop}");
            return dmg;
        }
    }
}
