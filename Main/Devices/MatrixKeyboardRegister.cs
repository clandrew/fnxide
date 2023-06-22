using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    /*
     * 
VIA_PRA  =  $db01  ; CIA#1 (Port Register A)
VIA_DDRA =  $db03  ; CIA#1 (Data Direction Register A)

VIA_PRB  =  $db00  ; CIA#1 (Port Register B)
VIA_DDRB =  $db02  ; CIA#1 (Data Direction Register B)


        public const int MATRIX_KEYBOARD_VIA_PORT_B = 0xDB00;
        public const int MATRIX_KEYBOARD_VIA_PORT_A = 0xDB01;
        public const int MATRIX_KEYBOARD_VIA_DATA_DIRECTION_B = 0xDB02;
        public const int MATRIX_KEYBOARD_VIA_DATA_DIRECTION_A = 0xDB03;
     */

    public class MatrixKeyboardRegister : MemoryLocations.MemoryRAM
    {
        public MatrixKeyboardRegister(int StartAddress, int Length) : base(StartAddress, Length)
        {
            System.Diagnostics.Debug.Assert(Length == 4);
        }

        public override void WriteByte(int Address, byte Value)
        {
            data[Address] = Value;
        }
        public override byte ReadByte(int Address)
        {
            if (Address == 1) // Port register A
            {
                byte pb = data[0]; // Port register B
                byte ddrb = data[2];
                byte ddra = data[3];

                if (pb == (1 << 4 ^ 0xFF) &&
                    ddrb == 0xFF &&
                    ddra == 0x0 &&
                    SpacePressed)
                {
                    return (1 << 7) ^ 0xFF;
                }
            }
            return data[Address];
        }

        public bool SpacePressed;
    }
}
