///////////////////////////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright(c) 2023 Henry de Jongh
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
////////////////////// https://github.com/Henry00IS/CSharp ////////// http://00laboratories.com/ //

using OOLaboratories.Collections;
using System;

namespace BitArrays
{
    /// <summary>
    /// Manages a compact two-dimensional array of bit values, which are represented as Booleans,
    /// where true indicates that the bit is on (1) and false indicates the bit is off (0).
    /// </summary>
    public class BitArray2 : ICloneable
    {
        /// <summary>The internal one-dimensional array of bits.</summary>
        private readonly BitArray _Bits;

        /// <summary>The width of the array in bits.</summary>
        private readonly int _Width;

        /// <summary>The height of the array in bits.</summary>
        private readonly int _Height;

        /// <summary>The width of the array in bits.</summary>
        public int Width => _Width;

        /// <summary>The height of the array in bits.</summary>
        public int Height => _Height;

        /// <summary>Creates a new instance of <see cref="BitArray2"/> for cloning.</summary>
        private BitArray2()
        {
            // invalid until private properties are manually assigned.
        }

        /// <summary>
        /// Creates a new instance of <see cref="BitArray2"/> with the specified amount of bits.
        /// </summary>
        /// <param name="width">The amount of bits to be stored in the array horizontally.</param>
        /// <param name="height">The amount of bits to be stored in the array vertically.</param>
        public BitArray2(int width, int height)
        {
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width), "Non-negative number required.");
            if (height < 0) throw new ArgumentOutOfRangeException(nameof(height), "Non-negative number required.");
            _Width = width;
            _Height = height;

            // create the internal one-dimensional array of bits.
            _Bits = new BitArray(width * height);
        }

        /// <summary>
        /// Creates a new instance of <see cref="BitArray2"/> from an existing <see cref="BitArray"/>.
        /// </summary>
        /// <param name="width">The amount of bits to be stored in the array horizontally.</param>
        /// <param name="height">The amount of bits to be stored in the array vertically.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The width and height must match the amount of bits in the given bit array.
        /// </exception>
        public BitArray2(BitArray bits, int width, int height)
        {
            if (width * height != bits.Length) throw new ArgumentOutOfRangeException("width,height", "The width and height must match the amount of bits in the given bit array.");
            _Width = width;
            _Height = height;

            // copy the one-dimensional array of bits.
            _Bits = new BitArray(bits);
        }

        /// <summary>
        /// Creates a new instance of <see cref="BitArray2"/> and copies the bits from another <see cref="BitArray2"/>.
        /// </summary>
        /// <param name="original">The bit array to be copied into this new instance.</param>
        public BitArray2(BitArray2 original)
        {
            _Bits = original.ToBitArray();
            _Width = original._Width;
            _Height = original._Height;
        }

        /// <summary>
        /// Converts this two-dimensional <see cref="BitArray2"/> into a one-dimensional <see cref="BitArray"/>.
        /// </summary>
        /// <returns>The one-dimensional <see cref="BitArray"/> with a copy of all of the bits.</returns>
        public BitArray ToBitArray() => new BitArray(_Bits);

        /// <summary>Gets or sets a bit at the specified coordinates.</summary>
        /// <param name="x">The X-Coordinate in the array (up to the width).</param>
        /// <param name="y">The Y-Coordinate in the array (up to the height).</param>
        /// <returns>True when the bit is on (1) and false when the bit is off (0).</returns>
        public bool this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                return _Bits[x + y * _Width];
            }
            set
            {
                if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                _Bits[x + y * _Width] = value;
            }
        }

        /// <summary>Reads 8 bits starting at the specified bit array index as an unsigned byte.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start reading at.</param>
        /// <param name="y">The Y-Coordinate in the bit array to start reading at.</param>
        /// <returns>The 8-bit unsigned integer.</returns>
        public byte GetByte(int x, int y)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            return _Bits.GetByte(x + y * _Width);
        }

        /// <summary>Writes 8 bits starting at the specified bit array index as an unsigned byte.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start writing at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start writing at.</param>
        /// <param name="value">The 8-bit unsigned integer.</param>
        public void SetByte(int x, int y, byte value)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            _Bits.SetByte(x + y * _Width, value);
        }

        /// <summary>Reads 16 bits starting at the specified bit array index as an unsigned integer.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start reading at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start reading at.</param>
        /// <returns>The 16-bit unsigned integer.</returns>
        public ushort GetUInt16(int x, int y)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            return _Bits.GetUInt16(x + y * _Width);
        }

        /// <summary>Writes 16 bits starting at the specified bit array index as an unsigned integer.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start writing at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start writing at.</param>
        /// <param name="value">The 16-bit unsigned integer.</param>
        public void SetUInt16(int x, int y, ushort value)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            _Bits.SetUInt16(x + y * _Width, value);
        }

        /// <summary>Reads 16 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start reading at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start reading at.</param>
        /// <returns>The 16-bit unsigned integer.</returns>
        public ushort GetUInt16BigEndian(int x, int y)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            return _Bits.GetUInt16BigEndian(x + y * _Width);
        }

        /// <summary>Writes 16 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start writing at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start writing at.</param>
        /// <param name="value">The 16-bit unsigned integer.</param>
        public void SetUInt16BigEndian(int x, int y, ushort value)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            _Bits.SetUInt16BigEndian(x + y * _Width, value);
        }

        /// <summary>Reads 16 bits starting at the specified bit array index as a signed integer.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start reading at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start reading at.</param>
        /// <returns>The 16-bit signed integer.</returns>
        public short GetInt16(int x, int y)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            return _Bits.GetInt16(x + y * _Width);
        }

        /// <summary>Writes 16 bits starting at the specified bit array index as a signed integer.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start writing at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start writing at.</param>
        /// <param name="value">The 16-bit signed integer.</param>
        public void SetInt16(int x, int y, short value)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            _Bits.SetInt16(x + y * _Width, value);
        }

        /// <summary>Reads 16 bits starting at the specified bit array index as a signed integer in big-endian order.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start reading at.</param>
        /// <param name="y">The Y-Coordinate in the bit array to start reading at.</param>
        /// <returns>The 16-bit signed integer.</returns>
        public short GetInt16BigEndian(int x, int y)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            return _Bits.GetInt16BigEndian(x + y * _Width);
        }

        /// <summary>Writes 16 bits starting at the specified bit array index as a signed integer in big-endian order.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start writing at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start writing at.</param>
        /// <param name="value">The 16-bit signed integer.</param>
        public void SetInt16BigEndian(int x, int y, short value)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            _Bits.SetInt16BigEndian(x + y * _Width, value);
        }

        /// <summary>Reads 32 bits starting at the specified bit array index as an unsigned integer.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start reading at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start reading at.</param>
        /// <returns>The 32-bit unsigned integer.</returns>
        public uint GetUInt32(int x, int y)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            return _Bits.GetUInt32(x + y * _Width);
        }

        /// <summary>Writes 32 bits starting at the specified bit array index as an unsigned integer.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start writing at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start writing at.</param>
        /// <param name="value">The 32-bit unsigned integer.</param>
        public void SetUInt32(int x, int y, uint value)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            _Bits.SetUInt32(x + y * _Width, value);
        }

        /// <summary>Reads 32 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start reading at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start reading at.</param>
        /// <returns>The 32-bit unsigned integer.</returns>
        public uint GetUInt32BigEndian(int x, int y)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            return _Bits.GetUInt32BigEndian(x + y * _Width);
        }

        /// <summary>Writes 32 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start writing at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start writing at.</param>
        /// <param name="value">The 32-bit unsigned integer.</param>
        public void SetUInt32BigEndian(int x, int y, uint value)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            _Bits.SetUInt32BigEndian(x + y * _Width, value);
        }

        /// <summary>Reads 32 bits starting at the specified bit array index as a signed integer.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start reading at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start reading at.</param>
        /// <returns>The 32-bit signed integer.</returns>
        public int GetInt32(int x, int y)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            return _Bits.GetInt32(x + y * _Width);
        }

        /// <summary>Writes 32 bits starting at the specified bit array index as a signed integer.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start writing at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start writing at.</param>
        /// <param name="value">The 32-bit signed integer.</param>
        public void SetInt32(int x, int y, int value)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            _Bits.SetInt32(x + y * _Width, value);
        }

        /// <summary>Reads 32 bits starting at the specified bit array index as a signed integer in big-endian order.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start reading at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start reading at.</param>
        /// <returns>The 32-bit signed integer.</returns>
        public int GetInt32BigEndian(int x, int y)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            return _Bits.GetInt32BigEndian(x + y * _Width);
        }

        /// <summary>Writes 32 bits starting at the specified bit array index as a signed integer in big-endian order.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start writing at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start writing at.</param>
        /// <param name="value">The 32-bit signed integer.</param>
        public void SetInt32BigEndian(int x, int y, int value)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            _Bits.SetInt32BigEndian(x + y * _Width, value);
        }

        /// <summary>Reads 32 bits starting at the specified bit array index as a single-precision floating-point value.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start reading at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start reading at.</param>
        /// <returns>The 32-bit single-precision floating-point number.</returns>
        public float GetSingle(int x, int y)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            return _Bits.GetSingle(x + y * _Width);
        }

        /// <summary>Writes 32 bits starting at the specified bit array index as a single-precision floating-point value.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start writing at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start writing at.</param>
        /// <param name="value">The 32-bit single-precision floating-point number.</param>
        public void SetSingle(int x, int y, float value)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            _Bits.SetSingle(x + y * _Width, value);
        }

        /// <summary>Reads 64 bits starting at the specified bit array index as an unsigned integer.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start reading at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start reading at.</param>
        /// <returns>The 64-bit unsigned integer.</returns>
        public ulong GetUInt64(int x, int y)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            return _Bits.GetUInt64(x + y * _Width);
        }

        /// <summary>Writes 64 bits starting at the specified bit array index as an unsigned integer.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start writing at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start writing at.</param>
        /// <param name="value">The 64-bit unsigned integer.</param>
        public void SetUInt64(int x, int y, ulong value)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            _Bits.SetUInt64(x + y * _Width, value);
        }

        /// <summary>Reads 64 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start reading at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start reading at.</param>
        /// <returns>The 64-bit unsigned integer.</returns>
        public ulong GetUInt64BigEndian(int x, int y)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            return _Bits.GetUInt64BigEndian(x + y * _Width);
        }

        /// <summary>Writes 64 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start writing at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start writing at.</param>
        /// <param name="value">The 64-bit unsigned integer.</param>
        public void SetUInt64BigEndian(int x, int y, ulong value)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            _Bits.SetUInt64BigEndian(x + y * _Width, value);
        }

        /// <summary>Reads 64 bits starting at the specified bit array index as a signed integer.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start reading at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start reading at.</param>
        /// <returns>The 64-bit signed integer.</returns>
        public long GetInt64(int x, int y)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            return _Bits.GetInt64(x + y * _Width);
        }

        /// <summary>Writes 64 bits starting at the specified bit array index as a signed integer.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start writing at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start writing at.</param>
        /// <param name="value">The 64-bit signed integer.</param>
        public void SetInt64(int x, int y, long value)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            _Bits.SetInt64(x + y * _Width, value);
        }

        /// <summary>Reads 64 bits starting at the specified bit array index as a signed integer in big-endian order.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start reading at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start reading at.</param>
        /// <returns>The 64-bit signed integer.</returns>
        public long GetInt64BigEndian(int x, int y)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            return _Bits.GetInt64BigEndian(x + y * _Width);
        }

        /// <summary>Writes 64 bits starting at the specified bit array index as a signed integer in big-endian order.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start writing at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start writing at.</param>
        /// <param name="value">The 64-bit signed integer.</param>
        public void SetInt64BigEndian(int x, int y, long value)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            _Bits.SetInt64BigEndian(x + y * _Width, value);
        }

        /// <summary>Reads 64 bits starting at the specified bit array index as a double-precision floating-point value.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start reading at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start reading at.</param>
        /// <returns>The 64-bit double-precision floating-point number.</returns>
        public double GetDouble(int x, int y)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            return _Bits.GetDouble(x + y * _Width);
        }

        /// <summary>Writes 64 bits starting at the specified bit array index as a double-precision floating-point value.</summary>
        /// <param name="x">The X-Coordinate in the bit array to start writing at.</param>
		/// <param name="y">The Y-Coordinate in the bit array to start writing at.</param>
        /// <param name="value">The 64-bit double-precision floating-point number.</param>
        public void SetDouble(int x, int y, double value)
        {
            if (x < 0 || x >= _Width || y < 0 || y >= _Height) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            _Bits.SetDouble(x + y * _Width, value);
        }

        #region ICloneable Implementation

        public object Clone()
        {
            return new BitArray2(this);
        }

        #endregion ICloneable Implementation
    }
}