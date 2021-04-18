using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Core
{
    public class StepsContext
    {
        private static ThreadLocal<StepsContext> _localContext = new ThreadLocal<StepsContext>();
        private readonly Dictionary<string, object> _context = new Dictionary<string, object>();

        public static StepsContext Current => _localContext.Value ?? throw new InvalidOperationException("StepsContext hasn't been initialized yet");
        public Logger Log { get; } = Logger.Instance;

        public StepsContext()
        {
            Log.Info("Feature Context is initialized");
            _localContext.Value = this;
        }

        /// <summary>
        /// Adds the context variable.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="value">The value.</param>
        public void Set<T>(string variable, T value, bool toLog = false)
        {
            _context[variable] = value;
            if (toLog)
            {
                Log.Info($"Variable '{variable}' is set with the value {value}");
                if (value != null && !value.IsSimpleType())
                {
                    value.GetPropertiesToLog();
                }
            }
        }

        /// <summary>
        /// Gets the context variable.
        /// </summary>
        /// <param name="variable">The variable.</param>
        public T Get<T>(string variable, bool toLog = false)
        {
            try
            {
                T value = (T)_context[variable];

                if (toLog)
                {
                    Log.Info($"Variable '{variable}' is found in Context");
                    value?.GetPropertiesToLog();
                }

                return value;
            }
            catch (KeyNotFoundException)
            {
                Log.Warn($"The value of '{variable}' doesn't present in Context.");
                throw;
            }
        }

        /// <summary>
        /// Deletes the context variable.
        /// </summary>
        /// <param name="variable">The variable.</param>
        public void Delete(string variable)
        {
            try
            {
                object value = _context[variable];
                _context.Remove(variable);
            }
            catch (KeyNotFoundException)
            {
            }
        }

        /// <summary>
        /// Clear the context.
        /// </summary>
        public void Clear()
        {
            _context.Clear();
        }
    }
}