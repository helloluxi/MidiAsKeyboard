using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using NAudio.Midi;


class Program
{    
    [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
    public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

    
    static void Press(KeyCode key) => keybd_event((byte)key, 0, 0, 0);
    static void Release(KeyCode key) => keybd_event((byte)key, 0, 2, 0);
    public class MidiAction{
        public Action midiPress, midiRelease;
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

        JsonDocument document = JsonDocument.Parse(System.IO.File.ReadAllText("config.json"));
        bool playback = document.RootElement.GetProperty("playback").GetBoolean();
        int patch = document.RootElement.GetProperty("patch").GetInt32();
        var mapping = new MidiAction[128];
        foreach(JsonProperty element in document.RootElement.GetProperty("mappings").EnumerateObject()){
            int note = NoteName.Parse(element.Name);
            KeyCode[][] keys = element.Value.GetString().Split(' ').Select(
                s => s.Split('+').Select(ss => Enum.Parse<KeyCode>(ss)).ToArray()
            ).ToArray();
            if(keys.Length == 1){
                mapping[note] = new MidiAction{
                    midiPress = () => Array.ForEach(keys[0], Press),
                    midiRelease = () => {
                        foreach(KeyCode key in keys[0].Reverse())
                            Release(key);
                    }
                };
            }
            else
            {
                mapping[note] = new MidiAction{
                    midiPress = () => {
                        foreach(KeyCode[] keys in keys){
                            foreach(KeyCode key in keys)
                                Press(key);
                            foreach(KeyCode key in keys.Reverse())
                                Release(key);
                        }
                    },
                    midiRelease = () => {}
                };
            }
        }

        using var midiIn = new MidiIn(0);
        using var midiOut = new MidiOut(0);

        // Set instrument <https://www.midi.org/specifications-old/item/gm-level-1-sound-set>
        midiOut.Send(MidiMessage.ChangePatch(patch, 1).RawData);
        
        midiIn.MessageReceived += (sender, e) => {
            if(e.MidiEvent is NoteEvent n){
                // Console.WriteLine(n);
                if(playback)
                    midiOut.Send(n.GetAsShortMessage());
                if(mapping[n.NoteNumber] != null){
                    if(n.CommandCode == MidiCommandCode.NoteOn)
                        mapping[n.NoteNumber].midiPress();
                    else if(n.CommandCode == MidiCommandCode.NoteOff)
                        mapping[n.NoteNumber].midiRelease();
                }
            }
        };
        midiIn.ErrorReceived += (sender, e) => {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e);
            Console.ResetColor();
        };
        midiIn.Start();

        while(true) Console.ReadKey();
    }
}