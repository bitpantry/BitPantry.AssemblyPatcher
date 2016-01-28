using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BitPantry.AssemblyPatcher
{
    /// <summary>
    /// Manages a collection of VersionPartGenerators where the string is the token and the IVersionPartGenerator is the generator
    /// implementation associated to that token
    /// </summary>
    public class VersionPartGeneratorCollection : IDictionary<string, IVersionPartGenerator>
    {
        readonly Dictionary<string, IVersionPartGenerator> _versionPartGeneratorDict = new Dictionary<string, IVersionPartGenerator>();

        #region IDictionary<> Implementation

        public IEnumerator<KeyValuePair<string, IVersionPartGenerator>> GetEnumerator()
        {
            return _versionPartGeneratorDict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _versionPartGeneratorDict).GetEnumerator();
        }

        public void Add(KeyValuePair<string, IVersionPartGenerator> item)
        {
            _versionPartGeneratorDict.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _versionPartGeneratorDict.Clear();
        }

        public bool Contains(KeyValuePair<string, IVersionPartGenerator> item)
        {
            return _versionPartGeneratorDict.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, IVersionPartGenerator>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, IVersionPartGenerator> item)
        {
            return _versionPartGeneratorDict.Remove(StandardizeToken(item.Key));
        }

        public int Count
        {
            get { return _versionPartGeneratorDict.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool ContainsKey(string token)
        {
            return _versionPartGeneratorDict.ContainsKey(StandardizeToken(token));
        }

        public void Add(string token, string generatorType)
        {
            try
            {
                var type = Type.GetType(generatorType);
                if (type == null)
                    throw new ArgumentException("The type could not be found");
                Add(token, type);
            }
            catch (Exception ex)
            {
                ThrowLoadException(token, generatorType, ex);
            }
        }

        /// <summary>
        /// Adds a new version part generator to the available generators collection
        /// </summary>
        /// <param name="token">The token the generator should generate for</param>
        /// <param name="generatorType">The generator type</param>
        public void Add(string token, Type generatorType)
        {
            try
            {
                // ensure parser has parameterless constructor

                if (generatorType.GetConstructor(Type.EmptyTypes) == null)
                    throw new VersionElementGeneratorLoadException(token, generatorType.FullName, string.Format("{0} type {1} does not have a parameterless constructor",
                        typeof(IVersionPartGenerator).Name, generatorType.FullName));

                // instantiate parser

                var generator = (IVersionPartGenerator)Activator.CreateInstance(generatorType);

                // add the generator

                Add(token, generator);
            }
            catch (Exception ex)
            {
                ThrowLoadException(token, generatorType.FullName, ex);
            }
        }

        public void Add(string token, IVersionPartGenerator generator)
        {
            try
            {
                token = StandardizeToken(token);

                // validate token

                if (!IsValidToken(token))
                    throw new VersionElementGeneratorLoadException(token, generator.GetType().FullName, string.Format("Token {0} is not valid - it must be alpha numeric.",
                        token));

                if (_versionPartGeneratorDict.ContainsKey(token))
                    throw new VersionElementGeneratorLoadException(token, generator.GetType().FullName, string.Format("Token {0} is already defined to use the {1} of type {2}",
                        token, typeof(IVersionPartGenerator), _versionPartGeneratorDict[token].GetType().FullName));

                // add parser to collection

                _versionPartGeneratorDict.Add(token, generator);

            }
            catch (Exception ex)
            {
                ThrowLoadException(token, generator.GetType().FullName, ex);
            }
        }   

        public bool Remove(string token)
        {
            return _versionPartGeneratorDict.Remove(StandardizeToken(token));
        }

        public bool TryGetValue(string token, out IVersionPartGenerator generator)
        {
            return _versionPartGeneratorDict.TryGetValue(StandardizeToken(token), out generator);
        }

        public IVersionPartGenerator this[string token]
        {
            get { return _versionPartGeneratorDict[StandardizeToken(token)]; }
            set { Add(token, value); }
        }

        public ICollection<string> Keys
        {
            get { return _versionPartGeneratorDict.Keys; }
        }

        public ICollection<IVersionPartGenerator> Values
        {
            get { return _versionPartGeneratorDict.Values; }
        }


        #endregion

        #region Helper Functions

        void ThrowLoadException(string token, string generatorType, Exception innerException)
        {
            throw new VersionElementGeneratorLoadException(
                token,
                generatorType,
                string.Format(
                    "The {0} type \"{1}\" could not be loaded for token {2}. See the inner exception for further details.",
                    typeof(IVersionPartGenerator).Name, generatorType, token), innerException);
        }

        /// <summary>
        /// Determines whether or not a token value is a valid token value
        /// </summary>
        /// <param name="token">The token to examine</param>
        /// <returns>Whether or not the token is valid</returns>
        /// <remarks>Token values must be a valid token character and can be any length</remarks>
        bool IsValidToken(string token)
        {
            return token.All(IsValidTokenCharacter);
        }

        /// <summary>
        /// Determines whether or not the given character is a valid token character
        /// </summary>
        /// <param name="chr">The character to evaluate</param>
        /// <returns>Whether or not the given character is a valid token value character</returns>
        bool IsValidTokenCharacter(char chr)
        {
            return char.IsLetterOrDigit(chr)
                   || new[] {'#', '@', '_', '-'}.Contains(chr);
        }

        /// <summary>
        /// Standardizes a token string to a common representation independent on how it was given to the funtion.
        /// </summary>
        /// <param name="token">The token value to standardize</param>
        /// <returns>The standardized token value</returns>
        string StandardizeToken(string token)
        {
            return token.Trim().Trim(new[] { '{', '}' }).ToLower();
        }

        #endregion

    }
}
