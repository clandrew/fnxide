using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    public class MatrixKeyboardRegister : MemoryLocations.MemoryRAM
    {
        public MatrixKeyboardRegister(int StartAddress, int Length) : base(StartAddress, Length)
        {
            System.Diagnostics.Debug.Assert(Length == 4);
        }

        public override void WriteByte(int Address, byte Value)
        {
            if (Address == 0) // Writing to port register B affects the value next read from port register A.
            {
                if ((Value & (1 << 4 ^ 0xFF)) != 0) // If we're asked to clear the corresponding bit
                {
                    bool isPortBWriteAllowed = (DDRB & (1 << 4)) != 0; 
                    if (isPortBWriteAllowed)
                    {
                        PRB &= 1 << 4 ^ 0xFF; // Clear corresponding reg bit

                        // Handle side-effects of write
                        byte maskA = 1 << 7;

                        // Set bits 0 through 6 of A by default
                        PRA |= 0x7F;

                        if (SpacePressed)
                        {
                            PRA &= (byte)(maskA ^ 0xFF); // Clear the corresponding bit of A
                        }
                        else
                        {
                            PRA |= maskA; // Set the corresponding bit of A
                        }
                    }
                }
            }
            else
            {
                data[Address] = Value;
            }
        }
        public override byte ReadByte(int Address)
        {
            if (Address == 1) // Port A
            {
                bool canReadPortA = DDRA == 0;
                if (!canReadPortA)
                {
                    return 0;
                }
            }

            return data[Address];
        }

        byte PRA { get { return data[1]; } set { data[1] = value; } }
        byte PRB { get { return data[0]; } set { data[0] = value; } }

        byte DDRA { get { return data[3]; } set { data[3] = value; } }
        byte DDRB { get { return data[2]; } set { data[2] = value; } }

        public bool SpacePressed;
    }
}
