﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS_TextParser
{
    public class TLSDictionary
    {
        private Dictionary<string, int> dictionary;

        public TLSDictionary()
        {
            dictionary = new Dictionary<string, int>();
        }

        public void IncrementTLS(string tls)
        {
            tls = tls.ToLower();

            if(dictionary.ContainsKey(tls))
            {
                int newCount = dictionary[tls] + 1;
                dictionary.Remove(tls);
                dictionary.Add(tls, newCount);
            }
            else
            {
                dictionary.Add(tls, 1);
            }
        }

        public int GetCount(string tls)
        {
            try
            {
                int count = dictionary[tls];
                return count;
            }
            catch(KeyNotFoundException)
            {
                return 0;
            }
        }
    }
}