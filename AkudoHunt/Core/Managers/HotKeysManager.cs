using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using SB = AkudoHunt.Core.Helpers.SpellBook;

namespace AkudoHunt.Core.Managers
{
    internal class HotKeyManager
    {
        public static bool KeysRegistered { get; set; }

        public static bool CooldownsOn { get; set; }
        public static bool ForceAttack { get; set; }

        public static bool FreezingTrapFocus { get; set; }
        public static bool FreezingTrapFocusNoLauncher { get; set; }
        public static bool FreezingTrapMouseoverUnit { get; set; }

        public static bool SnakeOrIceTrapCurrentTarget { get; set; }
        public static bool SnakeOrIceTrapFocus { get; set; }
        public static bool SnakeOrIceTrapunderMeNoLauncher { get; set; }

        public static bool ExplosiveTrapCurrentTarget { get; set; }
        public static bool ExplosiveTrapFocus { get; set; }
        public static bool ExplosiveTrapUnderMeNoLauncher { get; set; }
        public static bool ExplosiveTrapMouseoverLocation { get; set; }

        #region Hotkey Registration

        public static void RegisterHotKeys()
        {
            if (KeysRegistered)
                return;
            CooldownsOn = false;
            ForceAttack = false;

            FreezingTrapFocus = false;
            FreezingTrapFocusNoLauncher = false;
            FreezingTrapMouseoverUnit = false;

            SnakeOrIceTrapCurrentTarget = false;
            SnakeOrIceTrapFocus = false;
            SnakeOrIceTrapunderMeNoLauncher = false;

            ExplosiveTrapCurrentTarget = false;
            ExplosiveTrapFocus = false;
            ExplosiveTrapUnderMeNoLauncher = false;
            ExplosiveTrapMouseoverLocation = false;

            #region Freezing Trap

            HotkeysManager.Register("FreezingTrapFocus", Keys.D4, ModifierKeys.NoRepeat, ret =>
            {
                FreezingTrapFocus = true;

                StyxWoW.Overlay.AddToast(() =>
                    ("Freezing Trap -> Focus"), TimeSpan.FromSeconds(2),
                    Colors.LightSeaGreen,
                    Colors.DarkSlateGray,
                    new FontFamily("Consolas"),
                    FontWeights.Bold,
                    28);
            });

            HotkeysManager.Register("FreezingTrapFocusNoLauncher", Keys.D4, ModifierKeys.Shift, ret =>
            {
                FreezingTrapFocusNoLauncher = !FreezingTrapFocusNoLauncher;
                StyxWoW.Overlay.AddToast(() =>
                    (FreezingTrapFocusNoLauncher ? "Trap Focus Activated!" : "Trap Focus Disabled!"),
                    TimeSpan.FromSeconds(2),
                    Colors.LightSeaGreen,
                    Colors.DarkSlateGray,
                    new FontFamily("Consolas"),
                    FontWeights.Bold,
                    28);
            });

            #endregion

            #region Snake or Ice Trap

            HotkeysManager.Register("SnakeOrIceTrapCurrentTarget", Keys.Q, ModifierKeys.NoRepeat, ret =>
            {
                SnakeOrIceTrapCurrentTarget = true;

                StyxWoW.Overlay.AddToast(() =>
                    ("Snake/Ice Trap -> Current Target"), TimeSpan.FromSeconds(2),
                    Colors.LightSeaGreen,
                    Colors.DarkSlateGray,
                    new FontFamily("Consolas"),
                    FontWeights.Bold,
                    28);
            });

            HotkeysManager.Register("SnakeOrIceTrapFocus", Keys.Q, ModifierKeys.Shift, ret =>
            {

                SnakeOrIceTrapFocus = true;

                StyxWoW.Overlay.AddToast(() =>
                    ("Snake/Ice Trap -> Focus"), TimeSpan.FromSeconds(2),
                    Colors.LightSeaGreen,
                    Colors.DarkSlateGray,
                    new FontFamily("Consolas"),
                    FontWeights.Bold,
                    28);
            });

            HotkeysManager.Register("SnakeOrIceTrapUnderMe", Keys.Q, ModifierKeys.Control, ret =>
            {
                SnakeOrIceTrapunderMeNoLauncher = true;

                StyxWoW.Overlay.AddToast(() =>
                    ("Snake/Ice Trap -> Under Me."), TimeSpan.FromSeconds(2),
                    Colors.LightSeaGreen,
                    Colors.DarkSlateGray,
                    new FontFamily("Consolas"),
                    FontWeights.Bold,
                    28);
            });

            #endregion

            #region Explosive Trap
            HotkeysManager.Register("ExplosiveTrapCurrentTarget", Keys.D5, ModifierKeys.NoRepeat, ret =>
            {
                ExplosiveTrapCurrentTarget = true;

                StyxWoW.Overlay.AddToast(() =>
                    ("Explosive Trap -> Current Target"), TimeSpan.FromSeconds(2),
                    Colors.LightSeaGreen,
                    Colors.DarkSlateGray,
                    new FontFamily("Consolas"),
                    FontWeights.Bold,
                    28);
            });

            HotkeysManager.Register("ExplosiveTrapFocus", Keys.D5, ModifierKeys.Shift, ret =>
            {
                ExplosiveTrapFocus = true;

                StyxWoW.Overlay.AddToast(() =>
                    ("Explosive Trap -> Focus Target"), TimeSpan.FromSeconds(2),
                    Colors.LightSeaGreen,
                    Colors.DarkSlateGray,
                    new FontFamily("Consolas"),
                    FontWeights.Bold,
                    28);
            });

            HotkeysManager.Register("ExplosiveTrapUnderMeNoLauncher", Keys.D5, ModifierKeys.Control, ret =>
            {
                ExplosiveTrapUnderMeNoLauncher = true;

                StyxWoW.Overlay.AddToast(() =>
                    ("Explosive Trap -> Under Me"), TimeSpan.FromSeconds(2),
                    Colors.LightSeaGreen,
                    Colors.DarkSlateGray,
                    new FontFamily("Consolas"),
                    FontWeights.Bold,
                    28);
            });
            #endregion


            HotkeysManager.Register("CooldownsOn", Keys.D1, ModifierKeys.NoRepeat, ret =>
            {
                CooldownsOn = !CooldownsOn;
                StyxWoW.Overlay.AddToast(() =>
                    (CooldownsOn ? "Cooldowns Activated!" : "Cooldowns Disabled!"), TimeSpan.FromSeconds(2),
                    Colors.LightSeaGreen,
                    Colors.DarkSlateGray,
                    new FontFamily("Consolas"),
                    FontWeights.Bold,
                    28);
            });


            HotkeysManager.Register("Manual Pause", Keys.P, ModifierKeys.Control,
                ret => { StyxWoW.Overlay.AddToast(("Manual Pause for 2 seconds"), 2000); });


            //At some fights Meelerange won't cut it (for Example Gorefiend, so just force the bot to attack)
            HotkeysManager.Register("Force Attack", Keys.L, ModifierKeys.Alt, ret =>
            {
                StyxWoW.Overlay.AddToast(("Force Attackmode , to disable press alt + L again"), 2000);
                ForceAttack = !ForceAttack;
            });
        }

        #endregion

        #region [Method] - Hotkey Removal

        public static void RemoveHotkeys()
        {
            if (!KeysRegistered)
                return;
            HotkeysManager.Unregister("CooldownsOn");
            CooldownsOn = false;
            KeysRegistered = false;
            //Lua.DoString(@"print('Hotkeys: \124cFFE61515 Removed!')");
            Logging.Write(Colors.OrangeRed, "Hotkeys: Removed!");
        }

        #endregion
    }
}