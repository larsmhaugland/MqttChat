using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MQTT_Intek
{
    /// <summary>
    /// Simple MQTT topic matcher that supports single-level and multi-level wildcards.
    /// </summary>
    public static class MqttTopicMatcher
    {
        /// <summary>
        /// Simple MQTT topic matcher that supports single-level and multi-level wildcards.
        /// </summary>
        /// <param name="topicA">Topic that includes wildcards</param>
        /// <param name="topicB">Topic that is to be checked against Topic A</param>
        /// <returns>True if Topic B is within Topic A</returns>
        public static bool IsMatch(string topicA, string topicB)
        {
            if (topicA == "#")
                return true; // "#" matches any topic

            string[] topicAParts = topicA.Split('/');
            string[] topicBParts = topicB.Split('/');

            for (int i = 0; i < topicAParts.Length; i++)
            {
                if (i >= topicBParts.Length)
                    return false; // Topic A is longer than Topic B

                if (topicAParts[i] == "#")
                    return true; // "#" wildcard matches any remaining part of the topic

                if (topicAParts[i] != topicBParts[i] && topicAParts[i] != "+")
                    return false; // Exact match or "+" wildcard for single level
            }
            return topicAParts.Length == topicBParts.Length;
        }
    }
}