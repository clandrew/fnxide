using FoenixIDE.Simulator.FileFormat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Simulator.Devices
{
    public class CodecRAM: MemoryLocations.MemoryRAM
    {
        public CodecRAM(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        public override async void WriteByte(int Address, byte Value)
        {
            // TODO: Add implementation here. 
            // In particular, setting byte 2 (CODEC_WR_CTRL) to 1 causes command high and low to be 
            // issued to the chip.

            data[Address] = Value;

            // Set CODEC_WR_CTRL=0, indicating the write is finished.
            // A more accurate implementation will need to set this after some amount of emulated time has elapsed 
            // to convey the asynchronous nature of the write.
            // Applications should be able to read back CODEC_WR_CTRL to know if the codec is "busy", so it's
            // arguably not accurate to behave as if the operation happened instantaneously.
            data[2] = 0;
        }
    }
}
