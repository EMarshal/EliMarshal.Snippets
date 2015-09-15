// <copyright file="SQLLimitedRangeAttribute.cs" company="Éli Marshal">
//     Copyright (c) Éli Marshal. All rights reserved.
// </copyright>

namespace EliMarshal.ClassLibrary
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Validates to match SQL Server decimal data with given precision and scale
    /// </summary>
    public sealed class SQLLimitedRangeAttribute : ValidationAttribute
    {
        /// <summary>
        /// Equal to the scale for the SQL data type.
        /// </summary>
        private int scale;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SQLLimitedRangeAttribute" /> class.
        /// </summary>
        /// <param name="precision">The SQL precision to limit to.</param>
        /// <param name="scale">The SQL scale to limit to.</param>
        public SQLLimitedRangeAttribute(int precision, int scale)
        {
            Precision = precision;
            this.scale = scale;
        }

        /// <summary>
        /// Gets the precision for the SQL data type.
        /// </summary>
        public int Precision { get; private set; }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>true if the specified value is valid; otherwise, false.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string nines = new string('9', Precision);
            string decimalNines = nines.Insert(Precision - scale, ".");
            double minMaxValue = double.Parse(decimalNines);
            
            RangeAttribute range = new RangeAttribute(-minMaxValue, minMaxValue);

            if (range.IsValid(value))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(range.ErrorMessage);
            }
        }
    }
}
