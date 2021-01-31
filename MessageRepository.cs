using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_csharp_webapi
{
    public class MessageRepository : IMessageRepository
    {
        IDictionary<string, string> _dictionary = new ConcurrentDictionary<string, string>();

        public string GetMessage()
        {
            if (_dictionary.ContainsKey("motd"))
            {
                return _dictionary["motd"];
            }

            return "No motd entered.";
        }

        public string SetMessage(string input)
        {
            _dictionary["motd"] = input;
            return _dictionary["motd"];
        }
    }
}
