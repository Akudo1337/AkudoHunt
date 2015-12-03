using System.Threading.Tasks;
using Styx;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals.WoWObjects;

namespace AkudoHunt.Core.Routines
{
    public class Pull
    {
        private static LocalPlayer Me => StyxWoW.Me;
        private static WoWUnit MyCurrentTarget => Me.CurrentTarget;

        public static async Task<bool> Rotation()
        {
            if (Me.Mounted)
            {
                if (await CommonCoroutines.Dismount())
                {
                    return true;
                }
            }

            return false;
        }
    }
}