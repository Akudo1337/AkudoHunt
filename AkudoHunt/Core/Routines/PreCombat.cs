using System.Threading.Tasks;
using Styx;
using Styx.WoWInternals.WoWObjects;
using SB = AkudoHunt.Core.Helpers.SpellBook;
using U = AkudoHunt.Core.Unit;

namespace AkudoHunt.Core.Routines
{
    public class PreCombat
    {
        private static LocalPlayer Me => StyxWoW.Me;
        private static WoWUnit MyCurrentTarget => Me.CurrentTarget;

        public static async Task<bool> Rotation()
        {
            if (Me.IsDead || Me.IsGhost || Me.IsCasting || Me.IsChanneling || Me.IsFlying || Me.OnTaxi || Me.Mounted)
            {
                return true;
            }

            if (await Cast.Buff(SB.TrapLauncher, !U.TargetHasAura(Me, SB.TrapLauncher)))
            {
                return true;
            }

            return await Cast.Buff(SB.AspectOfTheCheetah, !U.TargetHasAura(Me, SB.AspectOfTheCheetah));
        }
    }
}