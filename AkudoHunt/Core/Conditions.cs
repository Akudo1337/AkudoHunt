using System;
using AkudoHunt.Core.Utilities;
using Styx;
using Styx.WoWInternals;

namespace AkudoHunt
{
    public class Conditions
    {
        public static uint Focus => StyxWoW.Me.GetPowerInfo(WoWPowerType.Focus).Current;
        public static uint FocusDeficit => StyxWoW.Me.MaxFocus - StyxWoW.Me.GetPowerInfo(WoWPowerType.Focus).Current;

        public static float LuaGetRegen()
        {
            try
            {
                using (StyxWoW.Memory.AcquireFrame()) { return Lua.GetReturnVal<float>("return GetPowerRegen();", 1); }
            }
            catch (Exception xException)
            {
                Log.diagnosticLog("Can't get regen: ", xException);
                return 4;
            }
        }
    }
}