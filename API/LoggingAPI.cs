using Aristois.Core;
using Aristois.Modules.Visual;
using System;
using System.Diagnostics;

namespace Aristois.API
{
    /// <summary>
    /// An exposed API Class to integrate usage the Aristois logging systems with other creator's mods.
    /// </summary>
    public static class LoggingAPI
    {
        /// <summary>
        /// Send a message to our console logger
        /// </summary>
        /// <param name="msg">the full content of your log message</param>
        public static void Log(string msg)
            => Logs.Log(msg);

        /// <summary>
        /// Send an error message to our console logger
        /// </summary>
        /// <param name="msg">the full content of your log message</param>
        public static void Error(string msg)
            => Logs.Error(msg);

        /// <summary>
        /// Send an error message to our console logger
        /// </summary>
        /// <param name="msg">the full content of your log message</param>
        /// <param name="st">the stack trace of your exception</param>
        public static void Error(string msg, StackTrace st)
            => Logs.Error(msg, st);

        /// <summary>
        /// Send an error message to our console logger
        /// </summary>
        /// <param name="msg">the full content of your log message</param>
        /// <param name="stackTrace">the stacktrace string of your exception or custom error message</param>
        public static void Error(string msg, string stackTrace)
            => Logs.Error(msg, stackTrace);

        /// <summary>
        /// Send an error message to our console logger
        /// </summary>
        /// <param name="exception">the exception itself</param>
        public static void Error(Exception exception)
            => Logs.Error(exception);

        /// <summary>
        /// Send a log message to our Debug Panel
        /// </summary>
        /// <param name="msg">the full content of your message</param>
        public static void Debugger(string msg)
            => DebugPanel.AddLog(msg);

        /// <summary>
        /// Send a log message to our Debug Panel Format: (HH:MM:SS) [Prefix] full message here.
        /// </summary>
        /// <param name="color">The hex string color of your prefix text</param>
        /// <param name="prefix">the text to show inside of [] brackets</param>
        /// <param name="msg">the full content of your message</param>
        public static void Debugger(string color, string prefix, string msg)
            => DebugPanel.AddLog(prefix, color, msg);
    }
}
