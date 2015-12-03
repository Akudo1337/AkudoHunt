using System.Threading.Tasks;
using System.Windows.Media;
using AkudoHunt.Core.Managers;
using Buddy.Coroutines;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using SB = AkudoHunt.Core.Helpers.SpellBook;

namespace AkudoHunt.Core.Routines
{
    public class Combat
    {
        private static LocalPlayer Me => StyxWoW.Me;
        private static WoWUnit MyCurrentTarget => Me.CurrentTarget;

        public static async Task<bool> Rotation()
        {
            if ((Me.IsCasting && HotKeyManager.FreezingTrapFocusNoLauncher == false) ||
                Me.IsChanneling && HotKeyManager.FreezingTrapFocusNoLauncher == false || Me.IsFlying || Me.OnTaxi)
            {
                return false;
            }

            #region Freezing Trap
            if (HotKeyManager.FreezingTrapFocus && Me.FocusedUnit.InLineOfSpellSight &&
                await Cast.CastGroundSpell(SB.FreezingTrap, Me.FocusedUnit.Location))
            {
                HotKeyManager.FreezingTrapFocus = false;
                return true;
            }

            if (HotKeyManager.FreezingTrapFocusNoLauncher && Me.FocusedUnit != null &&
                SpellManager.CanCast(SB.FreezingTrap))
            {
                if (Me.FocusedUnit.Distance2D <= 2)
                {
                    Logging.Write(Colors.Aqua, "In Trap without launcher.");
                    SpellManager.StopCasting();
                    await Cast.RemoveAura(SB.TrapLauncher);
                    if (!await Coroutine.Wait(3000, () => !Me.HasAura(SB.TrapLauncher)))
                    {
                        Logging.Write(Colors.Red, "Trap launcher aura was not removed within 3 seconds");
                        return true;
                    }

                    await CommonCoroutines.SleepForLagDuration();

                    return await Cast.CastSpell(SB.FreezingTrap);
                }
            }
            #endregion

            #region Snake - Or Ice Trap.

            if (HotKeyManager.SnakeOrIceTrapCurrentTarget && Me.CurrentTarget.InLineOfSpellSight &&
                await Cast.CastGroundSpell(SB.IceTrap, MyCurrentTarget.Location))
            {
                HotKeyManager.SnakeOrIceTrapCurrentTarget = false;
                return true;
            }

            if (Me.FocusedUnit != null && (HotKeyManager.SnakeOrIceTrapFocus && Me.FocusedUnit.InLineOfSpellSight && await Cast.CastGroundSpell(SB.IceTrap, Me.FocusedUnit.Location)))
            {
                HotKeyManager.SnakeOrIceTrapFocus = false;
                return true;
            }


            if (HotKeyManager.SnakeOrIceTrapunderMeNoLauncher && SpellManager.CanCast(SB.IceTrap))
            {
                await Cast.RemoveAura(SB.TrapLauncher);
                if (!await Coroutine.Wait(3000, () => !Me.HasAura(SB.TrapLauncher)))
                {
                    Logging.Write(Colors.Red, "Trap launcher aura was not removed within 3 seconds");
                    return true;
                }

                await CommonCoroutines.SleepForLagDuration();

                return await Cast.CastSpell(SB.IceTrap);
            }
            #endregion

            #region Explosive Trap

            if (HotKeyManager.ExplosiveTrapCurrentTarget && MyCurrentTarget.InLineOfSpellSight && await Cast.CastGroundSpell(SB.ExplosiveTrap, MyCurrentTarget.Location))
            {
                HotKeyManager.ExplosiveTrapCurrentTarget = false;
                return true;
            }

            if (Me.FocusedUnit != null && (HotKeyManager.ExplosiveTrapFocus && Me.FocusedUnit.InLineOfSpellSight && await Cast.CastGroundSpell(SB.ExplosiveTrap, Me.FocusedUnit.Location)))
            {
                HotKeyManager.ExplosiveTrapFocus = false;
                return true;
            }

            if (HotKeyManager.ExplosiveTrapUnderMeNoLauncher && SpellManager.CanCast(SB.ExplosiveTrap))
            {
                await Cast.RemoveAura(SB.TrapLauncher);
                if (!await Coroutine.Wait(3000, () => !Me.HasAura(SB.TrapLauncher)))
                {
                    Logging.Write(Colors.Red, "Trap launcher aura was not removed within 3 seconds");
                    return true;
                }

                await CommonCoroutines.SleepForLagDuration();

                return await Cast.CastSpell(SB.ExplosiveTrap);
            }
            #endregion

            #region What Rotation to run.
            // Single target rotation for now.
            if (Me.Specialization == WoWSpec.HunterBeastMastery)
            {
                return await BeastMasteryRotation();
            }

            // Single target rotation for now.
            if (Me.Specialization == WoWSpec.HunterMarksmanship)
            {
                return await MarksmanshipRotation();
            }

            // Single target rotation for now.
            if (Me.Specialization == WoWSpec.HunterSurvival)
            {
                return await SurvivalRotation();
            }
            #endregion

            return false;
        }

        private static async Task<bool> BeastMasteryRotation()
        {
            if (await Cast.CastSpell(SB.KillShot))
            {
                return true;
            }

            if (Managers.HotKeyManager.CooldownsOn)
            {
                if (await Cast.CastSpell(SB.BestialWrath))
                {
                    return true;
                }

                if (await Cast.CastSpell(SB.AMurderOfCrows))
                {
                    return true;
                }

                if (await Cast.UseDpsTrinket(Me.Inventory.Equipped.Trinket1))
                {
                    return true;
                }

                if (await Cast.CastSpell(SB.DireBeast))
                {
                    return true;
                }
            }

            if (await Cast.CastSpell(SB.KillCommand))
            {
                return true;
            }

            if (await Cast.CastSpell(SB.Barrage))
            {
                return true;
            }

            if (Conditions.Focus >= 75 && await Cast.CastSpell(SB.ArcaneShot))
            {
                return true;
            }

            return await Cast.CastSpell(SB.CobraShot);
        }

        private static async Task<bool> MarksmanshipRotation()
        {
            if (await Cast.CastSpell(SB.ChimaeraShot))
            {
                return true;
            }

            if (await Cast.CastSpell(SB.KillShot))
            {
                return true;
            }

            if (await Cast.CastSpell(SB.RapidFire))
            {
                return true;
            }

            if ((Unit.TargetHasAura(Me, SB.RapidFire) || MyCurrentTarget.HealthPercent <= 30) &&
                await Cast.CastSpell(SB.Stampede))
            {
                return true;
            }

            // actions+=/call_action_list,name=careful_aim,if=buff.careful_aim.up
            // actions+=/explosive_trap,if=spell_targets.explosive_trap_tick>1

            if (await Cast.CastSpell(SB.AMurderOfCrows))
            {
                return true;
            }

            if ((Conditions.Focus + 3 * Conditions.LuaGetRegen() < Me.GetMaxPower(WoWPowerType.Focus)) &&
                await Cast.CastSpell(SB.DireBeast))
            {
                return true;
            }

            if (await Cast.CastSpell(SB.GlaiveToss))
            {
                return true;
            }

            if (Conditions.FocusDeficit - (int)(2.25 * Conditions.LuaGetRegen()) > 0 &&
                await Cast.CastSpell(SB.Powershot))
            {
                return true;
            }

            if (await Cast.CastSpell(SB.Barrage))
            {
                return true;
            }

            // actions+=/steady_shot,if=focus.deficit*cast_time%(14+cast_regen)>cooldown.rapid_fire.remains
            // actions+=/focusing_shot,if=focus.deficit*cast_time%(50+cast_regen)>cooldown.rapid_fire.remains&focus<100

            // # Cast a second shot for steady focus if that won't cap us.
            // actions +=/ steady_shot,if= buff.pre_steady_focus.up & (14 + cast_regen + action.aimed_shot.cast_regen) <= focus.deficit
            // actions +=/ multishot,if= spell_targets.multi_shot > 6

            if (Me.GetLearnedTalent(6).Index == 1 && await Cast.CastSpell(SB.AimedShot))
            {
                return true;
            }

            if (Conditions.Focus + (int)(2.5 * Conditions.LuaGetRegen()) >= 85 && await Cast.CastSpell(SB.AimedShot))
            {
                return true;
            }

            if (Unit.TargetHasAura(Me, SB.ThrillOfTheHunt) &&
                Conditions.Focus + (int)(2.5 * Conditions.LuaGetRegen()) >= 65)
            {
                return true;
            }

            // # Allow FS to over-cap by 10 if we have nothing else to do
            if (Conditions.FocusDeficit - (50 + (int)(2.37 * Conditions.LuaGetRegen()) + 10) > 0 &&
                await Cast.CastSpell(SB.FocusingShot))
            {
                return true;
            }

            return await Cast.CastSpell(SB.SteadyShot);
        }

        private static async Task<bool> SurvivalRotation()
        {
            if (Managers.HotKeyManager.CooldownsOn)
            {
                if (await Cast.CastSpell(SB.AMurderOfCrows))
                {
                    return true;
                }

                if (await Cast.UseDpsTrinket(Me.Inventory.Equipped.Trinket1))
                {
                    return true;
                }
            }

            // stampede,if=buff.potion.up|(cooldown.potion.remains&(buff.archmages_greater_incandescence_agi.up|trinket.stat.any.up))|target.time_to_die<=45

            if (await Cast.CastSpell(SB.BlackArrow))
            {
                return true;
            }

            if ((Unit.TargetHasAura(Me, SB.SurgeOfConquest) && Unit.AuraTimeLeft(Me, SB.SurgeOfConquest) < 4000) ||
                Unit.AuraTimeLeft(MyCurrentTarget, SB.SerpentSting) <= 3000 && await Cast.CastSpell(SB.ArcaneShot))
            {
                return true;
            }

            if (await Cast.CastSpell(SB.ExplosiveShot))
            {
                return true;
            }

            if (Unit.TargetHasAura(Me, SB.SteadyFocus) && await Cast.CastSpell(SB.CobraShot))
            {
                return true;
            }

            if (Managers.HotKeyManager.CooldownsOn && await Cast.CastSpell(SB.DireBeast))
            {
                return true;
            }

            if (Unit.TargetHasAura(Me, SB.ThrillOfTheHunt) &&
                (Conditions.Focus > 35 || MyCurrentTarget.HealthPercent < 10) && await Cast.CastSpell(SB.ArcaneShot))
            {
                return true;
            }

            if (await Cast.CastSpell(SB.GlaiveToss))
            {
                return true;
            }

            if (await Cast.CastSpell(SB.Powershot))
            {
                return true;
            }

            if (await Cast.CastSpell(SB.Barrage))
            {
                return true;
            }

            //explosive_trap,if=!trinket.proc.any.react&!trinket.stacking_proc.any.react AND DOESN'T HAVE GLYPH OF EXPLOSIVE TRAP!!

            if (Me.GetLearnedTalent(3).Index == 0 && Me.GetLearnedTalent(6).Index != 1 &&
                Conditions.FocusDeficit < Conditions.LuaGetRegen() * 1.89 + 28)
            {
                return true;
            }

            if (Me.GetLearnedTalent(3).Index == 0 && Unit.AuraTimeLeft(Me, SB.SteadyFocus) < 5000 &&
                await Cast.CastSpell(SB.CobraShot))
            {
                return true;
            }

            if (Me.GetLearnedTalent(3).Index == 0 && Unit.AuraTimeLeft(Me, SB.SteadyFocusBuff) <= 2370 &&
                Conditions.FocusDeficit > ((int)2.370 * Conditions.LuaGetRegen()) &&
                await Cast.CastSpell(SB.FocusingShot))
            {
                return true;
            }

            if ((Conditions.Focus >= 70 || Me.GetLearnedTalent(6).Index == 1 ||
                 (Me.GetLearnedTalent(3).Index == 0 && Conditions.Focus >= 50) && await Cast.CastSpell(SB.ArcaneShot)))
            {
                return true;
            }

            if (await Cast.CastSpell(SB.FocusingShot))
            {
                return true;
            }

            return await Cast.CastSpell(SB.CobraShot);
        }
    }
}