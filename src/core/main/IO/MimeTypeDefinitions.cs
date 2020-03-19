using System.Collections.Generic;
using System.Linq;

namespace RapidCore.IO
{
    /*
     * Note that these definitions are by no means complete
     * or optimized.
     *
     * We could consider using a code-generator (run by a developer) in
     * order to generate super fast static code.
     *
     * List of mime types: https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Complete_list_of_MIME_types
     * Signatures: https://en.wikipedia.org/wiki/List_of_file_signatures
     */
    
    /// <summary>
    /// Definitions of MimeTypes that powers the <see cref="MimeTyper"/>
    /// </summary>
    public class MimeTypeDefinitions
    {
        public const string BinaryMimeType = "application/octet-stream";

        public class MimeTypeDefinition
        {
            public string MimeType { get; set; }
            
            /// <summary>
            /// A list of known binary prefixes for
            /// this particular mime-type.
            ///
            /// In some cases, a prefix can have wildcards in them,
            /// which is done here by setting that byte to null 
            /// </summary>
            public List<byte?[]> BinaryPrefix { get; set; }
            public string Extension { get; set; }
        }

        private Dictionary<string, MimeTypeDefinition> Definitions => new Dictionary<string, MimeTypeDefinition>
        {
            #region definitions
            { ".doc",
                new MimeTypeDefinition {
                    MimeType = "application/msword",
                    BinaryPrefix = new List<byte?[]>
                    {
                        new byte?[] { 0xd0, 0xcf, 0x11, 0xe0, 0xa1, 0xb1, 0x1a, 0xe1 }
                    },
                    Extension = ".doc"
                }
            },
            { ".docx",
                new MimeTypeDefinition
                {
                    MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    BinaryPrefix = new List<byte?[]> {new byte?[] { 0x50, 0x4b, 0x03, 0x04 }},
                    Extension = ".docx",
                }
            },
            { ".xlsx",
                new MimeTypeDefinition
                {
                    MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    BinaryPrefix = new List<byte?[]> {new byte?[] { 0x50, 0x4b, 0x03, 0x04 }},
                    Extension = ".xlsx",
                }
            },
            { ".gif",
                new MimeTypeDefinition
                {
                    MimeType = "image/gif",
                    BinaryPrefix = new List<byte?[]>
                    {
                        new byte?[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 },
                        new byte?[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }
                    },
                    Extension = ".gif"
                }
            },
            { ".jpg",
                new MimeTypeDefinition
                {
                    MimeType = "image/jpeg",
                    BinaryPrefix = new List<byte?[]>
                    {
                        new byte?[] { 0xff, 0xd8, 0xff, 0xe1, null, null, 0x45, 0x78, 0x69, 0x66, 0x00, 0x00 }, // jpg with exif
                        new byte?[] { 0xff, 0xd8, 0xff, 0xe0, 0x00, 0x10, 0x4a, 0x46, 0x49, 0x46, 0x00, 0x01 }, // jpg with jfif
                        new byte?[] { 0xff, 0xd8, 0xff, 0xdb },
                        new byte?[] { 0xff, 0xd8, 0xff, 0xee }
                    },
                Extension = ".jpg"
                }
            },
            { ".odt",
                new MimeTypeDefinition
                {
                    MimeType = "application/vnd.oasis.opendocument.text",
                    BinaryPrefix = new List<byte?[]> { new byte?[] { 0x50, 0x4b, 0x03, 0x04 } },
                    Extension = ".odt",
                }
            },
            { ".pdf",
                new MimeTypeDefinition
                {
                    MimeType = "application/pdf",
                    BinaryPrefix = new List<byte?[]> { new byte?[]  { 0x25, 0x50, 0x44, 0x46, 0x2d } },
                    Extension = ".pdf"
                }
            },
            { ".png",
                new MimeTypeDefinition
                {
                    MimeType = "image/png",
                    BinaryPrefix = new List<byte?[]> { new byte?[]  { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a }},
                    Extension = ".png"
                }
            },
            { ".heic",
                new MimeTypeDefinition
                {
                    MimeType = "image/heic",
                    BinaryPrefix = new List<byte?[]> { new byte?[]  { 0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70 } },
                    Extension = ".heic"
                }
            },
            { ".bin",
                new MimeTypeDefinition
                {
                    MimeType = "application/octet-stream",
                    BinaryPrefix = new List<byte?[]> { new byte?[]  { 0xfb, 0xff, 0xfa, 0x8e, 0xd9 } },
                    Extension = ".bin"
                }
            },
            { ".zip",
                new MimeTypeDefinition
                {
                    MimeType = "application/zip",
                    BinaryPrefix = new List<byte?[]>
                    {
                        new byte?[] { 0x50, 0x4b, 0x05, 0x06 }, // empty archive
                        new byte?[] { 0x50, 0x4b, 0x03, 0x04 }, // non-empty archive
                        new byte?[] { 0x50, 0x4b, 0x07, 0x08 }, // spanned archive
                    },
                    Extension = ".zip",
                }
            }
            #endregion
        };

        /// <summary>
        /// Get all mime-type definitions that have a prefix where the first
        /// byte matches the given byte
        /// </summary>
        /// <returns>An empty list or a list with matching mime-type definitions</returns>
        public virtual IReadOnlyList<MimeTypeDefinition> AllByFirstByte(byte firstByte)
        {
            return Definitions
                .Where(
                    def => def.Value.BinaryPrefix.Any(
                        prefix => prefix[0] == firstByte
                    )
                ).Select(x => x.Value)
                .ToList()
                .AsReadOnly();
        }
        
        
        /// <summary>
        /// Get the mime-type definition with the matching filename extension
        /// </summary>
        /// <returns>The match or default</returns>
        public virtual MimeTypeDefinition ByExtensionOrDefault(string extension)
        {
            // we have no extension or no matches
            if (string.IsNullOrWhiteSpace(extension) || !Definitions.ContainsKey(extension))
            {
                return default;
            }

            return Definitions[extension];
        }
    }
}