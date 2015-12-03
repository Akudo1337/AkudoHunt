using System.Threading.Tasks;
using Styx;
using Styx.WoWInternals.WoWObjects;
using SB = AkudoHunt.Core.Helpers.SpellBook;

namespace AkudoHunt.Core.Routines
{
    public class Heal
    {
        private static LocalPlayer Me => StyxWoW.Me;
        private static WoWUnit MyCurrentTarget => Me.CurrentTarget;

        public static async Task<bool> Rotation()
        {
            if (Me.IsDead || Me.IsGhost || Me.IsCasting || Me.IsChanneling || Me.IsFlying || Me.OnTaxi)
            {
                return false;
            }

            return await HealRotation();
        }

        private static async Task<bool> HealRotation()
        {
            // Logic for Heal abilities here - fx. if BM and pet is spirit beast.
            if (Me.Specialization != WoWSpec.HunterBeastMastery)
            {
                return false;
            }

            if (await Cast.CastSpell(SB.SpiritMend, Me))
            {
                return true;
            }

            return false;
        }
    }
}