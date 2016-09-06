namespace Shojy.Common.DataTypes
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a UK Postcode, and provides basic methods and operators.
    /// </summary>
    public struct Postcode
    {
        #region Private Constructors

        /// <summary>
        /// Initializes a new instance of a Postcode value, based on a string.
        /// </summary>
        /// <param name="postcode"></param>
        /// <exception cref="InvalidOperationException">The string postcode is not a valid postcode.</exception>
        private Postcode(string postcode)
        {
            postcode = postcode.ToUpper();
            var regex = new Regex(
                "^(GIR 0AA)|(?<first>(([A-Z-[QVX]][0-9][0-9]?)|(([A-Z-[QVX]][A-Z-[IJZ]][0-9][0-9]?)|(([A-Z-[QVX]][0-9][A-HJKPSTUW])|([A-Z-[QVX]][A-Z-[IJZ]][0-9][ABEHMNPRVWXY])))) ?(?<second>[0-9][A-Z-[CIKMOV]]{2}))$");

            if (!regex.IsMatch(postcode))
            {
                throw new InvalidOperationException("Not a valid postcode");
            }

            var groups = regex.Match(postcode).Groups;

            if (postcode.Length != 7)
            {
                var first = groups["first"].Value;
                var second = groups["second"].Value;

                var spaces = 7 - first.Length - second.Length;

                var gap = string.Empty;

                for (var i = 0; i < spaces; i++)
                {
                    gap += " ";
                }
                postcode = $"{first}{gap}{second}";
            }

            this.Value = postcode;
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// Gets the string value of the postcode. This will be in standard 7-character format in uppercase.
        /// </summary>
        public string Value { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Implicitly converts a string value into a Postcode object.
        /// </summary>
        /// <param name="postcode">string value to convert.</param>
        public static implicit operator Postcode(string postcode)
        {
            return new Postcode(postcode);
        }

        /// <summary>
        /// Implicitly converts a Postcode object to a string value.
        /// </summary>
        /// <param name="postcode">Postcode object to convert.</param>
        public static implicit operator string(Postcode postcode)
        {
            return postcode.Value;
        }

        /// <summary>
        /// Converts the string representation into its Postcode equivalent.
        /// </summary>
        /// <param name="postcode"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">The string postcode is not a valid postcode.</exception>
        public static Postcode Parse(string postcode)
        {
            return new Postcode(postcode);
        }

        /// <summary>
        /// Checks to see if parsing the string to a postcode is possible, and if so, sets the new Postcode value to the postcode
        /// parameter and returns true. If it's not possible to parse the string, the method returns false and postcode parameter
        /// is set to null. No exception is thrown from this method.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="postcode"></param>
        /// <returns></returns>
        public static bool TryParse(string value, out Postcode postcode)
        {
            try
            {
                postcode = new Postcode(value);
                return true;
            }
            catch (InvalidOperationException)
            {
                postcode = null;
                return false;
            }
        }

        /// <summary>
        /// Checks to see if this Postcode matches the value of another postcode or string. Any other datatype returns false.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Postcode)
            {
                return this.Value == ((Postcode)obj).Value;
            }
            var s = obj as string;

            if (null != s)
            {
                return this.Value == s;
            }

            return false;
        }

        /// <summary>
        /// Returns the hashcode for the postcode.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Returns the string representation of a Postcode. This will be in uppercase, 7-character format.
        /// </summary>
        /// <returns>String value.</returns>
        public override string ToString()
        {
            return this.Value;
        }

        #endregion Public Methods
    }
}