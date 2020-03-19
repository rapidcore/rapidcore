using System;
using System.IO;
using System.Linq;

namespace RapidCore.IO
{
    /// <summary>
    /// A thing that can work with mime-types (particularly
    /// figuring out what the mime-type of something is).
    /// </summary>
    public class MimeTyper
    {
        private readonly MimeTypeDefinitions _definitions;

        public MimeTyper()
        {
            _definitions = new MimeTypeDefinitions();
        }
        
        /// <summary>
        /// Get mime-type based on file contents.
        ///
        /// For binary signatures that are not unique, the
        /// filename (if given) will be used to guess which
        /// of the actual types it is.
        /// 
        /// </summary>
        public virtual string GetMimeType(byte[] bytes, string filename = null)
        {
            var extension = Path.GetExtension(filename);

            
            //
            // try the quick version where the extension of the file
            // actually corresponds to the content (this is likely to
            // be most cases)
            //
            if (!string.IsNullOrEmpty(extension))
            {
                var matchByExtension = _definitions.ByExtensionOrDefault(extension);

                if (matchByExtension != default && MatchesAnyOfThePrefixes(matchByExtension, bytes))
                {
                    return matchByExtension.MimeType;
                }
            }
            
            
            //
            // ok... we have to do a "full table scan"
            //
            foreach (var possibleMatch in _definitions.AllByFirstByte(bytes[0]))
            {
                if (MatchesAnyOfThePrefixes(possibleMatch, bytes))
                {
                    return possibleMatch.MimeType;
                }
            }

            return MimeTypeDefinitions.BinaryMimeType;
        }


        /// <summary>
        /// Does the given data match any of the prefixes
        /// in the given mime-type definition?
        /// </summary>
        private static bool MatchesAnyOfThePrefixes(MimeTypeDefinitions.MimeTypeDefinition definition, byte[] data)
        {
            return definition.BinaryPrefix.Any(prefix => DoesPrefixMatch(prefix, data));
        }

        /// <summary>
        /// Checks whether the given data matches the given prefix
        /// </summary>
        /// <param name="prefix">The binary prefix of a mime-type definition</param>
        /// <param name="data">The bytes we are testing</param>
        private static bool DoesPrefixMatch(byte?[] prefix, byte[] data)
        {
            for (var i = 0; i < prefix.Length - 1; i++)
            {
                if (prefix[i] != null && data[i] != prefix[i])
                {
                    return false;
                }
            }
            
            return true;
        }


        /// <summary>
        /// Get mime-type based on base64 encoded file contents.
        ///
        /// For binary signatures that are not unique, the
        /// filename (if given) will be used to guess which
        /// of the actual types it is.
        /// </summary>
        public virtual string GetMimeTypeFromBase64(string base64EncodedBytes, string filename = null)
        {
            /*
             * This is not pretty, but it seems to get the job done.
             * We could probably optimize this a bit (especially to get
             * rid of the base64 decode).
             */
            
            var decoded = Convert.FromBase64CharArray(
                base64EncodedBytes.ToCharArray(), 
                0, 
                Math.Min(16, base64EncodedBytes.Length)
            ).ToArray();

            return GetMimeType(decoded, filename);
        }
        
        /// <summary>
        /// "Guess" the mime-type based on a filename (i.e. extension).
        /// </summary>
        public virtual string GetMimeTypeFromFilename(string filename)
        {
            var ext = Path.GetExtension(filename);
            var mimeType = _definitions.ByExtensionOrDefault(ext);

            return mimeType?.MimeType ?? MimeTypeDefinitions.BinaryMimeType;
        }

        /// <summary>
        /// Check whether the given base64 encoded bytes
        /// have a mime-type that matches one of the
        /// allowed types
        /// </summary>
        /// <param name="base64EncodedBytes">The base64 encoded bytes</param>
        /// <param name="filename">The filename associated with the bytes (can be null)</param>
        /// <param name="allowedMimeTypes">The list of allowed mime-types</param>
        public virtual bool IsMimeTypeOneOfTheseFromBase64(string base64EncodedBytes, string filename, params string[] allowedMimeTypes)
        {
            var mimeType = GetMimeTypeFromBase64(base64EncodedBytes, filename);

            return allowedMimeTypes.Contains(mimeType);
        }
    }
}