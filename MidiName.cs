public static class NoteName
{
    public const int C2 = 24;
    public const int Cs2 = 25;
    public const int Db2 = 25;
    public const int D2 = 26;
    public const int Ds2 = 27;
    public const int Eb2 = 27;
    public const int E2 = 28;
    public const int F2 = 29;
    public const int Fs2 = 30;
    public const int Gb2 = 30;
    public const int G2 = 31;
    public const int Gs2 = 32;
    public const int Ab2 = 32;
    public const int A2 = 33;
    public const int As2 = 34;
    public const int Bb2 = 34;
    public const int B2 = 35;
    public const int C3 = 36;
    public const int Cs3 = 37;
    public const int Db3 = 37;
    public const int D3 = 38;
    public const int Ds3 = 39;
    public const int Eb3 = 39;
    public const int E3 = 40;
    public const int F3 = 41;
    public const int Fs3 = 42;
    public const int Gb3 = 42;
    public const int G3 = 43;
    public const int Gs3 = 44;
    public const int Ab3 = 44;
    public const int A3 = 45;
    public const int As3 = 46;
    public const int Bb3 = 46;
    public const int B3 = 47;
    public const int C4 = 48;
    public const int Cs4 = 49;
    public const int Db4 = 49;
    public const int D4 = 50;
    public const int Ds4 = 51;
    public const int Eb4 = 51;
    public const int E4 = 52;
    public const int F4 = 53;
    public const int Fs4 = 54;
    public const int Gb4 = 54;
    public const int G4 = 55;
    public const int Gs4 = 56;
    public const int Ab4 = 56;
    public const int A4 = 57;
    public const int As4 = 58;
    public const int Bb4 = 58;
    public const int B4 = 59;
    public const int C5 = 60;
    public const int Cs5 = 61;
    public const int Db5 = 61;
    public const int D5 = 62;
    public const int Ds5 = 63;
    public const int Eb5 = 63;
    public const int E5 = 64;
    public const int F5 = 65;
    public const int Fs5 = 66;
    public const int Gb5 = 66;
    public const int G5 = 67;
    public const int Gs5 = 68;
    public const int Ab5 = 68;
    public const int A5 = 69;
    public const int As5 = 70;
    public const int Bb5 = 70;
    public const int B5 = 71;
    public const int C6 = 72;
    public const int Cs6 = 73;
    public const int Db6 = 73;
    public const int D6 = 74;
    public const int Ds6 = 75;
    public const int Eb6 = 75;
    public const int E6 = 76;
    public const int F6 = 77;
    public const int Fs6 = 78;
    public const int Gb6 = 78;
    public const int G6 = 79;
    public const int Gs6 = 80;
    public const int Ab6 = 80;
    public const int A6 = 81;
    public const int As6 = 82;
    public const int Bb6 = 82;
    public const int B6 = 83;
    public const int C7 = 84;



    private static readonly string[] NoteNames = new string[] {
        "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"
    };
    public static string GetMidiName(int noteNumber) =>
        $"{NoteNames[noteNumber % 12]}{noteNumber / 12 - 1}";
    private static readonly Dictionary<string, int> NoteNameToNumber = new Dictionary<string, int> {
        {"C" , 0},
        {"C#", 1}, {"#C", 1}, {"Db", 1}, {"bD", 1},
        {"D" , 2},
        {"D#", 3}, {"#D", 3}, {"Eb", 3}, {"bE", 3},
        {"E" , 4},
        {"F" , 5},
        {"F#", 6}, {"#F", 6}, {"Gb", 6}, {"bG", 6},
        {"G" , 7},
        {"G#", 8}, {"#G", 8}, {"Ab", 8}, {"bA", 8},
        {"A" , 9},
        {"A#", 10}, {"#A", 10}, {"Bb", 10}, {"bB", 10},
        {"B" , 11}
    };
    public static int Parse(string name){
        var noteName = name.Substring(0, name.Length - 1);
        var octave = int.Parse(name.Substring(name.Length - 1));
        return NoteNameToNumber[noteName] + (octave + 1) * 12;
    }
}