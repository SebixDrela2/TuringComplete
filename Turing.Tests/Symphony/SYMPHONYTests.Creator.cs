using System.Runtime.InteropServices;
using Turing.Core.Computer.Symphony;

namespace Turing.Tests.Symphony;

internal partial class SYMPHONYTests
{
    private SYMPHONY RunOverture(Byte[] instructions, params IEnumerable<Byte> inputs)
    {
        var cpu = new SYMPHONY(instructions);

        var en = inputs.GetEnumerator();
        cpu.Input = en.MoveNext() ? en.Current : default;

        while (true)
        {
            cpu.EVal();

            if (cpu.InputPin)
            {
                cpu.Input = en.MoveNext() ? en.Current : default;
            }

            if (cpu.OffPin || cpu.OutputPin)
            {
                return cpu;
            }
        }
    }

    private static class Instruction
    {
        private static Dictionary<string, int> _labelDict = [];

        public static IReadOnlyList<Byte> Parse(string instructionList)
        {
            var instructions = new List<int>();

            foreach(var line in instructionList.AsSpan().EnumerateLines())
            {
                var lineSplit = line.ToString().Split(' ', StringSplitOptions.TrimEntries);

                switch(lineSplit)
                {
                    case ["nop"]:
                        instructions.Add(0b_00000000_00000000_00000000_00000000);
                        continue;
                    case ["in", var val]:
                        instructions.Add(0b_00000001_00000000_00000000_00000000 | Register(val) << 24);
                        continue;
                    case ["out", var val and [< '0' or  > '9', ..]]:
                        instructions.Add(0b_00000010_00000000_00000000_00000000 | Register(val) << 12);
                        continue;
                    case ["out", var val]:
                        instructions.Add(0b_00010010_00000000_00000000_00000000 | Value(val));
                        continue;
                    case ["counter", var val]:
                        instructions.Add(0b_00000111_00000000_00000000_00000000 | Register(val) << 24);
                        continue;
                    case ["nand", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                        instructions.Add(0b_00100000_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | Register(arg3) << 12);
                        continue;
                    case ["or", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                        instructions.Add(0b_00100001_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | Register(arg3) << 12);
                        continue;
                    case ["and", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                        instructions.Add(0b_00100010_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | Register(arg3) << 12);
                        continue;
                    case ["nor", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                        instructions.Add(0b_00100011_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | Register(arg3) << 12);
                        continue;
                    case ["add", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                        instructions.Add(0b_00100100_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | Register(arg3) << 12);
                        continue;
                    case ["sub", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                        instructions.Add(0b_00100101_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | Register(arg3) << 12);
                        continue;
                    case ["xor", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                        instructions.Add(0b_00100110_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | Register(arg3) << 12);
                        continue;
                    case ["lsl", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                        instructions.Add(0b_00100111_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | Register(arg3) << 12);
                        continue;
                    case ["lsr", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                        instructions.Add(0b_00101000_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | Register(arg3) << 12);
                        continue;
                    case ["asr", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                        instructions.Add(0b_00101001_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | Register(arg3) << 12);
                        continue;
                    case ["cmp", [.. var arg1, ','], var arg2] when IsRegister(arg2):
                        instructions.Add(0b_00101010_11110000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 12);
                        continue;
                    case ["nand", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                        instructions.Add(0b_00110000_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | ValueLabel(arg3));
                        continue;
                    case ["or", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                        instructions.Add(0b_00110001_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | ValueLabel(arg3));
                        continue;
                    case ["nor", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                        instructions.Add(0b_00110011_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | ValueLabel(arg3));
                        continue;
                    case ["and", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                        instructions.Add(0b_00110010_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | ValueLabel(arg3));
                        continue;
                    case ["add", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                        instructions.Add(0b_00110100_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | ValueLabel(arg3));
                        continue;
                    case ["sub", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                        instructions.Add(0b_00110101_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | ValueLabel(arg3));
                        continue;
                    case ["xor", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                        instructions.Add(0b_00110110_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | ValueLabel(arg3));
                        continue;
                    case ["xor", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                        instructions.Add(0b_00110110_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | ValueLabel(arg3));
                        continue;
                    case ["lsl", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                        instructions.Add(0b_00110111_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | ValueLabel(arg3));
                        continue;
                    case ["lsr", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                        instructions.Add(0b_00111000_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | ValueLabel(arg3));
                        continue;
                    case ["asr", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                        instructions.Add(0b_00111001_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 20 | ValueLabel(arg3));
                        continue;
                    case ["cmp", [.. var arg1, ','], var arg2] when IsImmiediate(arg2):
                        instructions.Add(0b_00111010_11110000_00000000_00000000 | Register(arg1) << 20 | int.Parse(arg2));
                        continue;
                    case ["jmp", var arg1] when IsRegister(arg1):
                        instructions.Add(0b_01001000_00001111_00000000_00000000 | Register(arg1) << 12);
                        continue;
                    case ["jmp", var arg1] when IsLabelOrImmiediate(arg1):
                        instructions.Add(0b_01011000_00001111_00000000_00000000 | ValueLabel(arg1));
                        continue;
                    case ["je", var arg1] when IsLabelOrImmiediate(arg1):
                        instructions.Add(0b_01010001_00001111_00000000_00000000 | ValueLabel(arg1));
                        continue;
                    case ["jne", var arg1] when IsLabelOrImmiediate(arg1):
                        instructions.Add(0b_01011001_00001111_00000000_00000000 | ValueLabel(arg1));
                        continue;
                    case ["jb", var arg1] when IsLabelOrImmiediate(arg1):
                        instructions.Add(0b_01010010_00001111_00000000_00000000 | ValueLabel(arg1));
                        continue;
                    case ["jae", var arg1] when IsLabelOrImmiediate(arg1):
                        instructions.Add(0b_01011010_00001111_00000000_00000000 | ValueLabel(arg1));
                        continue;
                    case ["jbe", var arg1] when IsLabelOrImmiediate(arg1):
                        instructions.Add(0b_01010011_00001111_00000000_00000000 | ValueLabel(arg1));
                        continue;
                    case ["ja", var arg1] when IsLabelOrImmiediate(arg1):
                        instructions.Add(0b_01011011_00001111_00000000_00000000 | ValueLabel(arg1));
                        continue;
                    case ["jl", var arg1] when IsLabelOrImmiediate(arg1):
                        instructions.Add(0b_01010100_00001111_00000000_00000000 | ValueLabel(arg1));
                        continue;
                    case ["jge", var arg1] when IsLabelOrImmiediate(arg1):
                        instructions.Add(0b_01011100_00001111_00000000_00000000 | ValueLabel(arg1));
                        continue;
                    case ["jle", var arg1] when IsLabelOrImmiediate(arg1):
                        instructions.Add(0b_01010101_00001111_00000000_00000000 | ValueLabel(arg1));
                        continue;
                    case ["jg", var arg1] when IsLabelOrImmiediate(arg1):
                        instructions.Add(0b_01011101_00001111_00000000_00000000 | ValueLabel(arg1));
                        continue;
                    case ["load_8", [.. var arg1, ','], ['[' ,.. var arg2, ']']] when IsRegister(arg2):
                        instructions.Add(0b_01100000_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 12);
                        continue;
                    case ["load_8", [.. var arg1, ','], ['[' ,.. var arg2, ']']] when IsImmiediate(arg2):
                        instructions.Add(0b_01110000_00000000_00000000_00000000 | Register(arg1) << 24 | int.Parse(arg2));
                        continue;
                    case ["load_16", [.. var arg1, ','], ['[' ,.. var arg2, ']']] when IsRegister(arg2):
                        instructions.Add(0b_01100001_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 12);
                        continue;
                    case ["load_16", [.. var arg1, ','], ['[' ,.. var arg2, ']']] when IsImmiediate(arg2):
                        instructions.Add(0b_01110001_00000000_00000000_00000000 | Register(arg1) << 24 | int.Parse(arg2));
                        continue;
                    case ["load_32", [.. var arg1, ','], ['[' ,.. var arg2, ']']] when IsRegister(arg2):
                        instructions.Add(0b_01100010_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg2) << 12);
                        continue;
                    case ["load_32", [.. var arg1, ','], ['[' ,.. var arg2, ']']] when IsImmiediate(arg2):
                        instructions.Add(0b_01110010_00000000_00000000_00000000 | Register(arg1) << 24 | int.Parse(arg2));
                        continue;
                    case ["store_8", ['[', .. var arg1, ']', ','], var arg2] when IsRegister(arg1):
                        instructions.Add(0b_01100100_00000000_00000000_00000000 | Register(arg2) << 20 | Register(arg1) << 12);
                        continue;
                    case ["store_8", ['[', .. var arg1, ']', ','], var arg2] when IsImmiediate(arg1):
                        instructions.Add(0b_01110100_00000000_00000000_00000000 | Register(arg2) << 20 | int.Parse(arg1));
                        continue;
                    case ["store_16", ['[', .. var arg1, ']', ','], var arg2] when IsRegister(arg1):
                        instructions.Add(0b_01100101_00000000_00000000_00000000 | Register(arg2) << 20 | Register(arg1) << 12);
                        continue;
                    case ["store_16", ['[', .. var arg1, ']', ','], var arg2] when IsImmiediate(arg1):
                        instructions.Add(0b_01110101_00000000_00000000_00000000 | Register(arg2) << 20 | int.Parse(arg1));
                        continue;
                    case ["store_32", ['[', .. var arg1, ']', ','], var arg2] when IsRegister(arg1):
                        instructions.Add(0b_01100110_00000000_00000000_00000000 | Register(arg2) << 20 | Register(arg1) << 12);
                        continue;
                    case ["store_32", ['[', .. var arg1, ']', ','], var arg2] when IsImmiediate(arg1):
                        instructions.Add(0b_01110110_00000000_00000000_00000000 | Register(arg2) << 20 | int.Parse(arg1));
                        continue;
                    case ["mov", [.. var arg1, ','], var arg2] when IsRegister(arg2):
                        instructions.Add(0b_00100001_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg1) << 12);
                        continue;
                    case ["mov", [.. var arg1, ','], var arg2] when IsLabelOrImmiediate(arg2):
                        instructions.Add(0b_00110001_00000000_00000000_00000000 | Register(arg1) << 24 | ValueLabel(arg2));
                        continue;
                    case ["neg", [.. var arg1, ','], var arg2] when IsRegister(arg2):
                        instructions.Add(0b_00100101_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg1) << 12);
                        continue;
                    case ["neg", [.. var arg1, ','], var arg2] when IsLabelOrImmiediate(arg2):
                        instructions.Add(0b_00110101_00000000_00000000_00000000 | Register(arg1) << 24 | ValueLabel(arg2));
                        continue;
                    case ["not", [.. var arg1, ','], var arg2] when IsRegister(arg2):
                        instructions.Add(0b_00100011_00000000_00000000_00000000 | Register(arg1) << 24 | Register(arg1) << 12);
                        continue;
                    case ["not", [.. var arg1, ','], var arg2] when IsLabelOrImmiediate(arg2):
                        instructions.Add(0b_00110011_00000000_00000000_00000000 | Register(arg1) << 24 | ValueLabel(arg2));
                        continue;
                    case [[.. var label, ':']]:
                        _labelDict.Add(label, instructions.Count * 4);
                        continue;
                    case [[]]:
                        continue;
                    case var @default:
                        throw new NotImplementedException(string.Join(' ', @default));
                }
            }

            return [.. instructions.SelectMany(instruction =>
            {
                return MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref instruction, 1))
                .ToArray()
                .Select(x => (Byte)x);
            })];
        }

        private static int ValueLabel(string val) => _labelDict.TryGetValue(val, out var labelValue) ? labelValue : int.Parse(val);
        private static int Value(string val) => int.Parse(val);

        private static int Register(string val) => val switch
        {
            "zr"    => 0b_0000,
            "r1"    => 0b_0001,
            "r2"    => 0b_0010,
            "r3"    => 0b_0011,
            "r4"    => 0b_0100,
            "r5"    => 0b_0101,
            "r6"    => 0b_0110,
            "r7"    => 0b_0111,
            "r8"    => 0b_1000,
            "r9"    => 0b_1001,
            "r10"   => 0b_1010,
            "r11"   => 0b_1011,
            "r12"   => 0b_1100,
            "r13"   => 0b_1101,
            "sp"    => 0b_1110,
            "flags" => 0b_1111,
            _ => throw new NotImplementedException(),
        };

        static bool IsRegister(string s) => !string.IsNullOrEmpty(s) && s.Any(char.IsLetter) && s.Any(char.IsDigit);

        static bool IsLabelOrImmiediate(string s) => !string.IsNullOrEmpty(s) && (s.All(char.IsLetter) || s.All(char.IsDigit));

        static bool IsImmiediate(string s) => !string.IsNullOrEmpty(s) && s.All(char.IsDigit);
    }
}
