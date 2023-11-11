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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OOLaboratories.Collections
{
    /// <summary>
    /// Manages a compact array of bit values, which are represented as Booleans, where true
    /// indicates that the bit is on (1) and false indicates the bit is off (0).
    /// </summary>
    public class BitArray : IReadOnlyCollection<bool>, ICloneable
    {
        /// <summary>The internal array of 32-bit elements that store the bits.</summary>
        private readonly uint[] _Data;

        /// <summary>The size of the array in bits.</summary>
        private readonly int _Size;

        /// <summary>Creates a new instance of <see cref="BitArray"/> for cloning.</summary>
        private BitArray()
        {
            // invalid until private properties are manually assigned.
        }

        /// <summary>
        /// Creates a new instance of <see cref="BitArray"/> with the specified amount of bits.
        /// </summary>
        /// <param name="size">The amount of bits to be stored in the array.</param>
        public BitArray(int size)
        {
            if (size < 0) throw new ArgumentOutOfRangeException(nameof(size), "Non-negative number required.");
            _Size = size;

            // create the internal array of 32-bit elements.
            _Data = new uint[CalculateDataLength(size)];
        }

        /// <summary>
        /// Creates a new instance of <see cref="BitArray"/> and copies the bits from another <see cref="BitArray"/>.
        /// </summary>
        /// <param name="original">The bit array to be copied into this new instance.</param>
        public BitArray(BitArray original)
        {
            _Data = (uint[])original._Data.Clone();
            _Size = original._Size;
        }

        /// <summary>Calculates the 32-bit array length required to fit the amount of bits inside.</summary>
        /// <param name="bits">The amount of bits to fit into the array.</param>
        /// <returns>The number of 32-bit elements required.</returns>
        private int CalculateDataLength(int bits)
        {
            return bits > 0 ? (((bits - 1) / 32) + 1) : 0;
        }

        /// <summary>Gets the total number of bits in the array.</summary>
        public int Length => _Size;

        /// <summary>Gets or sets the bit at the specified array index.</summary>
        /// <param name="index">The index of the bit in the array.</param>
        /// <returns>True when the bit is on (1) and false when the bit is off (0).</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Index was outside the bounds of the array.
        /// </exception>
        public bool this[int index]
        {
            get
            {
                if (index >= _Size) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                return (_Data[index / 32] & (1 << ((31 - index) % 32))) > 0;
            }
            set
            {
                if (index < 0 || index >= _Size) throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                if (value)
                    _Data[index / 32] |= (uint)1 << ((31 - index) % 32);
                else
                    _Data[index / 32] &= ~((uint)1 << ((31 - index) % 32));
            }
        }

        /// <summary>Reads 8 bits starting at the specified bit array index as an unsigned byte.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 8-bit unsigned integer.</returns>
        public byte GetByte(int index)
        {
            if (index < 0 || index > _Size - 8) throw new IndexOutOfRangeException("Index was outside; or reading beyond the bounds of the array.");
            byte result = 0;
            for (int i = 0; i < 8; i++)
                result |= (byte)((this[index + i] ? 1 : 0) << (7 - i));
            return result;
        }

        /// <summary>Writes 8 bits starting at the specified bit array index as an unsigned byte.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 8-bit unsigned integer.</param>
        public void SetByte(int index, byte value)
        {
            if (index < 0 || index > _Size - 8) throw new IndexOutOfRangeException("Index was outside; or writing beyond the bounds of the array.");
            for (int i = 0; i < 8; i++)
                this[index + i] = (value & (1 << (7 - i))) > 0;
        }

        /// <summary>Reads 16 bits starting at the specified bit array index as an unsigned integer.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 16-bit unsigned integer.</returns>
        public ushort GetUInt16(int index)
        {
            if (index < 0 || index > _Size - 16) throw new IndexOutOfRangeException("Index was outside; or reading beyond the bounds of the array.");
            return BitConverter.ToUInt16(new byte[] { GetByte(index), GetByte(index + 8) }, 0);
        }

        /// <summary>Writes 16 bits starting at the specified bit array index as an unsigned integer.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 16-bit unsigned integer.</param>
        public void SetUInt16(int index, ushort value)
        {
            var bytes = BitConverter.GetBytes(value);
            SetByte(index, bytes[0]);
            SetByte(index + 8, bytes[1]);
        }

        /// <summary>Reads 16 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 16-bit unsigned integer.</returns>
        public ushort GetUInt16BigEndian(int index)
        {
            if (index < 0 || index > _Size - 16) throw new IndexOutOfRangeException("Index was outside; or reading beyond the bounds of the array.");
            return BitConverter.ToUInt16(new byte[] { GetByte(index + 8), GetByte(index) }, 0);
        }

        /// <summary>Writes 16 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 16-bit unsigned integer.</param>
        public void SetUInt16BigEndian(int index, ushort value)
        {
            var bytes = BitConverter.GetBytes(value);
            SetByte(index, bytes[1]);
            SetByte(index + 8, bytes[0]);
        }

        /// <summary>Reads 16 bits starting at the specified bit array index as a signed integer.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 16-bit signed integer.</returns>
        public short GetInt16(int index) => unchecked((short)GetUInt16(index));

        /// <summary>Writes 16 bits starting at the specified bit array index as a signed integer.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 16-bit signed integer.</param>
        public void SetInt16(int index, short value) => SetUInt16(index, unchecked((ushort)value));

        /// <summary>Reads 16 bits starting at the specified bit array index as a signed integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 16-bit signed integer.</returns>
        public short GetInt16BigEndian(int index) => unchecked((short)GetUInt16BigEndian(index));

        /// <summary>Writes 16 bits starting at the specified bit array index as a signed integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 16-bit signed integer.</param>
        public void SetInt16BigEndian(int index, short value) => SetUInt16BigEndian(index, unchecked((ushort)value));

        /// <summary>Reads 32 bits starting at the specified bit array index as an unsigned integer.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 32-bit unsigned integer.</returns>
        public uint GetUInt32(int index)
        {
            if (index < 0 || index > _Size - 32) throw new IndexOutOfRangeException("Index was outside; or reading beyond the bounds of the array.");
            return BitConverter.ToUInt32(new byte[] { GetByte(index), GetByte(index + 8), GetByte(index + 16), GetByte(index + 24) }, 0);
        }

        /// <summary>Writes 32 bits starting at the specified bit array index as an unsigned integer.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 32-bit unsigned integer.</param>
        public void SetUInt32(int index, uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            SetByte(index, bytes[0]);
            SetByte(index + 8, bytes[1]);
            SetByte(index + 16, bytes[2]);
            SetByte(index + 24, bytes[3]);
        }

        /// <summary>Reads 32 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 32-bit unsigned integer.</returns>
        public uint GetUInt32BigEndian(int index)
        {
            if (index < 0 || index > _Size - 32) throw new IndexOutOfRangeException("Index was outside; or reading beyond the bounds of the array.");
            return BitConverter.ToUInt32(new byte[] { GetByte(index + 24), GetByte(index + 16), GetByte(index + 8), GetByte(index) }, 0);
        }

        /// <summary>Writes 32 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 32-bit unsigned integer.</param>
        public void SetUInt32BigEndian(int index, uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            SetByte(index, bytes[3]);
            SetByte(index + 8, bytes[2]);
            SetByte(index + 16, bytes[1]);
            SetByte(index + 24, bytes[0]);
        }

        /// <summary>Reads 32 bits starting at the specified bit array index as a signed integer.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 32-bit signed integer.</returns>
        public int GetInt32(int index) => unchecked((int)GetUInt32(index));

        /// <summary>Writes 32 bits starting at the specified bit array index as a signed integer.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 32-bit signed integer.</param>
        public void SetInt32(int index, int value) => SetUInt32(index, unchecked((uint)value));

        /// <summary>Reads 32 bits starting at the specified bit array index as a signed integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 32-bit signed integer.</returns>
        public int GetInt32BigEndian(int index) => unchecked((int)GetUInt32BigEndian(index));

        /// <summary>Writes 32 bits starting at the specified bit array index as a signed integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 32-bit signed integer.</param>
        public void SetInt32BigEndian(int index, int value) => SetUInt32BigEndian(index, unchecked((uint)value));

        /// <summary>Reads 32 bits starting at the specified bit array index as a single-precision floating-point value.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 32-bit single-precision floating-point number.</returns>
        public float GetSingle(int index) => FloatToInt.Convert(GetUInt32(index));

        /// <summary>Writes 32 bits starting at the specified bit array index as a single-precision floating-point value.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 32-bit single-precision floating-point number.</param>
        public void SetSingle(int index, float value) => SetUInt32(index, FloatToInt.Convert(value));

        /// <summary>Reads 64 bits starting at the specified bit array index as an unsigned integer.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 64-bit unsigned integer.</returns>
        public ulong GetUInt64(int index)
        {
            if (index < 0 || index > _Size - 64) throw new IndexOutOfRangeException("Index was outside; or reading beyond the bounds of the array.");
            return BitConverter.ToUInt64(new byte[] { GetByte(index), GetByte(index + 8), GetByte(index + 16), GetByte(index + 24), GetByte(index + 32), GetByte(index + 40), GetByte(index + 48), GetByte(index + 56) }, 0);
        }

        /// <summary>Writes 64 bits starting at the specified bit array index as an unsigned integer.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 64-bit unsigned integer.</param>
        public void SetUInt64(int index, ulong value)
        {
            var bytes = BitConverter.GetBytes(value);
            SetByte(index, bytes[0]);
            SetByte(index + 8, bytes[1]);
            SetByte(index + 16, bytes[2]);
            SetByte(index + 24, bytes[3]);
            SetByte(index + 32, bytes[4]);
            SetByte(index + 40, bytes[5]);
            SetByte(index + 48, bytes[6]);
            SetByte(index + 56, bytes[7]);
        }

        /// <summary>Reads 64 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 64-bit unsigned integer.</returns>
        public ulong GetUInt64BigEndian(int index)
        {
            if (index < 0 || index > _Size - 64) throw new IndexOutOfRangeException("Index was outside; or reading beyond the bounds of the array.");
            return BitConverter.ToUInt64(new byte[] { GetByte(index + 56), GetByte(index + 48), GetByte(index + 40), GetByte(index + 32), GetByte(index + 24), GetByte(index + 16), GetByte(index + 8), GetByte(index) }, 0);
        }

        /// <summary>Writes 64 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 64-bit unsigned integer.</param>
        public void SetUInt64BigEndian(int index, ulong value)
        {
            var bytes = BitConverter.GetBytes(value);
            SetByte(index, bytes[7]);
            SetByte(index + 8, bytes[6]);
            SetByte(index + 16, bytes[5]);
            SetByte(index + 24, bytes[4]);
            SetByte(index + 32, bytes[3]);
            SetByte(index + 40, bytes[2]);
            SetByte(index + 48, bytes[1]);
            SetByte(index + 56, bytes[0]);
        }

        /// <summary>Reads 64 bits starting at the specified bit array index as a signed integer.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 64-bit signed integer.</returns>
        public long GetInt64(int index) => unchecked((long)GetUInt64(index));

        /// <summary>Writes 64 bits starting at the specified bit array index as a signed integer.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 64-bit signed integer.</param>
        public void SetInt64(int index, long value) => SetUInt64(index, unchecked((ulong)value));

        /// <summary>Reads 64 bits starting at the specified bit array index as a signed integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 64-bit signed integer.</returns>
        public long GetInt64BigEndian(int index) => unchecked((long)GetUInt64BigEndian(index));

        /// <summary>Writes 64 bits starting at the specified bit array index as a signed integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 64-bit signed integer.</param>
        public void SetInt64BigEndian(int index, long value) => SetUInt64BigEndian(index, unchecked((ulong)value));

        /// <summary>Reads 64 bits starting at the specified bit array index as a double-precision floating-point value.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 64-bit double-precision floating-point number.</returns>
        public double GetDouble(int index) => DoubleToInt.Convert(GetUInt64(index));

        /// <summary>Writes 64 bits starting at the specified bit array index as a double-precision floating-point value.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 64-bit double-precision floating-point number.</param>
        public void SetDouble(int index, double value) => SetUInt64(index, DoubleToInt.Convert(value));

        #region IReadOnlyCollection<int> Implementation

        int IReadOnlyCollection<bool>.Count => _Size;

        public IEnumerator<bool> GetEnumerator()
        {
            for (int i = 0; i < _Size; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IReadOnlyCollection<int> Implementation

        #region ICloneable Implementation

        public object Clone()
        {
            return new BitArray(this);
        }

        #endregion ICloneable Implementation

        // https://stackoverflow.com/a/59273138
        [StructLayout(LayoutKind.Explicit)]
        private struct FloatToInt
        {
            [FieldOffset(0)] private float f;
            [FieldOffset(0)] private uint i;

            public static uint Convert(float value)
            {
                return new FloatToInt { f = value }.i;
            }

            public static float Convert(uint value)
            {
                return new FloatToInt { i = value }.f;
            }
        }

        // https://stackoverflow.com/a/59273138
        [StructLayout(LayoutKind.Explicit)]
        private struct DoubleToInt
        {
            [FieldOffset(0)] private double f;
            [FieldOffset(0)] private ulong i;

            public static ulong Convert(double value)
            {
                return new DoubleToInt { f = value }.i;
            }

            public static double Convert(ulong value)
            {
                return new DoubleToInt { i = value }.f;
            }
        }
    }
}