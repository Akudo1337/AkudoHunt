using System.Windows.Media;
using AkudoHunt.Core.Managers;
using CommonBehaviors.Actions;
using Styx;
using Styx.Common;
using Styx.CommonBot.Routines;
using Styx.WoWInternals.WoWObjects;
using R = AkudoHunt.Core.Routines;
using Styx.TreeSharp;

namespace AkudoHunt
{
    public class AkudoHunt : CombatRoutine
    {
        public override string Name => "AkudoHunt";
        public override WoWClass Class => WoWClass.Hunter;
        public static LocalPlayer Me => StyxWoW.Me;
        public static WoWUnit MyCurrentTarget => Me.CurrentTarget;
        public override bool WantButton => true;

        //public Composite PreCombatBuffBehaviour { get { return new ActionRunCoroutine(ctx => R.PreCombat.Rotation());} }
        public override Composite PullBehavior { get { return new ActionRunCoroutine(ctx => R.Pull.Rotation()); } }
        public override Composite CombatBehavior { get { return new ActionRunCoroutine(ctx => R.Combat.Rotation()); } }
        //public override Composite HealBehavior { get { return new ActionRunCoroutine(ctx => R.Heal.Rotation()); } }
        public override Composite CombatBuffBehavior { get { return new ActionRunCoroutine(ctx => R.CombatBuff.Rotation()); } }

        public override void Initialize()
        {
            HotKeyManager.RegisterHotKeys();
        }

        public override void ShutDown()
        {
            HotKeyManager.RemoveHotkeys();
        }

        public override void OnButtonPress()
        {
            Logging.Write(Colors.Aqua, "Class Configuration Requested");
        }
    }
}
