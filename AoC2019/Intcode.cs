using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    public class Intcode
    {
        private Queue<long> _inputs = new Queue<long>();
        private long[] _program;
        private IDictionary<long, long> _memory;
        private long? _output;

        public Intcode()
        {

        }

        public Intcode(string textMemory)
        {
            InitMemory(textMemory);
        }

        private enum OpCode
        {
            Add = 1,
            Mult = 2,
            Input = 3,
            Output = 4,
            JumpNotZero = 5,
            JumpZero = 6,
            LessThan = 7,
            Equals = 8,
            SetBase = 9,
            Terminate = 99,
        }

        private static readonly IReadOnlyDictionary<OpCode, int> opLen = 
            new Dictionary<OpCode, int>
        {
            { OpCode.Add, 4 },
            { OpCode.Mult, 4 },
            { OpCode.Input, 2 },
            { OpCode.Output, 2 },
            { OpCode.JumpNotZero, 3 },
            { OpCode.JumpZero, 3 },
            { OpCode.LessThan, 4 },
            { OpCode.Equals, 4 },
            { OpCode.SetBase, 2 },
            { OpCode.Terminate, 2 },
        };

        class Instruction
        {
            public OpCode OpCode { get; set; }
            public int[] Modes { get; set; }
        }

        public void AddInput(long val)
            => _inputs.Enqueue(val);
        public void AddInputs(params long[] vals)
        {
            foreach (var val in vals)
                AddInput(val);
        }
        public void UpdateInput(long val)
        {
            if (InputQueueLen == 0)
                AddInput(val);
            else
            {
                if (InputQueueLen > 1)
                    throw new Exception("Not supported");
                _inputs.Dequeue();
                _inputs.Enqueue(val);
            }
        }
        public int InputQueueLen { get => _inputs.Count; }

        public void InitMemory(string textData)
        {
            var data = textData.Split(',').Select(long.Parse).ToArray();
            _program = new long[data.Length];
            _memory = new Dictionary<long, long>(data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                _program[i] = data[i];
                _memory[i] = data[i];
            }
        }
        public void ResetProgram()
        {
            _memory = new Dictionary<long, long>(_program.Length);
            for (int i = 0; i < _program.Length; i++)
            {
                _memory[i] = _program[i];
            }
        }

        public void Poke(long addr, long value)
            => _memory[addr] = value;

        public long Peek(long addr)
            => _memory[addr];

        public long LastOutput { get => _output.Value;  }

        public IEnumerable<long?> Execute(
            string memory,
            long[] inputs,
            bool outputToConsole = false)
        {
            InitMemory(memory);
            AddInputs(inputs);
            foreach (var value in Execute(outputToConsole))
                yield return value;
        }

        public IEnumerable<long> Execute(
            bool outputToConsole = false)
        {
            long pc = 0;
            long relativeBase = 0;
            bool isTerminated = false;
            Instruction inst;
            while (!isTerminated)
            {
                inst = ReadInst(pc);
                var skipPcInc = false;
                switch (inst.OpCode)
                {
                    case OpCode.Add:
                        Write(3, Read(1) + Read(2));
                        break;
                    case OpCode.Mult:
                        Write(3, Read(1) * Read(2));
                        break;
                    case OpCode.Input:
                        Write(1, _inputs.Dequeue());
                        break;
                    case OpCode.Output:
                        _output = Read(1);
                        if (outputToConsole)
                            Console.WriteLine(_output);
                        yield return _output.Value;
                        break;
                    case OpCode.JumpNotZero:
                        if (Read(1) != 0)
                        {
                            pc = Read(2);
                            skipPcInc = true;
                        }
                        break;
                    case OpCode.JumpZero:
                        if (Read(1) == 0)
                        {
                            pc = Read(2);
                            skipPcInc = true;
                        }
                        break;
                    case OpCode.LessThan:
                        Write(3, (Read(1) < Read(2)) ? 1 : 0);
                        break;
                    case OpCode.Equals:
                        // Equals
                        Write(3, (Read(1) == Read(2)) ? 1 : 0);
                        break;
                    case OpCode.SetBase:
                        relativeBase += Read(1);
                        break;
                    case OpCode.Terminate:
                        isTerminated = true;
                        break;
                }
                if (!skipPcInc)
                    pc += opLen[inst.OpCode];
            }

            void Write(long offset, long val)
            {
                switch (inst.Modes[offset - 1])
                {
                    case 0:
                        _memory[ReadLit(pc + offset)] = val;
                        break;
                    case 2:
                        _memory[relativeBase + ReadLit(pc + offset)] = val;
                        break;
                    default:
                        throw new Exception();
                }
            }
            long Read(long offset)
            {
                switch (inst.Modes[offset - 1])
                {
                    case 0:
                        return ReadAddr(pc + offset);
                    case 1:
                        return ReadLit(pc + offset);
                    case 2:
                        return ReadLit(relativeBase + ReadLit(pc + offset));
                    default:
                        throw new Exception();
                }
            }
            long ReadLit(long x) => _memory.ContainsKey(x) ? _memory[x] : 0;
            long ReadAddr(long x) => ReadLit(ReadLit(x));
            Instruction ReadInst(long x)
            {
                var val = (int)ReadLit(x);
                return new Instruction
                {
                    OpCode = (OpCode)(val % 100),
                    Modes = new int[]
                    {
                        (val / 100) % 10,
                        (val / 1000) % 10,
                        (val / 10000) % 10,
                    },
                };
            }
        }
    }
}
