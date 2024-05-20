using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace GarageMVC.Data
{
    /// <summary>
    /// Provides access to constants related to vehicles, such as colors and types, 
    /// loaded from the application configuration.
    /// </summary>
    public class VehicleConstants
    {
        
        private readonly Dictionary<string, string[]> _constants;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleConstants"/> class 
        /// with the provided application configuration.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <exception cref="InvalidOperationException">Thrown when configuration errors occur, such as missing or duplicate keys.</exception>
        public VehicleConstants(IConfiguration configuration)
        {
            _constants = new Dictionary<string, string[]>();

            // Retrieve the "VehicleData" section from the application configuration.
            var section = configuration.GetSection("VehicleData");

            // Check if the "VehicleData" section exists in the configuration.
            if (section is null)
            {
                throw new InvalidOperationException("Configuration error: Missing VehicleData.");
            }

            // Extract key-value pairs from child sections of "VehicleData" and store them in the dictionary.
            _constants = section.GetChildren()
                .ToDictionary(
                    child => child.Key,
                    child =>
                    {
                        var key = child.Key;

                        // Check for duplicate keys.
                        if (_constants.ContainsKey(key))
                        {
                            throw new InvalidOperationException($"Configuration error: Multiple keys `{key}`");
                        }

                        // Get the value associated with the key.
                        // Throws an exception if the value is not valid.
                        return child.Get<string[]>() ?? throw new InvalidOperationException($"Configuration error: Illegal json for key: {key}");
                    });

            // Check for mandatory keys.
            if (!_constants.ContainsKey("Colors"))
            {
                throw new InvalidOperationException("Configuration error: Missing Color.");
            }

            if (!_constants.ContainsKey("Types"))
            {
                throw new InvalidOperationException("Configuration error: Missing Type.");
            }
        }

        /// <summary>
        /// Gets the array of values associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the constant.</param>
        /// <returns>An array of values associated with the specified key, or null if the key does not exist.</returns>
        public string[]? this[string key]
        {
            get
            {
                if (!_constants.ContainsKey(key))
                {
                    return null;
                }

                return _constants[key];
            }
        }
    }
}
