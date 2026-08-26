using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Turing.Core.Computer.Symphony;

namespace Turing.Core.Executeables;

public class SymphonyRunner
{
    private readonly InstructionParser _parser = new();

    public SYMPHONY RunSymphony(string asm, params IEnumerable<Byte> inputs)
    {
        var instructions = _parser.Parse(asm);

        return RunSymphony(instructions, inputs);
    }

    public SYMPHONY RunSymphony(Byte[] instructions, params IEnumerable<Byte> inputs)
    {
        var cpu = new SYMPHONY(instructions);

        var en = inputs.GetEnumerator();
        bool hasNext = en.MoveNext();
        cpu.Input = hasNext ? en.Current : default;


        while (true)
        {
            cpu.EVal();

            if (cpu.InputPin)
            {
                if (!hasNext)
                {
                    return cpu;
                }

                hasNext = en.MoveNext();
                cpu.Input = hasNext ? en.Current : default;
            }

            if (cpu.OutputPin)
            {
                return cpu;
            }

            if (cpu.OffPin)
            {
                return cpu;
            }
        }
    }

    public class LabelToken
    {
        private int? _value;
        private Action<int>? _callback;

        public int RequireLabel(Action<int> value)
        {
            if (_value is { } x) return x;
            _callback += value;
            return 0;
        }

        public void DefineLabel(int value)
        {
            (var callback, _callback) = (_callback, null);
            if (callback is { }) callback(value);
            _value = value;
        }
    }

    private class InstructionParser
    {
        private readonly Dictionary<string, LabelToken> _labelDict = [];

        public Byte[] Parse(string instructionList)
        {
            var instructions = new List<int>();
            instructionList = PreProcess(instructionList);

            foreach (var line in instructionList.AsSpan().EnumerateLines())
            {
                if (string.IsNullOrEmpty(line.ToString()))
                {
                    continue;
                }

                var lineSplit = line.ToString().Trim().Split(' ', StringSplitOptions.TrimEntries);
                Assemble();

                void Assemble()
                {
                    switch (lineSplit)
                    {
                        case ["nop"]:
                            instructions.Add(0b_00000000_00000000_00000000_00000000);
                            return;
                        case ["in", var val]:
                            instructions.Add(0b_00000001_00000000_00000000_00000000 | Register(val) << 20);
                            return;
                        case ["out", var val and [< '0' or > '9', ..]]:
                            instructions.Add(0b_00000010_00000000_00000000_00000000 | Register(val) << 8);
                            return;
                        case ["out", var val]:
                            instructions.Add(0b_00010010_00000000_00000000_00000000 | Value(val));
                            return;
                        case ["counter", var val]:
                            instructions.Add(0b_00000111_00000000_00000000_00000000 | Register(val) << 20);
                            return;
                        case ["nand", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                            instructions.Add(0b_00100000_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | Register(arg3) << 8);
                            return;
                        case ["or", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                            instructions.Add(0b_00100001_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | Register(arg3) << 8);
                            return;
                        case ["and", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                            instructions.Add(0b_00100010_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | Register(arg3) << 8);
                            return;
                        case ["nor", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                            instructions.Add(0b_00100011_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | Register(arg3) << 8);
                            return;
                        case ["add", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                            instructions.Add(0b_00100100_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | Register(arg3) << 8);
                            return;
                        case ["sub", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                            instructions.Add(0b_00100101_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | Register(arg3) << 8);
                            return;
                        case ["xor", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                            instructions.Add(0b_00100110_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | Register(arg3) << 8);
                            return;
                        case ["lsl", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                            instructions.Add(0b_00100111_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | Register(arg3) << 8);
                            return;
                        case ["lsr", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                            instructions.Add(0b_00101000_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | Register(arg3) << 8);
                            return;
                        case ["asr", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsRegister(arg3):
                            instructions.Add(0b_00101001_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | Register(arg3) << 8);
                            return;
                        case ["cmp", [.. var arg1, ','], var arg2] when IsRegister(arg2):
                            instructions.Add(0b_00101010_11110000_00000000_00000000 | Register(arg1) << 16 | Register(arg2) << 8);
                            return;
                        case ["nand", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                            instructions.Add(0b_00110000_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | ValueLabel(arg3));
                            return;
                        case ["or", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                            instructions.Add(0b_00110001_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | ValueLabel(arg3));
                            return;
                        case ["nor", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                            instructions.Add(0b_00110011_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | ValueLabel(arg3));
                            return;
                        case ["and", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                            instructions.Add(0b_00110010_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | ValueLabel(arg3));
                            return;
                        case ["add", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                            instructions.Add(0b_00110100_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | ValueLabel(arg3));
                            return;
                        case ["sub", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                            instructions.Add(0b_00110101_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | ValueLabel(arg3));
                            return;
                        case ["xor", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                            instructions.Add(0b_00110110_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | ValueLabel(arg3));
                            return;
                        case ["xor", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                            instructions.Add(0b_00110110_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | ValueLabel(arg3));
                            return;
                        case ["lsl", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                            instructions.Add(0b_00110111_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | ValueLabel(arg3));
                            return;
                        case ["lsr", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                            instructions.Add(0b_00111000_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | ValueLabel(arg3));
                            return;
                        case ["asr", [.. var arg1, ','], [.. var arg2, ','], var arg3] when IsLabelOrImmiediate(arg3):
                            instructions.Add(0b_00111001_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 16 | ValueLabel(arg3));
                            return;
                        case ["cmp", [.. var arg1, ','], var arg2] when IsImmiediate(arg2):
                            instructions.Add(0b_00111010_11110000_00000000_00000000 | Register(arg1) << 16 | int.Parse(arg2));
                            return;
                        case ["jmp", var arg1] when IsRegister(arg1):
                            instructions.Add(0b_01001000_00001111_00000000_00000000 | Register(arg1) << 8);
                            return;
                        case ["jmp", var arg1] when IsLabelOrImmiediate(arg1):
                            instructions.Add(0b_01011000_00001111_00000000_00000000 | ValueLabel(arg1));
                            return;
                        case ["je", var arg1] when IsLabelOrImmiediate(arg1):
                            instructions.Add(0b_01010001_00001111_00000000_00000000 | ValueLabel(arg1));
                            return;
                        case ["jne", var arg1] when IsLabelOrImmiediate(arg1):
                            instructions.Add(0b_01011001_00001111_00000000_00000000 | ValueLabel(arg1));
                            return;
                        case ["jb", var arg1] when IsLabelOrImmiediate(arg1):
                            instructions.Add(0b_01010010_00001111_00000000_00000000 | ValueLabel(arg1));
                            return;
                        case ["jae", var arg1] when IsLabelOrImmiediate(arg1):
                            instructions.Add(0b_01011010_00001111_00000000_00000000 | ValueLabel(arg1));
                            return;
                        case ["jbe", var arg1] when IsLabelOrImmiediate(arg1):
                            instructions.Add(0b_01010011_00001111_00000000_00000000 | ValueLabel(arg1));
                            return;
                        case ["ja", var arg1] when IsLabelOrImmiediate(arg1):
                            instructions.Add(0b_01011011_00001111_00000000_00000000 | ValueLabel(arg1));
                            return;
                        case ["jl", var arg1] when IsLabelOrImmiediate(arg1):
                            instructions.Add(0b_01010100_00001111_00000000_00000000 | ValueLabel(arg1));
                            return;
                        case ["jge", var arg1] when IsLabelOrImmiediate(arg1):
                            instructions.Add(0b_01011100_00001111_00000000_00000000 | ValueLabel(arg1));
                            return;
                        case ["jle", var arg1] when IsLabelOrImmiediate(arg1):
                            instructions.Add(0b_01010101_00001111_00000000_00000000 | ValueLabel(arg1));
                            return;
                        case ["jg", var arg1] when IsLabelOrImmiediate(arg1):
                            instructions.Add(0b_01011101_00001111_00000000_00000000 | ValueLabel(arg1));
                            return;
                        case ["load_8", [.. var arg1, ','], ['[', .. var arg2, ']']] when IsRegister(arg2):
                            instructions.Add(0b_01100000_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 8);
                            return;
                        case ["load_8", [.. var arg1, ','], ['[', .. var arg2, ']']] when IsImmiediate(arg2):
                            instructions.Add(0b_01110000_00000000_00000000_00000000 | Register(arg1) << 20 | int.Parse(arg2));
                            return;
                        case ["load_16", [.. var arg1, ','], ['[', .. var arg2, ']']] when IsRegister(arg2):
                            instructions.Add(0b_01100001_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 8);
                            return;
                        case ["load_16", [.. var arg1, ','], ['[', .. var arg2, ']']] when IsImmiediate(arg2):
                            instructions.Add(0b_01110001_00000000_00000000_00000000 | Register(arg1) << 20 | int.Parse(arg2));
                            return;
                        case ["load_32", [.. var arg1, ','], ['[', .. var arg2, ']']] when IsRegister(arg2):
                            instructions.Add(0b_01100010_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 8);
                            return;
                        case ["load_32", [.. var arg1, ','], ['[', .. var arg2, ']']] when IsImmiediate(arg2):
                            instructions.Add(0b_01110010_00000000_00000000_00000000 | Register(arg1) << 20 | int.Parse(arg2));
                            return;
                        case ["store_8", ['[', .. var arg1, ']', ','], var arg2] when IsRegister(arg1):
                            instructions.Add(0b_01100100_00000000_00000000_00000000 | Register(arg2) << 16 | Register(arg1) << 8);
                            return;
                        case ["store_8", ['[', .. var arg1, ']', ','], var arg2] when IsImmiediate(arg1):
                            instructions.Add(0b_01110100_00000000_00000000_00000000 | Register(arg2) << 16 | int.Parse(arg1));
                            return;
                        case ["store_16", ['[', .. var arg1, ']', ','], var arg2] when IsRegister(arg1):
                            instructions.Add(0b_01100101_00000000_00000000_00000000 | Register(arg2) << 16 | Register(arg1) << 8);
                            return;
                        case ["store_16", ['[', .. var arg1, ']', ','], var arg2] when IsImmiediate(arg1):
                            instructions.Add(0b_01110101_00000000_00000000_00000000 | Register(arg2) << 16 | int.Parse(arg1));
                            return;
                        case ["store_32", ['[', .. var arg1, ']', ','], var arg2] when IsRegister(arg1):
                            instructions.Add(0b_01100110_00000000_00000000_00000000 | Register(arg2) << 16 | Register(arg1) << 8);
                            return;
                        case ["store_32", ['[', .. var arg1, ']', ','], var arg2] when IsImmiediate(arg1):
                            instructions.Add(0b_01110110_00000000_00000000_00000000 | Register(arg2) << 16 | int.Parse(arg1));
                            return;
                        case ["mov", [.. var arg1, ','], var arg2] when IsRegister(arg2):
                            instructions.Add(0b_00100001_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 8);
                            return;
                        case ["mov", [.. var arg1, ','], var arg2] when IsLabelOrImmiediate(arg2):
                            instructions.Add(0b_00110001_00000000_00000000_00000000 | Register(arg1) << 20 | ValueLabel(arg2));
                            return;
                        case ["neg", [.. var arg1, ','], var arg2] when IsRegister(arg2):
                            instructions.Add(0b_00100101_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 8);
                            return;
                        case ["neg", [.. var arg1, ','], var arg2] when IsLabelOrImmiediate(arg2):
                            instructions.Add(0b_00110101_00000000_00000000_00000000 | Register(arg1) << 20 | ValueLabel(arg2));
                            return;
                        case ["not", [.. var arg1, ','], var arg2] when IsRegister(arg2):
                            instructions.Add(0b_00100011_00000000_00000000_00000000 | Register(arg1) << 20 | Register(arg2) << 8);
                            return;
                        case ["not", [.. var arg1, ','], var arg2] when IsLabelOrImmiediate(arg2):
                            instructions.Add(0b_00110011_00000000_00000000_00000000 | Register(arg1) << 20 | ValueLabel(arg2));
                            return;
                        case ["push", var arg1] when IsRegister(arg1):
                            Push(arg1);
                            return;
                        case ["pop", var arg1] when IsRegister(arg1):
                            Pop(arg1);
                            return;
                        case ["call", var arg1] when IsLabel(arg1):
                            instructions.Add(0b_00000111_11110000_00000000_00000000);
                            instructions.Add(0b_00110100_11111111_00000000_00010100);
                            Push("flags");
                            instructions.Add(0b_01011000_00001111_00000000_00000000 | ValueLabel(arg1));
                            return;
                        case ["ret"]:
                            Pop("flags");
                            instructions.Add(0b_01001000_00001111_00001111_00000000);
                            return;
                        case [[.. var label, ':']]:

                            if (!_labelDict.TryGetValue(label, out var token))
                            {
                                token = new LabelToken();
                                _labelDict.Add(label, token);
                            }

                            token.DefineLabel(instructions.Count * 4);
                            return;
                        case [[]]:
                            return;
                        case var @default:
                            throw new NotImplementedException(string.Join(' ', @default));
                    }
                }
            }

            return [.. instructions.SelectMany(instruction =>
            {
                return MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref instruction, 1))
                .ToArray()
                .Select(x => (Byte)x);
            })];

            void Push(string arg1)
            {
                instructions.Add(0b_00110101_11101110_00000000_00000100);
                instructions.Add(0b_01100110_00000000_00001110_00000000 | Register(arg1) << 16);
            }

            void Pop(string arg1)
            {
                instructions.Add(0b_01100010_00000000_00001110_00000000 | Register(arg1) << 20);
                instructions.Add(0b_00110100_11101110_00000000_00000100);
            }

            int ValueLabel(string val)
            {
                if (IsImmiediate(val))
                {
                    return int.Parse(val);
                }

                var count = instructions.Count;
                Action<int> lambda = (x) => instructions[count] |= x;

                if (!_labelDict.TryGetValue(val, out var token))
                {
                    token = new LabelToken();
                    _labelDict.Add(val, token);
                }

                return token.RequireLabel(lambda);
            }
        }
        private static int Value(string val) => int.Parse(val);

        private static int Register(string val) => val switch
        {

            "zr" => 0b_0000,
            "r1" => 0b_0001,
            "r2" => 0b_0010,
            "r3" => 0b_0011,
            "r4" => 0b_0100,
            "r5" => 0b_0101,
            "r6" => 0b_0110,
            "r7" => 0b_0111,
            "r8" => 0b_1000,
            "r9" => 0b_1001,
            "r10" => 0b_1010,
            "r11" => 0b_1011,
            "r12" => 0b_1100,
            "r13" => 0b_1101,
            "sp" => 0b_1110,
            "flags" => 0b_1111,
            _ => throw new NotImplementedException(),
        };

        private static bool IsRegister(string s) => !string.IsNullOrEmpty(s) && s.Any(s => !char.IsDigit(s)) && s.Any(char.IsDigit);
        private static bool IsLabelOrImmiediate(string s) => !string.IsNullOrEmpty(s) && (s.All(s => !char.IsDigit(s)) || s.All(char.IsDigit));
        private static bool IsLabel(string s) => !string.IsNullOrEmpty(s) && s.All(s => !char.IsDigit(s));
        private static bool IsImmiediate(string s) => !string.IsNullOrEmpty(s) && s.All(char.IsDigit);

        private static string PreProcess(string instructionList)
        {
            var regex = new Regex(@"^\s*?const\s+(?<key>.*?)\s+=\s+(?<value>.*?)\s*?$", RegexOptions.Multiline);
            var matches = regex.Matches(instructionList);

            instructionList = regex.Replace(instructionList, string.Empty);

            foreach (Match match in matches)
            {
                var key = match.Groups["key"].Value;
                var value = match.Groups["value"].Value;

                instructionList = Regex.Replace(instructionList, $@"(?<=[\s,]){key}(?=[\s,])", value);
            }

            return instructionList;
        }
    }
}
