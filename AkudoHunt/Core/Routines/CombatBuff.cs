using System.Threading.Tasks;
using System.Windows.Media;
using Buddy.Coroutines;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals.WoWObjects;
using SB = AkudoHunt.Core.Helpers.SpellBook;

namespace AkudoHunt.Core.Routines
{
    public class CombatBuff
    {
        private static LocalPlayer Me => StyxWoW.Me;
        private static WoWUnit MyCurrentTarget => Me.CurrentTarget;

        public static async Task<bool> Rotation()
        {
            if (Me.IsDead || Me.IsGhost || Me.IsCasting || Me.IsChanneling || Me.IsFlying || Me.OnTaxi || Me.Mounted)
            {
                return true;
            }

            if (await Cast.Buff(SB.TrapLauncher, !Unit.TargetHasAura(Me, SB.TrapLauncher)))
            {
                return true;
            }

            return !MyCurrentTarget.InLineOfSight & await Cast.Buff(SB.AspectOfTheCheetah, !Unit.TargetHasAura(Me, SB.AspectOfTheCheetah));
        }
    }
}