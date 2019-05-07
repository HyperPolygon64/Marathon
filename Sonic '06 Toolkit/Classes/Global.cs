﻿using System;

namespace Sonic_06_Toolkit
{
    public class Global
    {
        #region Strings
        public static string versionNumber = "1.81";
        public static string latestVersion = "Version " + versionNumber;
        public static string serverStatus;
        public static string currentPath;
        public static string updateState;
        public static string arcState;
        public static string adxState;
        public static string csbState;
        public static string lubState;
        public static string setState;
        public static string mstState;
        public static string xnoState;

        #region Directories
        public static string applicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        #endregion

        #endregion

        #region Integers
        public static int pathChange = 0;
        public static int sessionID;
        public static int getIndex;

        #region Boolean
        public static bool javaCheck = true;
        #endregion

        #endregion
    }
}
