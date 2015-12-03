using System;
using System.Windows.Media;
using Styx.Common;

namespace AkudoHunt.Core.Utilities
{
    class Log
    {
        #region [Method] - Combat Log
        public static string lastCombatMSG;
        public static void combatLog(string Message, params object[] args)
        {
            if (Message == lastCombatMSG)
                return;
            Logging.Write(Colors.DarkOrange, "[AkudoHunt] {0}", String.Format(Message, args));
            lastCombatMSG = Message;
        }
        #endregion

        #region [Method] - Diagnostics Log
        public static void diagnosticLog(string Message, params object[] args)
        {
            if (Message == null)
                return;
            Logging.WriteDiagnostic(Colors.Firebrick, "[Error] {0}", String.Format(Message, args));
        }
        #endregion
    }
}