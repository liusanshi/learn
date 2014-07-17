//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================


namespace LL.FrameWork.Core.Infrastructure.Logging
{
    /// <summary>
    /// Log Factory
    /// </summary>
    public static class LoggerFactory
    {
        #region Members

        static ILoggerFactory _currentLogFactory = null;

        static readonly NOLogger nologger = new NOLogger();
        #endregion

        #region Public Methods

        /// <summary>
        /// Set the  log factory to use
        /// </summary>
        /// <param name="logFactory">Log factory to use</param>
        public static void SetCurrent(ILoggerFactory logFactory)
        {
            _currentLogFactory = logFactory;
        }

        /// <summary>
        /// Createt a new <paramref name="LL.FrameWork.Core.Infrastructure.Logging.ILog"/>
        /// </summary>
        /// <returns>Created ILog</returns>
        public static ILogger CreateLog()
        {
            return (_currentLogFactory != null) ? _currentLogFactory.Create() : LoggerFactory.nologger;
        }

        #endregion

        /// <summary>
        /// 不记录的记录者
        /// </summary>
        class NOLogger : ILogger
        {

            public void Debug(string message, params object[] args)
            {

            }

            public void Debug(string message, System.Exception exception, params object[] args)
            {

            }

            public void Debug(object item)
            {

            }

            public void Fatal(string message, params object[] args)
            {

            }

            public void Fatal(string message, System.Exception exception, params object[] args)
            {

            }

            public void LogInfo(string message, params object[] args)
            {

            }

            public void LogWarning(string message, params object[] args)
            {

            }

            public void LogError(string message, params object[] args)
            {

            }

            public void LogError(string message, System.Exception exception, params object[] args)
            {

            }
        }
    }
}
