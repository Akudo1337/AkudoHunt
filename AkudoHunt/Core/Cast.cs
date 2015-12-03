using System.Threading.Tasks;
using Buddy.Coroutines;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals.WoWObjects;

namespace AkudoHunt.Core
{
    public static class Cast
    {
        public static LocalPlayer Me => StyxWoW.Me;
        public static WoWUnit MyCurrentTarget => Me.CurrentTarget;

        public static async Task<bool> CastGroundSpell(int spell, WoWPoint targetLoc)
        {
            if (!SpellManager.CanCast(spell))
            {
                return false;
            }

            if (SpellManager.Cast(spell))
            {
                return false;
            }

            if (!await Coroutine.Wait(1000, () => Me.CurrentPendingCursorSpell != null))
            {
                return false;
            }

            SpellManager.ClickRemoteLocation(targetLoc);

            await CommonCoroutines.SleepForLagDuration();

            return true;
        }

        public static async Task<bool> CastSpell(int spell, WoWUnit target)
        {
            if (!SpellManager.CanCast(spell))
            {
                return false;
            }

            if (!SpellManager.Cast(spell, target))
            {
                return false;
            }

            await CommonCoroutines.SleepForLagDuration();

            return true;
        }

        public static async Task<bool> CastSpell(int spell)
        {
            return await CastSpell(spell, MyCurrentTarget);
        }

        public static async Task<bool> RemoveAura(int auraId)
        {
            if (Me.HasAura(auraId))
            {
                await CastSpell(auraId);
            }

            await CommonCoroutines.SleepForLagDuration();
            return true;
        }

        public static async Task<bool> UseDpsTrinket(WoWItem item)
        {
            if (item.CooldownTimeLeft.TotalMilliseconds <= 0 && item.Usable)
            {
                item.Use();
            }

            await CommonCoroutines.SleepForLagDuration();
            return true;
        }

        public static async Task<bool> Buff(int spell, bool hasBuff)
        {
            if (!hasBuff && !SpellManager.CanBuff(spell))
            {
                return false;
            }

            if (!hasBuff && !SpellManager.Buff(spell))
            {
                return false;
            }

            await CommonCoroutines.SleepForLagDuration();
            return true;
        } 

    }
}