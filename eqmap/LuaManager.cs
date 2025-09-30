using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NLua;
using NLog;

namespace eqmap
{
    /// <summary>
    /// Centralized Lua script management for EQMap
    /// Handles all Lua initialization, execution, and provides script functions
    /// </summary>
    public class LuaManager : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private Lua _lua;
        private bool _disposed = false;

        // Event handlers for Lua scripts
        public delegate void LogonResultEventHandler(bool success, string reason);
        public delegate void SpawnEventHandler(object mob);
        public delegate void MessageEventHandler(object message);

        private LogonResultEventHandler _logonResultHandler;
        private SpawnEventHandler _spawnEventHandler;
        private MessageEventHandler _messageEventHandler;

        public LuaManager()
        {
            InitializeLua();
        }

        private void InitializeLua()
        {
            try
            {
                _lua = new Lua();
                RegisterCoreFunctions();
                Logger.Info("Lua manager initialized successfully");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to initialize Lua manager");
                throw;
            }
        }

        private void RegisterCoreFunctions()
        {
            // Sleep functions
            _lua.RegisterFunction("sleep", this, typeof(LuaManager).GetMethod("Sleep"));
            _lua.RegisterFunction("sleepSeconds", this, typeof(LuaManager).GetMethod("SleepSeconds"));

            // Event handler registration functions
            _lua.RegisterFunction("SetLogonResultHandler", this, typeof(LuaManager).GetMethod("SetLogonResultHandler"));
            _lua.RegisterFunction("SetMessageEventHandler", this, typeof(LuaManager).GetMethod("SetMessageEventHandler"));
            _lua.RegisterFunction("SetSpawnEventHandler", this, typeof(LuaManager).GetMethod("SetSpawnEventHandler"));

            // Zone utility functions (now from GameClient namespace)
            _lua.RegisterFunction("ZoneNameToNumber", typeof(OpenEQ.Netcode.GameClient.ZoneUtils).GetMethod("ZoneNameToNumber"));
            _lua.RegisterFunction("ZoneNumberToName", typeof(OpenEQ.Netcode.GameClient.ZoneUtils).GetMethod("ZoneNumberToName"));
            _lua.RegisterFunction("IsValidZoneId", typeof(OpenEQ.Netcode.GameClient.ZoneUtils).GetMethod("IsValidZoneId"));
            _lua.RegisterFunction("IsValidZoneName", typeof(OpenEQ.Netcode.GameClient.ZoneUtils).GetMethod("IsValidZoneName"));

            Logger.Info("Core Lua functions registered");
        }

        #region Sleep Functions

        /// <summary>
        /// Sleep for specified milliseconds
        /// </summary>
        /// <param name="milliseconds">Number of milliseconds to sleep</param>
        public void Sleep(double milliseconds)
        {
            if (milliseconds <= 0)
            {
                Logger.Warn("Sleep called with non-positive milliseconds: {0}", milliseconds);
                return;
            }

            try
            {
                Logger.Debug("Sleeping for {0}ms", milliseconds);
                Thread.Sleep((int)Math.Round(milliseconds));
                Logger.Debug("Sleep completed");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error during sleep");
            }
        }

        /// <summary>
        /// Sleep for specified seconds (convenience method for Lua)
        /// </summary>
        /// <param name="seconds">Number of seconds to sleep</param>
        public void SleepSeconds(double seconds)
        {
            Sleep(seconds * 1000);
        }

        #endregion

        #region Object Registration

        /// <summary>
        /// Register an object to be available in Lua scripts
        /// </summary>
        /// <param name="name">Name to use in Lua</param>
        /// <param name="obj">Object to register</param>
        public void RegisterObject(string name, object obj)
        {
            try
            {
                _lua[name] = obj;
                Logger.Debug("Registered object '{0}' of type {1}", name, obj?.GetType().Name ?? "null");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to register object '{0}'", name);
            }
        }

        /// <summary>
        /// Register multiple objects at once
        /// </summary>
        public void RegisterObjects(params (string name, object obj)[] objects)
        {
            foreach (var (name, obj) in objects)
            {
                RegisterObject(name, obj);
            }
        }

        #endregion

        #region Event Handlers

        public void SetLogonResultHandler(LogonResultEventHandler eventHandler)
        {
            _logonResultHandler = eventHandler;
            Logger.Debug("Logon result handler registered");
        }

        public void SetSpawnEventHandler(SpawnEventHandler eventHandler)
        {
            _spawnEventHandler = eventHandler;
            Logger.Debug("Spawn event handler registered");
        }

        public void SetMessageEventHandler(MessageEventHandler eventHandler)
        {
            _messageEventHandler = eventHandler;
            Logger.Debug("Message event handler registered");
        }

        // Methods to call the registered handlers
        public void CallLogonResult(bool success, string reason)
        {
            try
            {
                _logonResultHandler?.Invoke(success, reason);
                Logger.Debug("Called logon result handler: success={0}, reason={1}", success, reason);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error calling logon result handler");
            }
        }

        public void CallSpawnEvent(object mob)
        {
            try
            {
                _spawnEventHandler?.Invoke(mob);
                Logger.Debug("Called spawn event handler");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error calling spawn event handler");
            }
        }

        public void CallMessageEvent(object message)
        {
            try
            {
                _messageEventHandler?.Invoke(message);
                Logger.Debug("Called message event handler");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error calling message event handler");
            }
        }

        #endregion

        #region Script Execution

        /// <summary>
        /// Execute a Lua script from file
        /// </summary>
        /// <param name="filePath">Path to the Lua script file</param>
        /// <returns>Result of the script execution</returns>
        public object ExecuteFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"Lua script file not found: {filePath}");
                }

                Logger.Info("Executing Lua script: {0}", filePath);
                var result = _lua.DoFile(filePath);
                Logger.Info("Lua script executed successfully");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error executing Lua script file: {0}", filePath);
                throw;
            }
        }

        /// <summary>
        /// Execute a Lua script from string
        /// </summary>
        /// <param name="script">Lua script code</param>
        /// <returns>Result of the script execution</returns>
        public object ExecuteString(string script)
        {
            try
            {
                Logger.Debug("Executing Lua script string");
                var result = _lua.DoString(script);
                Logger.Debug("Lua script string executed successfully");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error executing Lua script string");
                throw;
            }
        }

        /// <summary>
        /// Execute a Lua file and call its Main() function
        /// </summary>
        /// <param name="filePath">Path to the Lua script file</param>
        /// <returns>Result of the Main() function</returns>
        public object ExecuteMainFunction(string filePath)
        {
            try
            {
                ExecuteFile(filePath);
                Logger.Info("Calling Main() function from Lua script");
                var result = _lua.DoString("return Main()");
                Logger.Info("Main() function executed successfully");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error executing Main() function from: {0}", filePath);
                throw;
            }
        }

        #endregion

        #region Advanced Features

        /// <summary>
        /// Get a Lua global variable
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <returns>Variable value</returns>
        public object GetGlobal(string name)
        {
            try
            {
                return _lua[name];
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error getting Lua global variable: {0}", name);
                return null;
            }
        }

        /// <summary>
        /// Set a Lua global variable
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <param name="value">Variable value</param>
        public void SetGlobal(string name, object value)
        {
            try
            {
                _lua[name] = value;
                Logger.Debug("Set Lua global variable: {0}", name);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error setting Lua global variable: {0}", name);
            }
        }

        /// <summary>
        /// Check if a Lua function exists
        /// </summary>
        /// <param name="functionName">Function name to check</param>
        /// <returns>True if function exists</returns>
        public bool FunctionExists(string functionName)
        {
            try
            {
                var func = _lua[functionName];
                return func is LuaFunction;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Call a Lua function by name
        /// </summary>
        /// <param name="functionName">Function name</param>
        /// <param name="args">Function arguments</param>
        /// <returns>Function result</returns>
        public object CallFunction(string functionName, params object[] args)
        {
            try
            {
                var func = _lua[functionName] as LuaFunction;
                if (func == null)
                {
                    throw new InvalidOperationException($"Lua function '{functionName}' not found");
                }

                Logger.Debug("Calling Lua function: {0}", functionName);
                var result = func.Call(args);
                Logger.Debug("Lua function call completed: {0}", functionName);

                return result?.Length > 0 ? result[0] : null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error calling Lua function: {0}", functionName);
                throw;
            }
        }

        #endregion

        #region State Management

        /// <summary>
        /// Reset the Lua state (clears all variables and functions)
        /// </summary>
        public void Reset()
        {
            try
            {
                Logger.Info("Resetting Lua state");
                _lua?.Dispose();
                InitializeLua();
                Logger.Info("Lua state reset completed");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error resetting Lua state");
                throw;
            }
        }

        /// <summary>
        /// Get current Lua memory usage
        /// </summary>
        /// <returns>Memory usage in KB</returns>
        public int GetMemoryUsage()
        {
            try
            {
                // This requires calling Lua's collectgarbage function
                var result = _lua.DoString("return collectgarbage('count')");
                if (result?.Length > 0 && result[0] is double memKB)
                {
                    return (int)memKB;
                }
                return -1;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error getting Lua memory usage");
                return -1;
            }
        }

        /// <summary>
        /// Force garbage collection in Lua
        /// </summary>
        public void CollectGarbage()
        {
            try
            {
                _lua.DoString("collectgarbage('collect')");
                Logger.Debug("Lua garbage collection performed");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error performing Lua garbage collection");
            }
        }

        #endregion

        #region Dispose Pattern

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    try
                    {
                        _lua?.Dispose();
                        Logger.Info("Lua manager disposed");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, "Error disposing Lua manager");
                    }
                }

                _disposed = true;
            }
        }

        ~LuaManager()
        {
            Dispose(false);
        }

        #endregion
    }
}