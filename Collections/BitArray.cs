﻿///////////////////////////////////////////////////////////////////////////////////////////////////
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

        /// <summary>Retrieves a <see cref="uint[]"/> containing all of the bits in the array.</summary>
        /// <returns>The <see cref="uint[]"/> containing all of the bits.</returns>
        public uint[] ToUInt32Array() => (uint[])_Data.Clone();

        #region Setting and Getting Bytes, Integers and Floats

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
            return new Bytes64(GetByte(index), GetByte(index + 8)).vUInt16;
        }

        /// <summary>Writes 16 bits starting at the specified bit array index as an unsigned integer.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 16-bit unsigned integer.</param>
        public void SetUInt16(int index, ushort value)
        {
            var bytes = new Bytes64(value);
            SetByte(index, bytes.b0);
            SetByte(index + 8, bytes.b1);
        }

        /// <summary>Reads 16 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 16-bit unsigned integer.</returns>
        public ushort GetUInt16BigEndian(int index)
        {
            if (index < 0 || index > _Size - 16) throw new IndexOutOfRangeException("Index was outside; or reading beyond the bounds of the array.");
            return new Bytes64(GetByte(index + 8), GetByte(index)).vUInt16;
        }

        /// <summary>Writes 16 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 16-bit unsigned integer.</param>
        public void SetUInt16BigEndian(int index, ushort value)
        {
            var bytes = new Bytes64(value);
            SetByte(index, bytes.b1);
            SetByte(index + 8, bytes.b0);
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
            return new Bytes64(GetByte(index), GetByte(index + 8), GetByte(index + 16), GetByte(index + 24)).vUInt32;
        }

        /// <summary>Writes 32 bits starting at the specified bit array index as an unsigned integer.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 32-bit unsigned integer.</param>
        public void SetUInt32(int index, uint value)
        {
            var bytes = new Bytes64(value);
            SetByte(index, bytes.b0);
            SetByte(index + 8, bytes.b1);
            SetByte(index + 16, bytes.b2);
            SetByte(index + 24, bytes.b3);
        }

        /// <summary>Reads 32 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 32-bit unsigned integer.</returns>
        public uint GetUInt32BigEndian(int index)
        {
            if (index < 0 || index > _Size - 32) throw new IndexOutOfRangeException("Index was outside; or reading beyond the bounds of the array.");
            return new Bytes64(GetByte(index + 24), GetByte(index + 16), GetByte(index + 8), GetByte(index)).vUInt32;
        }

        /// <summary>Writes 32 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 32-bit unsigned integer.</param>
        public void SetUInt32BigEndian(int index, uint value)
        {
            var bytes = new Bytes64(value);
            SetByte(index, bytes.b3);
            SetByte(index + 8, bytes.b2);
            SetByte(index + 16, bytes.b1);
            SetByte(index + 24, bytes.b0);
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
        public float GetSingle(int index) => new Bytes64(GetUInt32(index)).vSingle;

        /// <summary>Writes 32 bits starting at the specified bit array index as a single-precision floating-point value.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 32-bit single-precision floating-point number.</param>
        public void SetSingle(int index, float value) => SetUInt32(index, new Bytes64(value).vUInt32);

        /// <summary>Reads 64 bits starting at the specified bit array index as a double-precision floating-point value in big-endian order.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 64-bit double-precision floating-point number.</returns>
        public float GetSingleBigEndian(int index)
        {
            if (index < 0 || index > _Size - 64) throw new IndexOutOfRangeException("Index was outside; or reading beyond the bounds of the array.");
            return new Bytes64(GetByte(index + 24), GetByte(index + 16), GetByte(index + 8), GetByte(index)).vSingle;
        }

        /// <summary>Reads 64 bits starting at the specified bit array index as a double-precision floating-point value in big-endian order.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 64-bit double-precision floating-point number.</returns>
        public void SetSingleBigEndian(int index, float value)
        {
            var bytes = new Bytes64(value);
            SetByte(index, bytes.b3);
            SetByte(index + 8, bytes.b2);
            SetByte(index + 16, bytes.b1);
            SetByte(index + 24, bytes.b0);
        }

        /// <summary>Reads 64 bits starting at the specified bit array index as an unsigned integer.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 64-bit unsigned integer.</returns>
        public ulong GetUInt64(int index)
        {
            if (index < 0 || index > _Size - 64) throw new IndexOutOfRangeException("Index was outside; or reading beyond the bounds of the array.");
            return new Bytes64(GetByte(index), GetByte(index + 8), GetByte(index + 16), GetByte(index + 24), GetByte(index + 32), GetByte(index + 40), GetByte(index + 48), GetByte(index + 56)).vUInt64;
        }

        /// <summary>Writes 64 bits starting at the specified bit array index as an unsigned integer.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 64-bit unsigned integer.</param>
        public void SetUInt64(int index, ulong value)
        {
            var bytes = new Bytes64(value);
            SetByte(index, bytes.b0);
            SetByte(index + 8, bytes.b1);
            SetByte(index + 16, bytes.b2);
            SetByte(index + 24, bytes.b3);
            SetByte(index + 32, bytes.b4);
            SetByte(index + 40, bytes.b5);
            SetByte(index + 48, bytes.b6);
            SetByte(index + 56, bytes.b7);
        }

        /// <summary>Reads 64 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 64-bit unsigned integer.</returns>
        public ulong GetUInt64BigEndian(int index)
        {
            if (index < 0 || index > _Size - 64) throw new IndexOutOfRangeException("Index was outside; or reading beyond the bounds of the array.");
            return new Bytes64(GetByte(index + 56), GetByte(index + 48), GetByte(index + 40), GetByte(index + 32), GetByte(index + 24), GetByte(index + 16), GetByte(index + 8), GetByte(index)).vUInt64;
        }

        /// <summary>Writes 64 bits starting at the specified bit array index as an unsigned integer in big-endian order.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 64-bit unsigned integer.</param>
        public void SetUInt64BigEndian(int index, ulong value)
        {
            var bytes = new Bytes64(value);
            SetByte(index, bytes.b7);
            SetByte(index + 8, bytes.b6);
            SetByte(index + 16, bytes.b5);
            SetByte(index + 24, bytes.b4);
            SetByte(index + 32, bytes.b3);
            SetByte(index + 40, bytes.b2);
            SetByte(index + 48, bytes.b1);
            SetByte(index + 56, bytes.b0);
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
        public double GetDouble(int index) => new Bytes64(GetUInt64(index)).vDouble;

        /// <summary>Writes 64 bits starting at the specified bit array index as a double-precision floating-point value.</summary>
        /// <param name="index">The bit array index to start writing at.</param>
        /// <param name="value">The 64-bit double-precision floating-point number.</param>
        public void SetDouble(int index, double value) => SetUInt64(index, new Bytes64(value).vUInt64);

        /// <summary>Reads 64 bits starting at the specified bit array index as a double-precision floating-point value in big-endian order.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 64-bit double-precision floating-point number.</returns>
        public double GetDoubleBigEndian(int index)
        {
            if (index < 0 || index > _Size - 64) throw new IndexOutOfRangeException("Index was outside; or reading beyond the bounds of the array.");
            return new Bytes64(GetByte(index + 56), GetByte(index + 48), GetByte(index + 40), GetByte(index + 32), GetByte(index + 24), GetByte(index + 16), GetByte(index + 8), GetByte(index)).vDouble;
        }

        /// <summary>Reads 64 bits starting at the specified bit array index as a double-precision floating-point value in big-endian order.</summary>
        /// <param name="index">The bit array index to start reading at.</param>
        /// <returns>The 64-bit double-precision floating-point number.</returns>
        public void SetDoubleBigEndian(int index, double value)
        {
            var bytes = new Bytes64(value);
            SetByte(index, bytes.b7);
            SetByte(index + 8, bytes.b6);
            SetByte(index + 16, bytes.b5);
            SetByte(index + 24, bytes.b4);
            SetByte(index + 32, bytes.b3);
            SetByte(index + 40, bytes.b2);
            SetByte(index + 48, bytes.b1);
            SetByte(index + 56, bytes.b0);
        }

        /// <summary>
        /// Exposes 64 bits of memory as bytes that can be read as several different data types.
        /// <para>Inspired by: https://stackoverflow.com/a/59273138</para>
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        private struct Bytes64
        {
            [FieldOffset(0)] public byte b0;
            [FieldOffset(1)] public byte b1;
            [FieldOffset(2)] public byte b2;
            [FieldOffset(3)] public byte b3;
            [FieldOffset(4)] public byte b4;
            [FieldOffset(5)] public byte b5;
            [FieldOffset(6)] public byte b6;
            [FieldOffset(7)] public byte b7;

            [FieldOffset(0)] public ushort vUInt16;
            [FieldOffset(0)] public uint vUInt32;
            [FieldOffset(0)] public ulong vUInt64;
            [FieldOffset(0)] public float vSingle;
            [FieldOffset(0)] public double vDouble;

            public Bytes64(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7)
            {
                vUInt16 = 0; // required to be initialized by the C# compiler.
                vUInt32 = 0; // required to be initialized by the C# compiler.
                vUInt64 = 0; // required to be initialized by the C# compiler.
                vSingle = 0f; // required to be initialized by the C# compiler.
                vDouble = 0d; // required to be initialized by the C# compiler.

                this.b0 = b0;
                this.b1 = b1;
                this.b2 = b2;
                this.b3 = b3;
                this.b4 = b4;
                this.b5 = b5;
                this.b6 = b6;
                this.b7 = b7;
            }

            public Bytes64(byte b0, byte b1)
            {
                vUInt16 = 0; // required to be initialized by the C# compiler.
                vUInt32 = 0; // required to be initialized by the C# compiler.
                vUInt64 = 0; // required to be initialized by the C# compiler.
                vSingle = 0f; // required to be initialized by the C# compiler.
                vDouble = 0d; // required to be initialized by the C# compiler.
                b2 = 0; // required to be initialized by the C# compiler.
                b3 = 0; // required to be initialized by the C# compiler.
                b4 = 0; // required to be initialized by the C# compiler.
                b5 = 0; // required to be initialized by the C# compiler.
                b6 = 0; // required to be initialized by the C# compiler.
                b7 = 0; // required to be initialized by the C# compiler.

                this.b0 = b0;
                this.b1 = b1;
            }

            public Bytes64(byte b0, byte b1, byte b2, byte b3)
            {
                vUInt16 = 0; // required to be initialized by the C# compiler.
                vUInt32 = 0; // required to be initialized by the C# compiler.
                vUInt64 = 0; // required to be initialized by the C# compiler.
                vSingle = 0f; // required to be initialized by the C# compiler.
                vDouble = 0d; // required to be initialized by the C# compiler.
                b4 = 0; // required to be initialized by the C# compiler.
                b5 = 0; // required to be initialized by the C# compiler.
                b6 = 0; // required to be initialized by the C# compiler.
                b7 = 0; // required to be initialized by the C# compiler.

                this.b0 = b0;
                this.b1 = b1;
                this.b2 = b2;
                this.b3 = b3;
            }

            public Bytes64(ushort value)
            {
                vUInt32 = 0; // required to be initialized by the C# compiler.
                vUInt64 = 0; // required to be initialized by the C# compiler.
                vSingle = 0f; // required to be initialized by the C# compiler.
                vDouble = 0d; // required to be initialized by the C# compiler.
                b0 = 0; // required to be initialized by the C# compiler.
                b1 = 0; // required to be initialized by the C# compiler.
                b2 = 0; // required to be initialized by the C# compiler.
                b3 = 0; // required to be initialized by the C# compiler.
                b4 = 0; // required to be initialized by the C# compiler.
                b5 = 0; // required to be initialized by the C# compiler.
                b6 = 0; // required to be initialized by the C# compiler.
                b7 = 0; // required to be initialized by the C# compiler.

                vUInt16 = value;
            }

            public Bytes64(uint value)
            {
                vUInt16 = 0; // required to be initialized by the C# compiler.
                vUInt64 = 0; // required to be initialized by the C# compiler.
                vSingle = 0f; // required to be initialized by the C# compiler.
                vDouble = 0d; // required to be initialized by the C# compiler.
                b0 = 0; // required to be initialized by the C# compiler.
                b1 = 0; // required to be initialized by the C# compiler.
                b2 = 0; // required to be initialized by the C# compiler.
                b3 = 0; // required to be initialized by the C# compiler.
                b4 = 0; // required to be initialized by the C# compiler.
                b5 = 0; // required to be initialized by the C# compiler.
                b6 = 0; // required to be initialized by the C# compiler.
                b7 = 0; // required to be initialized by the C# compiler.

                vUInt32 = value;
            }

            public Bytes64(ulong value)
            {
                vUInt16 = 0; // required to be initialized by the C# compiler.
                vUInt32 = 0; // required to be initialized by the C# compiler.
                vSingle = 0f; // required to be initialized by the C# compiler.
                vDouble = 0d; // required to be initialized by the C# compiler.
                b0 = 0; // required to be initialized by the C# compiler.
                b1 = 0; // required to be initialized by the C# compiler.
                b2 = 0; // required to be initialized by the C# compiler.
                b3 = 0; // required to be initialized by the C# compiler.
                b4 = 0; // required to be initialized by the C# compiler.
                b5 = 0; // required to be initialized by the C# compiler.
                b6 = 0; // required to be initialized by the C# compiler.
                b7 = 0; // required to be initialized by the C# compiler.

                vUInt64 = value;
            }

            public Bytes64(float value)
            {
                vUInt16 = 0; // required to be initialized by the C# compiler.
                vUInt32 = 0; // required to be initialized by the C# compiler.
                vUInt64 = 0; // required to be initialized by the C# compiler.
                vDouble = 0d; // required to be initialized by the C# compiler.
                b0 = 0; // required to be initialized by the C# compiler.
                b1 = 0; // required to be initialized by the C# compiler.
                b2 = 0; // required to be initialized by the C# compiler.
                b3 = 0; // required to be initialized by the C# compiler.
                b4 = 0; // required to be initialized by the C# compiler.
                b5 = 0; // required to be initialized by the C# compiler.
                b6 = 0; // required to be initialized by the C# compiler.
                b7 = 0; // required to be initialized by the C# compiler.

                vSingle = value;
            }

            public Bytes64(double value)
            {
                vUInt16 = 0; // required to be initialized by the C# compiler.
                vUInt32 = 0; // required to be initialized by the C# compiler.
                vUInt64 = 0; // required to be initialized by the C# compiler.
                vSingle = 0f; // required to be initialized by the C# compiler.
                b0 = 0; // required to be initialized by the C# compiler.
                b1 = 0; // required to be initialized by the C# compiler.
                b2 = 0; // required to be initialized by the C# compiler.
                b3 = 0; // required to be initialized by the C# compiler.
                b4 = 0; // required to be initialized by the C# compiler.
                b5 = 0; // required to be initialized by the C# compiler.
                b6 = 0; // required to be initialized by the C# compiler.
                b7 = 0; // required to be initialized by the C# compiler.

                vDouble = value;
            }
        }

        #endregion Setting and Getting Bytes, Integers and Floats

        #region IReadOnlyCollection<bool> Implementation

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

        #endregion IReadOnlyCollection<bool> Implementation

        #region ICloneable Implementation

        public object Clone()
        {
            return new BitArray(this);
        }

        #endregion ICloneable Implementation
    }
}