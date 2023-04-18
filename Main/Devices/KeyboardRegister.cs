using FoenixIDE.Basic;
using System;

namespace FoenixIDE.Simulator.Devices
{
    public class KeyboardRegister: MemoryLocations.MemoryRAM
    {
        private bool mouseDevice = false;
        private bool breakKey = false;
        private byte ps2PacketCntr = 0;
        private int packetLength = 0;
        private byte[] ps2packet = new byte[6];
        public delegate void TriggerInterruptDelegate();
        public TriggerInterruptDelegate TriggerKeyboardInterrupt;
        public TriggerInterruptDelegate TriggerMouseInterrupt;
        public FoenixIDE.UI.CPUWindow.CPULogger CPULogger;

        public KeyboardRegister(int StartAddress, int Length) : base(StartAddress, Length)
        {
        }

        // This is used to simulate the Keyboard Register
        public override void WriteByte(int Address, byte Value)
        {
            CPULogger.Log("Keyboard[000]: Start WriteByte Address: " + Address + " Value: " + Value);
            CPULogger.Log("Keyboard[001]:     data[Address] was " + data[Address]);
            CPULogger.Log("Keyboard[002]:     data[Address] = Value;");
            data[Address] = Value;
            CPULogger.Log("Keyboard[003]:     data[Address] became " + data[Address]);

            switch (Address)
            {
                case 0:
                    switch (Value)
                    {
                        case 0x69:
                            CPULogger.Log("Keyboard[004]:     data[4] was " + data[4]);
                            CPULogger.Log("Keyboard[005]:     data[4] = 1;");
                            data[4] = 1;
                            break;
                        case 0xEE: // echo command
                            CPULogger.Log("Keyboard[006]:     data[4] was " + data[4]);
                            CPULogger.Log("Keyboard[007]:     data[4] = 1;");
                            data[4] = 1;
                            break;
                        case 0xF4:
                            CPULogger.Log("Keyboard[008]:     data[0] was " + data[0]);
                            CPULogger.Log("Keyboard[009]:     data[0] = 0xFA;");
                            data[0] = 0xFA;

                            CPULogger.Log("Keyboard[010]:     data[4] was " + data[4]);
                            CPULogger.Log("Keyboard[011]:     data[4] = 1;");
                            data[4] = 1;
                            break;
                        case 0xF6:

                            CPULogger.Log("Keyboard[012]:     data[4] was " + data[4]);
                            CPULogger.Log("Keyboard[013]:     data[4] = 1;");
                            data[4] = 1;
                            break;
                    }
                    break;
                case 4:
                    switch (Value)
                    {
                        case 0x60:

                            break;
                        case 0xA9:
                            CPULogger.Log("Keyboard[014]:     data[0] was " + data[0]);
                            CPULogger.Log("Keyboard[015]:     data[0] = 0;");
                            data[0] = 0;
                            break;
                        case 0xAA: // self test
                            CPULogger.Log("Keyboard[016]:     data[0] was " + data[0]);
                            CPULogger.Log("Keyboard[017]:     data[0] = 0x55;");
                            data[0] = 0x55;

                            CPULogger.Log("Keyboard[018]:     data[4] was " + data[4]);
                            CPULogger.Log("Keyboard[019]:     data[4] = 1;");
                            data[4] = 1;
                            break;
                        case 0xAB: // keyboard test
                            CPULogger.Log("Keyboard[020]:     data[0] was " + data[0]);
                            CPULogger.Log("Keyboard[021]:     data[0] = 0;");
                            data[0] = 0;

                            CPULogger.Log("Keyboard[022]:     data[4] was " + data[4]);
                            CPULogger.Log("Keyboard[023]:     data[4] = 1;");
                            data[4] = 1;
                            break;
                        case 0xAD: // disable keyboard
                            CPULogger.Log("Keyboard[024]:     data[0] was " + data[0]);
                            CPULogger.Log("Keyboard[025]:     data[0] = 0;");
                            data[0] = 0;

                            CPULogger.Log("Keyboard[026]:     data[1] was " + data[1]);
                            CPULogger.Log("Keyboard[027]:     data[1] = 10;");
                            data[1] = 0;
                            break;
                        case 0xAE: // re-enabled sending data
                            CPULogger.Log("Keyboard[028]:     data[4] was " + data[4]);
                            CPULogger.Log("Keyboard[029]:     data[4] = 1;");
                            data[4] = 1;
                            break;
                        case 0xFF:  // reset 
                            CPULogger.Log("Keyboard[030]:     data[4] was " + data[4]);
                            CPULogger.Log("Keyboard[031]:     data[4] = 0xAA;");
                            data[4] = 0xAA;
                            break;
                        case 0x20:
                            CPULogger.Log("Keyboard[032]:     data[4] was " + data[4]);
                            CPULogger.Log("Keyboard[033]:     data[4] = 1;");
                            data[4] = 1;
                            break;
                        case 0xD4:
                            CPULogger.Log("Keyboard[034]:     data[4] was " + data[4]);
                            CPULogger.Log("Keyboard[035]:     data[4] = 1;");
                            data[4] = 1;
                            break;

                    }
                    
                    break;
            }
            CPULogger.Log("Keyboard[03x]: Done WriteByte.");
        }

        public override byte ReadByte(int Address)
        {
            CPULogger.Log("Keyboard[03x]: Start ReadByte " + Address);

            // Whenever the buffer is read, set the buffer to empty.
            if (Address == 0)
            {
                if (!mouseDevice && !breakKey)
                {
                    CPULogger.Log("Keyboard[036]:     data[4] was " + data[4]);
                    CPULogger.Log("Keyboard[037]:     data[4] = 0;");
                    data[4] = 0;
                    CPULogger.Log("Keyboard[038]:     data[4] became zero.");
                }
                else if (packetLength != 0)
                {
                    // raise interrupt
                    if (mouseDevice)
                    {
                        // send the next byte in the packet
                        CPULogger.Log("Keyboard[039]:     data[4] was " + data[4]);
                        CPULogger.Log("Keyboard[040]:     data[4] = ps2packet[ps2PacketCntr++];");
                        data[4] = ps2packet[ps2PacketCntr++];
                        CPULogger.Log("Keyboard[041]:     data[4] became " + data[4]);

                        TriggerMouseInterrupt();
                        CPULogger.Log("Keyboard[041]:     ps2PacketCntr is " + ps2PacketCntr);
                        CPULogger.Log("Keyboard[041]:     packetLength is " + packetLength);
                        if (ps2PacketCntr == packetLength)
                        {
                            CPULogger.Log("Keyboard[041]:     ps2PacketCntr and  packetLength are being set to 0.");
                            ps2PacketCntr = 0;
                            mouseDevice = false;
                            packetLength = 0;
                        }
                    } 
                    else if (breakKey)  // this doesn't work yet
                    {
                        // send the next byte in the packet
                        CPULogger.Log("Keyboard[042]:     data[0] was " + data[0]);
                        CPULogger.Log("Keyboard[043]:     data[0] = ps2packet[ps2PacketCntr++];");
                        data[0] = ps2packet[ps2PacketCntr++];
                        CPULogger.Log("Keyboard[044]:     data[0] became " + data[0]);

                        CPULogger.Log("Keyboard[045]:     data[4] was " + data[4]);
                        CPULogger.Log("Keyboard[046]:     data[4] = 0;");
                        data[4] = 0;
                        CPULogger.Log("Keyboard[047]:     data[4] became zero.");

                        TriggerKeyboardInterrupt();
                        if (ps2PacketCntr == packetLength)
                        {
                            ps2PacketCntr = 0;
                            breakKey = false;
                            packetLength = 0;
                        }
                    }
                }
                CPULogger.Log("Keyboard[048]:     returning data[0] = " + data[0]);
                CPULogger.Log("Keyboard[049]: Done ReadByte.");
                return data[0];
            }
            else if (Address == 5)
            {
                CPULogger.Log("Keyboard[050]:     returning 0.");
                CPULogger.Log("Keyboard[051]: Done ReadByte.");
                return 0;
            }
            CPULogger.Log("Keyboard[052]: Done ReadByte.");
            return data[Address];
        }
        public void WriteScanCodeSequence(byte[] codes, int seqLength)
        {
            CPULogger.Log("Keyboard[053]: Start WriteScanCodeSequence " + seqLength);

            breakKey = true;
            data[0] = codes[0];
            data[4] = 0;
            ps2PacketCntr = 1;
            packetLength = seqLength;
            Array.Copy(codes, ps2packet, seqLength);

            TriggerKeyboardInterrupt?.Invoke();
            CPULogger.Log("Keyboard[054]: Done WriteScanCodeSequence.");
        }
        
        public void MousePackets(byte buttons, byte X, byte Y)
        {
            mouseDevice = true;
            data[0] = buttons;
            ps2PacketCntr = 1;
            packetLength = 3;
            ps2packet[0] = buttons;
            ps2packet[1] = X;
            ps2packet[2] = Y;

            TriggerMouseInterrupt?.Invoke();
        }
        
    }
}
