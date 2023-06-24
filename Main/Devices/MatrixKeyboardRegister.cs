﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoenixIDE.Basic;

namespace FoenixIDE.Simulator.Devices
{
    public class MatrixKeyboardRegister
    {
        public class VIA0Range : MemoryLocations.MemoryRAM
        {
            public VIA0Range(int StartAddress, int Length) : base(StartAddress, Length)
            {
                System.Diagnostics.Debug.Assert(Length == 4);
            }
            public byte VIA0_PRA { get { return data[1]; } set { data[1] = value; } }
            public byte VIA0_PRB { get { return data[0]; } set { data[0] = value; } }

            public byte VIA0_DDRA { get { return data[3]; } set { data[3] = value; } }
            public byte VIA0_DDRB { get { return data[2]; } set { data[2] = value; } }
        }

        public class VIA1Range : MemoryLocations.MemoryRAM
        {
            public VIA1Range(MatrixKeyboardRegister m, int StartAddress, int Length) : base(StartAddress, Length)
            {
                System.Diagnostics.Debug.Assert(Length == 4);
                matrix = m;
            }

            public override void WriteByte(int Address, byte Value)
            {
                data[Address] = Value;

                // Handle side effects
                if (Address == 1) // Write to port A.
                {
                    if (Value == (1 << 0 ^ 0xFF)) // PA0
                    {
                        VIA1_PRB = 0;
                        VIA1_PRB |= (1 << 0); // delete key, unmapped
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_enter] ? (byte)0 : (byte)(1 << 1);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_left_arrow] ? (byte)0 : (byte)(1 << 2);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_F7] ? (byte)0 : (byte)(1 << 3);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_F1] ? (byte)0 : (byte)(1 << 4);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_F3] ? (byte)0 : (byte)(1 << 5);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_F5] ? (byte)0 : (byte)(1 << 6);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_up_arrow] ? (byte)0 : (byte)(1 << 7);

                        // Also set VIA0
                        matrix.VIA0.VIA0_PRB = 0;
                        matrix.VIA0.VIA0_PRB |= (1 << 0);
                        matrix.VIA0.VIA0_PRB |= (1 << 1);
                        matrix.VIA0.VIA0_PRB |= (1 << 2);
                        matrix.VIA0.VIA0_PRB |= (1 << 3);
                        matrix.VIA0.VIA0_PRB |= (1 << 4);
                        matrix.VIA0.VIA0_PRB |= (1 << 5);
                        matrix.VIA0.VIA0_PRB |= (1 << 6);
                        matrix.VIA0.VIA0_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_down_arrow]? (byte)0 : (byte)(1 << 7);
                    }
                    else if (Value == (1 << 1 ^ 0xFF)) // PA1
                    {
                        VIA1_PRB = 0;
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_3] ? (byte)0 : (byte)(1 << 0);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_w] ? (byte)0 : (byte)(1 << 1);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_a] ? (byte)0 : (byte)(1 << 2);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_4] ? (byte)0 : (byte)(1 << 3);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_z] ? (byte)0 : (byte)(1 << 4);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_s] ? (byte)0 : (byte)(1 << 5);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_e] ? (byte)0 : (byte)(1 << 6);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_shiftLeft] ? (byte)0 : (byte)(1 << 7);
                    }
                    else if (Value == (1 << 2 ^ 0xFF)) // PA2
                    {
                        VIA1_PRB = 0;
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_5] ? (byte)0 : (byte)(1 << 0);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_r] ? (byte)0 : (byte)(1 << 1);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_d] ? (byte)0 : (byte)(1 << 2);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_6] ? (byte)0 : (byte)(1 << 3);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_c] ? (byte)0 : (byte)(1 << 4);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_f] ? (byte)0 : (byte)(1 << 5);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_t] ? (byte)0 : (byte)(1 << 6);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_x] ? (byte)0 : (byte)(1 << 7);
                    }
                    else if (Value == (1 << 3 ^ 0xFF)) // PA3
                    {
                        VIA1_PRB = 0;
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_7] ? (byte)0 : (byte)(1 << 0);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_y] ? (byte)0 : (byte)(1 << 1);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_g] ? (byte)0 : (byte)(1 << 2);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_8] ? (byte)0 : (byte)(1 << 3);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_b] ? (byte)0 : (byte)(1 << 4);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_h] ? (byte)0 : (byte)(1 << 5);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_u] ? (byte)0 : (byte)(1 << 6);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_v] ? (byte)0 : (byte)(1 << 7);
                    }
                    else if (Value == (1 << 4 ^ 0xFF)) // PA4
                    {
                        VIA1_PRB = 0;
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_9] ? (byte)0 : (byte)(1 << 0);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_i] ? (byte)0 : (byte)(1 << 1);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_j] ? (byte)0 : (byte)(1 << 2);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_0] ? (byte)0 : (byte)(1 << 3);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_m] ? (byte)0 : (byte)(1 << 4);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_k] ? (byte)0 : (byte)(1 << 5);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_o] ? (byte)0 : (byte)(1 << 6);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_n] ? (byte)0 : (byte)(1 << 7);
                    }
                    else if (Value == (1 << 5 ^ 0xFF)) // PA5
                    {
                        VIA1_PRB = 0;
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_minus] ? (byte)0 : (byte)(1 << 0);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_p] ? (byte)0 : (byte)(1 << 1);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_l] ? (byte)0 : (byte)(1 << 2);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_capslock] ? (byte)0 : (byte)(1 << 3);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_period] ? (byte)0 : (byte)(1 << 4);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_semicolon] ? (byte)0 : (byte)(1 << 5);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_bracketLeft] ? (byte)0 : (byte)(1 << 6);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_comma] ? (byte)0 : (byte)(1 << 7);
                    }
                    else if (Value == (1 << 6 ^ 0xFF)) // PA6
                    {
                        VIA1_PRB = 0;
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_equals] ? (byte)0 : (byte)(1 << 0);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_bracketRight] ? (byte)0 : (byte)(1 << 1);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_semicolon] ? (byte)0 : (byte)(1 << 2);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_backslash] ? (byte)0 : (byte)(1 << 3); // A backslash\ is mapped to the HOME key
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_shiftRight] ? (byte)0 : (byte)(1 << 4);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_altLeft] ? (byte)0 : (byte)(1 << 5);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_tab] ? (byte)0 : (byte)(1 << 6);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_slash] ? (byte)0 : (byte)(1 << 7);

                        matrix.VIA0.VIA0_PRB = 0;
                        matrix.VIA0.VIA0_PRB |= (1 << 0);
                        matrix.VIA0.VIA0_PRB |= (1 << 1);
                        matrix.VIA0.VIA0_PRB |= (1 << 2);
                        matrix.VIA0.VIA0_PRB |= (1 << 3);
                        matrix.VIA0.VIA0_PRB |= (1 << 4);
                        matrix.VIA0.VIA0_PRB |= (1 << 5);
                        matrix.VIA0.VIA0_PRB |= (1 << 6);
                        matrix.VIA0.VIA0_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_right_arrow] ? (byte)0 : (byte)(1 << 7);
                    }
                    else if (Value == (1 << 7 ^ 0xFF)) // PA7
                    {
                        VIA1_PRB = 0;
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_1] ? (byte)0 : (byte)(1 << 0);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_backspace] ? (byte)0 : (byte)(1 << 1);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_controlLeft] ? (byte)0 : (byte)(1 << 2);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_2] ? (byte)0 : (byte)(1 << 3);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_space] ? (byte)0 : (byte)(1 << 4);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_grave] ? (byte)0 : (byte)(1 << 5); // A backtick` is mapped to the Foenix key
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_q] ? (byte)0 : (byte)(1 << 6);
                        VIA1_PRB |= matrix.scanCodeBuffer[(int)ScanCode.sc_escape] ? (byte)0 : (byte)(1 << 7); // Escape is mapped to RUN/STOP
                    }
                    else
                    {
                        VIA1_PRB = 0xFF;
                    }
                }
            }
            public override byte ReadByte(int Address)
            {
                if (Address == 0) // Port B
                {
                    bool canReadPortB = VIA1_DDRB == 0;
                    if (!canReadPortB)
                    {
                        return 0;
                    }
                }

                return data[Address];
            }
            byte VIA1_PRA { get { return data[1]; } set { data[1] = value; } }
            byte VIA1_PRB { get { return data[0]; } set { data[0] = value; } }

            byte VIA1_DDRA { get { return data[3]; } set { data[3] = value; } }
            byte VIA1_DDRB { get { return data[2]; } set { data[2] = value; } }

            MatrixKeyboardRegister matrix;
        }

        public MatrixKeyboardRegister(int Range0StartAddress, int Range0Length, int Range1StartAddress, int Range1Length)
        {
            System.Diagnostics.Debug.Assert(Range0Length == 4);
            scanCodeBuffer = new bool[(int)ScanCode.sc_down_arrow + 1];
            VIA0 = new VIA0Range(Range0StartAddress, Range0Length);
            VIA1 = new VIA1Range(this, Range1StartAddress, Range1Length);
        }

        public VIA0Range VIA0; // Used for the right and down arrow keys
        public VIA1Range VIA1; // Used for all the other keys


        public void WriteScanCode(ScanCode sc)
        {
            int scn = (int)sc;

            if (scn < scanCodeBuffer.Length)
            {
                scanCodeBuffer[scn] = true;
            }
            else
            {
                scn -= 0x80;
                scanCodeBuffer[scn] = false;
            }
        }
        bool[] scanCodeBuffer;

    }
}
