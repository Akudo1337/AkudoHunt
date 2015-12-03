using System.Linq;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;


namespace AkudoHunt.Core
{
    internal static class Unit
    {
        private static LocalPlayer Me => StyxWoW.Me;
        private static WoWUnit MyCurrentTarget => Me.CurrentTarget;

        public static bool TargetHasAura(WoWUnit target, int aura, bool isMyAura = false)
        {
            if (target == null || !target.IsValid)
            {
                return false;
            }
            return (isMyAura
                ? target.GetAllAuras().FirstOrDefault(ctx => ctx.SpellId == aura && ctx.CreatorGuid == Me.Guid)
                : target.GetAllAuras().FirstOrDefault(ctx => ctx.SpellId == aura)) != null;
        }


        public static double AuraTimeLeft(WoWUnit unit, int auraId, bool isMyAura = false)
        {
            if (unit == null || !unit.IsValid)
                return 0;
            WoWAura aura = isMyAura
                ? unit.GetAllAuras().FirstOrDefault(A => A.SpellId == auraId && A.CreatorGuid == Me.Guid)
                : unit.GetAllAuras().FirstOrDefault(A => A.SpellId == auraId);
            return aura?.TimeLeft.TotalMilliseconds ?? 0;
        }
    }
}