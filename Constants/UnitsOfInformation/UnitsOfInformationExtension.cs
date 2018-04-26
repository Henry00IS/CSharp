using System.Numerics;

// requires adding a reference to System.Numerics (even if it may appear like it's finding it in the using above).

namespace OOLaboratories.Constants.UnitsOfInformation
{
    /// <summary>
    /// Provides extension methods to transform a <see cref="long"/> (amount of bytes) into a unit of
    /// information string like "23 GiB".
    /// <para>Ansprechpartner: Henry de Jongh</para>
    /// </summary>
    public static class UnitsOfInformationExtension
    {
        // -------------------------------------------------------------------------------------------------
        // PUBLIC METHODS : PART 1
        // -------------------------------------------------------------------------------------------------

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="to">The unit of information to convert to.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this byte value, UnitsOfInformation from, UnitsOfInformation to)
            => ConvertToUnitOfInformation(value, from, to);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="to">The unit of information to convert to.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this char value, UnitsOfInformation from, UnitsOfInformation to)
            => ConvertToUnitOfInformation(value, from, to);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="to">The unit of information to convert to.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this decimal value, UnitsOfInformation from, UnitsOfInformation to)
            => ConvertToUnitOfInformation((BigInteger)value, from, to);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="to">The unit of information to convert to.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this double value, UnitsOfInformation from, UnitsOfInformation to)
            => ConvertToUnitOfInformation((BigInteger)value, from, to);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="to">The unit of information to convert to.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this float value, UnitsOfInformation from, UnitsOfInformation to)
            => ConvertToUnitOfInformation((BigInteger)value, from, to);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="to">The unit of information to convert to.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this int value, UnitsOfInformation from, UnitsOfInformation to)
            => ConvertToUnitOfInformation(value, from, to);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="to">The unit of information to convert to.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this long value, UnitsOfInformation from, UnitsOfInformation to)
            => ConvertToUnitOfInformation(value, from, to);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="to">The unit of information to convert to.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this sbyte value, UnitsOfInformation from, UnitsOfInformation to)
            => ConvertToUnitOfInformation(value, from, to);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="to">The unit of information to convert to.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this short value, UnitsOfInformation from, UnitsOfInformation to)
            => ConvertToUnitOfInformation(value, from, to);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="to">The unit of information to convert to.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this uint value, UnitsOfInformation from, UnitsOfInformation to)
            => ConvertToUnitOfInformation(value, from, to);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="to">The unit of information to convert to.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this ulong value, UnitsOfInformation from, UnitsOfInformation to)
            => ConvertToUnitOfInformation(value, from, to);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="to">The unit of information to convert to.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this ushort value, UnitsOfInformation from, UnitsOfInformation to)
            => ConvertToUnitOfInformation(value, from, to);

        // -------------------------------------------------------------------------------------------------
        // PUBLIC METHODS : PART 2
        // -------------------------------------------------------------------------------------------------

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="radix">The desired radix to use.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this byte value, UnitsOfInformation from, UnitsOfInformationRadix radix)
            => ConvertToUnitOfInformation(value, from, radix);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="radix">The desired radix to use.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this char value, UnitsOfInformation from, UnitsOfInformationRadix radix)
            => ConvertToUnitOfInformation(value, from, radix);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="radix">The desired radix to use.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this decimal value, UnitsOfInformation from, UnitsOfInformationRadix radix)
            => ConvertToUnitOfInformation((BigInteger)value, from, radix);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="radix">The desired radix to use.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this double value, UnitsOfInformation from, UnitsOfInformationRadix radix)
            => ConvertToUnitOfInformation((BigInteger)value, from, radix);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="radix">The desired radix to use.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this float value, UnitsOfInformation from, UnitsOfInformationRadix radix)
            => ConvertToUnitOfInformation((BigInteger)value, from, radix);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="radix">The desired radix to use.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this int value, UnitsOfInformation from, UnitsOfInformationRadix radix)
            => ConvertToUnitOfInformation(value, from, radix);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="radix">The desired radix to use.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this long value, UnitsOfInformation from, UnitsOfInformationRadix radix)
            => ConvertToUnitOfInformation(value, from, radix);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="radix">The desired radix to use.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this sbyte value, UnitsOfInformation from, UnitsOfInformationRadix radix)
            => ConvertToUnitOfInformation(value, from, radix);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="radix">The desired radix to use.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this short value, UnitsOfInformation from, UnitsOfInformationRadix radix)
            => ConvertToUnitOfInformation(value, from, radix);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="radix">The desired radix to use.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this uint value, UnitsOfInformation from, UnitsOfInformationRadix radix)
            => ConvertToUnitOfInformation(value, from, radix);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="radix">The desired radix to use.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this ulong value, UnitsOfInformation from, UnitsOfInformationRadix radix)
            => ConvertToUnitOfInformation(value, from, radix);

        /// <summary>Converts the number to a unit of information.<para>Ansprechpartner: Henry de Jongh</para></summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="radix">The desired radix to use.</param>
        /// <returns>The unit of information as string.</returns>
        public static string ToUnitOfInformation(this ushort value, UnitsOfInformation from, UnitsOfInformationRadix radix)
            => ConvertToUnitOfInformation(value, from, radix);

        // -------------------------------------------------------------------------------------------------
        // PRIVATE METHODS
        // -------------------------------------------------------------------------------------------------

        /// <summary>Converts the number to the specified unit of information.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="to">The unit of information to convert to.</param>
        /// <returns>The unit of information as string.</returns>
        private static string ConvertToUnitOfInformation(BigInteger value, UnitsOfInformation from, UnitsOfInformation to)
        {
            bool fromIsBits = IsBits(from);
            bool toIsBits = IsBits(to);
            bool fromIsBytes = IsBytes(from);
            bool toIsBytes = IsBytes(to);

            // result in bits
            if (toIsBits)
            {
                // convert "from" to bits if bytes
                if (fromIsBytes)
                    value = BytesToBits(value);

                // convert value "from" of any bit-unit to bits.
                value = AnyBitUnitToBits(value, from);

                // convert the bits to the "to" unit.
                return BitsToAnyBitUnit(value, to) + " " + GetPrefix(to);
            }

            // result in bytes
            if (toIsBytes)
            {
                // convert "from" to bytes if bits
                if (fromIsBits)
                    value = BitsToBytes(value);

                // convert value "from" of any byte-unit to byte.
                value = AnyByteUnitToBytes(value, from);

                // convert the bytes to the "to" unit.
                return BytesToAnyByteUnit(value, to) + " " + GetPrefix(to);
            }

            // should never happen:
            return value + " ???";
        }

        /// <summary>Converts the number to an appropriate unit of information.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="from">The unit of information to convert from.</param>
        /// <param name="radix">The desired radix to use.</param>
        /// <returns>The unit of information as string.</returns>
        private static string ConvertToUnitOfInformation(BigInteger value, UnitsOfInformation from, UnitsOfInformationRadix radix)
        {
            bool fromIsBits = IsBits(from);
            bool fromIsBytes = IsBytes(from);

            // result in bits
            if (fromIsBits)
            {
                // convert value "from" of any bit-unit to bits.
                value = AnyBitUnitToBits(value, from);

                // convert the bits to the "to" unit.
                return BitsToAppropriateBitUnit(value, radix, out UnitsOfInformation uoi) + " " + GetPrefix(uoi);
            }

            // result in bytes
            if (fromIsBytes)
            {
                // convert value "from" of any byte-unit to byte.
                value = AnyByteUnitToBytes(value, from);

                // convert the bytes to the "to" unit.
                return BytesToAppropriateByteUnit(value, radix, out UnitsOfInformation uoi) + " " + GetPrefix(uoi);
            }

            // should never happen:
            return value + " ???";
        }

        private static BigInteger BytesToBits(BigInteger bytes) => bytes * 8;

        private static BigInteger BitsToBytes(BigInteger bits) => bits / 8;

        /// <summary>Determines whether the specified UnitsOfInformation is bits.</summary>
        /// <param name="uoi">The UnitsOfInformation.</param>
        /// <returns><c>true</c> if the specified UnitsOfInformation is bits; otherwise, <c>false</c>.</returns>
        private static bool IsBits(UnitsOfInformation uoi)
        {
            switch (uoi)
            {
                case UnitsOfInformation.bits:
                case UnitsOfInformation.kilobit:
                case UnitsOfInformation.megabit:
                case UnitsOfInformation.gigabit:
                case UnitsOfInformation.terabit:
                case UnitsOfInformation.petabit:
                case UnitsOfInformation.exabit:
                case UnitsOfInformation.zettabit:
                case UnitsOfInformation.yottabit:
                case UnitsOfInformation.kibibit:
                case UnitsOfInformation.mebibit:
                case UnitsOfInformation.gibibit:
                case UnitsOfInformation.tebibit:
                case UnitsOfInformation.pebibit:
                case UnitsOfInformation.exbibit:
                case UnitsOfInformation.zebibit:
                case UnitsOfInformation.yobibit:
                    return true;
            }
            return false;
        }

        /// <summary>Determines whether the specified UnitsOfInformation is bytes.</summary>
        /// <param name="uoi">The UnitsOfInformation.</param>
        /// <returns><c>true</c> if the specified UnitsOfInformation is bytes; otherwise, <c>false</c>.</returns>
        private static bool IsBytes(UnitsOfInformation uoi)
        {
            switch (uoi)
            {
                case UnitsOfInformation.bytes:
                case UnitsOfInformation.kilobyte:
                case UnitsOfInformation.megabyte:
                case UnitsOfInformation.gigabyte:
                case UnitsOfInformation.terabyte:
                case UnitsOfInformation.petabyte:
                case UnitsOfInformation.exabyte:
                case UnitsOfInformation.zettabyte:
                case UnitsOfInformation.yottabyte:
                case UnitsOfInformation.kibibyte:
                case UnitsOfInformation.mebibyte:
                case UnitsOfInformation.gibibyte:
                case UnitsOfInformation.tebibyte:
                case UnitsOfInformation.pebibyte:
                case UnitsOfInformation.exbibyte:
                case UnitsOfInformation.zebibyte:
                case UnitsOfInformation.yobibyte:
                    return true;
            }
            return false;
        }

        private static string GetPrefix(UnitsOfInformation uoi)
        {
            switch (uoi)
            {
                case UnitsOfInformation.bits:
                    return "b";

                case UnitsOfInformation.bytes:
                    return "B";

                case UnitsOfInformation.kilobit:
                    return "Kbit"; // officially should be "kbit"
                case UnitsOfInformation.megabit:
                    return "Mbit";

                case UnitsOfInformation.gigabit:
                    return "Gbit";

                case UnitsOfInformation.terabit:
                    return "Tbit";

                case UnitsOfInformation.petabit:
                    return "Pbit";

                case UnitsOfInformation.exabit:
                    return "Ebit";

                case UnitsOfInformation.zettabit:
                    return "Zbit";

                case UnitsOfInformation.yottabit:
                    return "Ybit";

                case UnitsOfInformation.kibibit:
                    return "Kibit";

                case UnitsOfInformation.mebibit:
                    return "Mibit";

                case UnitsOfInformation.gibibit:
                    return "Gibit";

                case UnitsOfInformation.tebibit:
                    return "Tibit";

                case UnitsOfInformation.pebibit:
                    return "Pibit";

                case UnitsOfInformation.exbibit:
                    return "Eibit";

                case UnitsOfInformation.zebibit:
                    return "Zibit";

                case UnitsOfInformation.yobibit:
                    return "Yibit";

                case UnitsOfInformation.kilobyte:
                    return "KB"; // officially should be "kB"
                case UnitsOfInformation.megabyte:
                    return "MB";

                case UnitsOfInformation.gigabyte:
                    return "GB";

                case UnitsOfInformation.terabyte:
                    return "TB";

                case UnitsOfInformation.petabyte:
                    return "PB";

                case UnitsOfInformation.exabyte:
                    return "EB";

                case UnitsOfInformation.zettabyte:
                    return "ZB";

                case UnitsOfInformation.yottabyte:
                    return "YB";

                case UnitsOfInformation.kibibyte:
                    return "KiB";

                case UnitsOfInformation.mebibyte:
                    return "MiB";

                case UnitsOfInformation.gibibyte:
                    return "GiB";

                case UnitsOfInformation.tebibyte:
                    return "TiB";

                case UnitsOfInformation.pebibyte:
                    return "PiB";

                case UnitsOfInformation.exbibyte:
                    return "EiB";

                case UnitsOfInformation.zebibyte:
                    return "ZiB";

                case UnitsOfInformation.yobibyte:
                    return "YiB";

                default:
                    return "???";
            }
        }

        private static BigInteger AnyBitUnitToBits(BigInteger value, UnitsOfInformation uoi)
        {
            switch (uoi)
            {
                case UnitsOfInformation.bits:
                    return value;

                case UnitsOfInformation.kilobit:
                    return value * 1000;

                case UnitsOfInformation.megabit:
                    return value * 1000 * 1000;

                case UnitsOfInformation.gigabit:
                    return value * 1000 * 1000 * 1000;

                case UnitsOfInformation.terabit:
                    return value * 1000 * 1000 * 1000 * 1000;

                case UnitsOfInformation.petabit:
                    return value * 1000 * 1000 * 1000 * 1000 * 1000;

                case UnitsOfInformation.exabit:
                    return value * 1000 * 1000 * 1000 * 1000 * 1000 * 1000;

                case UnitsOfInformation.zettabit:
                    return value * 1000 * 1000 * 1000 * 1000 * 1000 * 1000 * 1000;

                case UnitsOfInformation.yottabit:
                    return value * 1000 * 1000 * 1000 * 1000 * 1000 * 1000 * 1000 * 1000;

                case UnitsOfInformation.kibibit:
                    return value * 1024;

                case UnitsOfInformation.mebibit:
                    return value * 1024 * 1024;

                case UnitsOfInformation.gibibit:
                    return value * 1024 * 1024 * 1024;

                case UnitsOfInformation.tebibit:
                    return value * 1024 * 1024 * 1024 * 1024;

                case UnitsOfInformation.pebibit:
                    return value * 1024 * 1024 * 1024 * 1024 * 1024;

                case UnitsOfInformation.exbibit:
                    return value * 1024 * 1024 * 1024 * 1024 * 1024 * 1024;

                case UnitsOfInformation.zebibit:
                    return value * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024;

                case UnitsOfInformation.yobibit:
                    return value * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024;

                default:
                    return value;
            }
        }

        private static BigInteger AnyByteUnitToBytes(BigInteger value, UnitsOfInformation uoi)
        {
            switch (uoi)
            {
                case UnitsOfInformation.bytes:
                    return value;

                case UnitsOfInformation.kilobyte:
                    return value * 1000;

                case UnitsOfInformation.megabyte:
                    return value * 1000 * 1000;

                case UnitsOfInformation.gigabyte:
                    return value * 1000 * 1000 * 1000;

                case UnitsOfInformation.terabyte:
                    return value * 1000 * 1000 * 1000 * 1000;

                case UnitsOfInformation.petabyte:
                    return value * 1000 * 1000 * 1000 * 1000 * 1000;

                case UnitsOfInformation.exabyte:
                    return value * 1000 * 1000 * 1000 * 1000 * 1000 * 1000;

                case UnitsOfInformation.zettabyte:
                    return value * 1000 * 1000 * 1000 * 1000 * 1000 * 1000 * 1000;

                case UnitsOfInformation.yottabyte:
                    return value * 1000 * 1000 * 1000 * 1000 * 1000 * 1000 * 1000 * 1000;

                case UnitsOfInformation.kibibyte:
                    return value * 1024;

                case UnitsOfInformation.mebibyte:
                    return value * 1024 * 1024;

                case UnitsOfInformation.gibibyte:
                    return value * 1024 * 1024 * 1024;

                case UnitsOfInformation.tebibyte:
                    return value * 1024 * 1024 * 1024 * 1024;

                case UnitsOfInformation.pebibyte:
                    return value * 1024 * 1024 * 1024 * 1024 * 1024;

                case UnitsOfInformation.exbibyte:
                    return value * 1024 * 1024 * 1024 * 1024 * 1024 * 1024;

                case UnitsOfInformation.zebibyte:
                    return value * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024;

                case UnitsOfInformation.yobibyte:
                    return value * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024;

                default:
                    return value;
            }
        }

        private static BigInteger BitsToAnyBitUnit(BigInteger value, UnitsOfInformation uoi)
        {
            switch (uoi)
            {
                case UnitsOfInformation.bits:
                    return value;

                case UnitsOfInformation.kilobit:
                    return value / 1000;

                case UnitsOfInformation.megabit:
                    return value / 1000 / 1000;

                case UnitsOfInformation.gigabit:
                    return value / 1000 / 1000 / 1000;

                case UnitsOfInformation.terabit:
                    return value / 1000 / 1000 / 1000 / 1000;

                case UnitsOfInformation.petabit:
                    return value / 1000 / 1000 / 1000 / 1000 / 1000;

                case UnitsOfInformation.exabit:
                    return value / 1000 / 1000 / 1000 / 1000 / 1000 / 1000;

                case UnitsOfInformation.zettabit:
                    return value / 1000 / 1000 / 1000 / 1000 / 1000 / 1000 / 1000;

                case UnitsOfInformation.yottabit:
                    return value / 1000 / 1000 / 1000 / 1000 / 1000 / 1000 / 1000 / 1000;

                case UnitsOfInformation.kibibit:
                    return value / 1024;

                case UnitsOfInformation.mebibit:
                    return value / 1024 / 1024;

                case UnitsOfInformation.gibibit:
                    return value / 1024 / 1024 / 1024;

                case UnitsOfInformation.tebibit:
                    return value / 1024 / 1024 / 1024 / 1024;

                case UnitsOfInformation.pebibit:
                    return value / 1024 / 1024 / 1024 / 1024 / 1024;

                case UnitsOfInformation.exbibit:
                    return value / 1024 / 1024 / 1024 / 1024 / 1024 / 1024;

                case UnitsOfInformation.zebibit:
                    return value / 1024 / 1024 / 1024 / 1024 / 1024 / 1024 / 1024;

                case UnitsOfInformation.yobibit:
                    return value / 1024 / 1024 / 1024 / 1024 / 1024 / 1024 / 1024 / 1024;

                default:
                    return value;
            }
        }

        private static BigInteger BytesToAnyByteUnit(BigInteger value, UnitsOfInformation uoi)
        {
            switch (uoi)
            {
                case UnitsOfInformation.bytes:
                    return value;

                case UnitsOfInformation.kilobyte:
                    return value / 1000;

                case UnitsOfInformation.megabyte:
                    return value / 1000 / 1000;

                case UnitsOfInformation.gigabyte:
                    return value / 1000 / 1000 / 1000;

                case UnitsOfInformation.terabyte:
                    return value / 1000 / 1000 / 1000 / 1000;

                case UnitsOfInformation.petabyte:
                    return value / 1000 / 1000 / 1000 / 1000 / 1000;

                case UnitsOfInformation.exabyte:
                    return value / 1000 / 1000 / 1000 / 1000 / 1000 / 1000;

                case UnitsOfInformation.zettabyte:
                    return value / 1000 / 1000 / 1000 / 1000 / 1000 / 1000 / 1000;

                case UnitsOfInformation.yottabyte:
                    return value / 1000 / 1000 / 1000 / 1000 / 1000 / 1000 / 1000 / 1000;

                case UnitsOfInformation.kibibyte:
                    return value / 1024;

                case UnitsOfInformation.mebibyte:
                    return value / 1024 / 1024;

                case UnitsOfInformation.gibibyte:
                    return value / 1024 / 1024 / 1024;

                case UnitsOfInformation.tebibyte:
                    return value / 1024 / 1024 / 1024 / 1024;

                case UnitsOfInformation.pebibyte:
                    return value / 1024 / 1024 / 1024 / 1024 / 1024;

                case UnitsOfInformation.exbibyte:
                    return value / 1024 / 1024 / 1024 / 1024 / 1024 / 1024;

                case UnitsOfInformation.zebibyte:
                    return value / 1024 / 1024 / 1024 / 1024 / 1024 / 1024 / 1024;

                case UnitsOfInformation.yobibyte:
                    return value / 1024 / 1024 / 1024 / 1024 / 1024 / 1024 / 1024 / 1024;

                default:
                    return value;
            }
        }

        private static BigInteger BitsToAppropriateBitUnit(BigInteger value, UnitsOfInformationRadix radix, out UnitsOfInformation uoi)
        {
            switch (radix)
            {
                case UnitsOfInformationRadix.Decimal:
                    if (value < 1000) { uoi = UnitsOfInformation.bits; return value; }
                    value /= 1000;
                    if (value < 1000) { uoi = UnitsOfInformation.kilobit; return value; }
                    value /= 1000;
                    if (value < 1000) { uoi = UnitsOfInformation.megabit; return value; }
                    value /= 1000;
                    if (value < 1000) { uoi = UnitsOfInformation.gigabit; return value; }
                    value /= 1000;
                    if (value < 1000) { uoi = UnitsOfInformation.terabit; return value; }
                    value /= 1000;
                    if (value < 1000) { uoi = UnitsOfInformation.petabit; return value; }
                    value /= 1000;
                    if (value < 1000) { uoi = UnitsOfInformation.exabit; return value; }
                    value /= 1000;
                    if (value < 1000) { uoi = UnitsOfInformation.zettabit; return value; }
                    value /= 1000;
                    if (value < 1000) { uoi = UnitsOfInformation.yottabit; return value; }
                    value /= 1000;
                    uoi = UnitsOfInformation.bits;
                    return value;

                case UnitsOfInformationRadix.Binary:
                    if (value < 1024) { uoi = UnitsOfInformation.bits; return value; }
                    value /= 1024;
                    if (value < 1024) { uoi = UnitsOfInformation.kibibit; return value; }
                    value /= 1024;
                    if (value < 1024) { uoi = UnitsOfInformation.mebibit; return value; }
                    value /= 1024;
                    if (value < 1024) { uoi = UnitsOfInformation.gibibit; return value; }
                    value /= 1024;
                    if (value < 1024) { uoi = UnitsOfInformation.tebibit; return value; }
                    value /= 1024;
                    if (value < 1024) { uoi = UnitsOfInformation.pebibit; return value; }
                    value /= 1024;
                    if (value < 1024) { uoi = UnitsOfInformation.exbibit; return value; }
                    value /= 1024;
                    if (value < 1024) { uoi = UnitsOfInformation.zebibit; return value; }
                    value /= 1024;
                    if (value < 1024) { uoi = UnitsOfInformation.yobibit; return value; }
                    value /= 1024;
                    uoi = UnitsOfInformation.bits;
                    return value;
            }
            uoi = UnitsOfInformation.bits;
            return value;
        }

        private static BigInteger BytesToAppropriateByteUnit(BigInteger value, UnitsOfInformationRadix radix, out UnitsOfInformation uoi)
        {
            switch (radix)
            {
                case UnitsOfInformationRadix.Decimal:
                    if (value < 1000) { uoi = UnitsOfInformation.bytes; return value; }
                    value /= 1000;
                    if (value < 1000) { uoi = UnitsOfInformation.kilobyte; return value; }
                    value /= 1000;
                    if (value < 1000) { uoi = UnitsOfInformation.megabyte; return value; }
                    value /= 1000;
                    if (value < 1000) { uoi = UnitsOfInformation.gigabyte; return value; }
                    value /= 1000;
                    if (value < 1000) { uoi = UnitsOfInformation.terabyte; return value; }
                    value /= 1000;
                    if (value < 1000) { uoi = UnitsOfInformation.petabyte; return value; }
                    value /= 1000;
                    if (value < 1000) { uoi = UnitsOfInformation.exabyte; return value; }
                    value /= 1000;
                    if (value < 1000) { uoi = UnitsOfInformation.zettabyte; return value; }
                    value /= 1000;
                    if (value < 1000) { uoi = UnitsOfInformation.yottabyte; return value; }
                    value /= 1000;
                    uoi = UnitsOfInformation.bytes;
                    return value;

                case UnitsOfInformationRadix.Binary:
                    if (value < 1024) { uoi = UnitsOfInformation.bytes; return value; }
                    value /= 1024;
                    if (value < 1024) { uoi = UnitsOfInformation.kibibyte; return value; }
                    value /= 1024;
                    if (value < 1024) { uoi = UnitsOfInformation.mebibyte; return value; }
                    value /= 1024;
                    if (value < 1024) { uoi = UnitsOfInformation.gibibyte; return value; }
                    value /= 1024;
                    if (value < 1024) { uoi = UnitsOfInformation.tebibyte; return value; }
                    value /= 1024;
                    if (value < 1024) { uoi = UnitsOfInformation.pebibyte; return value; }
                    value /= 1024;
                    if (value < 1024) { uoi = UnitsOfInformation.exbibyte; return value; }
                    value /= 1024;
                    if (value < 1024) { uoi = UnitsOfInformation.zebibyte; return value; }
                    value /= 1024;
                    if (value < 1024) { uoi = UnitsOfInformation.yobibyte; return value; }
                    value /= 1024;
                    uoi = UnitsOfInformation.bytes;
                    return value;
            }
            uoi = UnitsOfInformation.bytes;
            return value;
        }
    }
}