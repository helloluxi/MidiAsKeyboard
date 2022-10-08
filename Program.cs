using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using NAudio.Midi;


class Program
{    
    [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
    public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
    
    static void Press(Keys key) => keybd_event((byte)key, 0, 0, 0);
    static void Release(Keys key) => keybd_event((byte)key, 0, 2, 0);
    static void Click(Keys key)
    {
        Press(key); Release(key);
        System.Console.WriteLine($"Click {key}");
    }
    static void Sequence(params Keys[] control)
    {
        Array.ForEach(control, Press);
        for (int i = control.Length - 1; i >= 0 ; i--)
        {
            Release(control[i]);
        }
        System.Console.WriteLine("Click " + string.Join('+', control));
    }

    static void Main(string[] args)
    {
        if(MidiIn.NumberOfDevices == 0){
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No device found");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Using Device {MidiIn.DeviceInfo(0).ProductName}");
        Console.ResetColor();
        Console.WriteLine();

        var mapping = new Keys[128];
        
        mapping[NoteName.Gs3] = Keys.Esc;
        mapping[NoteName.As3] = Keys.Tab;
        mapping[NoteName.C4]  = Keys.LCtrl;
        mapping[NoteName.Cs4]  = Keys.X;
        mapping[NoteName.D4]  = Keys.Space;
        mapping[NoteName.Ds4]  = Keys.R;
        mapping[NoteName.E4]  = Keys.C;
        mapping[NoteName.Fs4] = Keys.E;

        mapping[NoteName.C5]  = Keys.Q;
        mapping[NoteName.Cs5] = Keys.Z;
        mapping[NoteName.D5]  = Keys.S;
        mapping[NoteName.E5]  = Keys.D;

        mapping[NoteName.C6]  = Keys.Left;
        mapping[NoteName.Cs6] = Keys.Up;
        mapping[NoteName.D6]  = Keys.Down;
        mapping[NoteName.Ds6] = Keys.Shift;
        mapping[NoteName.E6]  = Keys.Right;

        using var midiIn = new MidiIn(0);
        using var midiOut = new MidiOut(0);
        midiIn.MessageReceived += (sender, e) => {
            if(e.MidiEvent is NoteEvent n){
                // Console.WriteLine(n);
                midiOut.Send(n.GetAsShortMessage());
                if(mapping[n.NoteNumber] != Keys.None){
                    if(n.Velocity > 0)
                        Press(mapping[n.NoteNumber]);
                    else
                        Release(mapping[n.NoteNumber]);
                }
            }
        };
        midiIn.ErrorReceived += (sender, e) => {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e);
            Console.ResetColor();
        };
        midiIn.Start();

        Console.ReadKey();
    }
}