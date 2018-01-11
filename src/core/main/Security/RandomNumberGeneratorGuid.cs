using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RapidCore.Security
{
    // taken from https://stackoverflow.com/questions/37170388/create-a-cryptographically-secure-random-guid-in-net
    public class RandomNumberGeneratorGuid
    {
        /// <summary>
        /// Returns a guid created from
        /// 16 random bytes using a cryptographically strong
        ///  sequence of random values
        /// </summary>
        /// <returns>
        /// the guid
        /// </returns>
        public Guid GenerateGuid()
        {
            using (var provider = RandomNumberGenerator.Create())
            {
                var bytes = new byte[16];
                provider.GetBytes(bytes);

                return new Guid(bytes);
            }
        }
    }
}
