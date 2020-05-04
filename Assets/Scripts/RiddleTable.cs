using System;
using System.Collections.Generic;
using System.Linq;

namespace LevelGeneration
{
    public class RiddleTable
    {
        public struct RiddleConstraints
        {
            public int HighestNumber;
        }

        private readonly RiddleConstraints _constraints;
        private readonly Dictionary<char, int> _symbols;
        private readonly char[] _chars = {'▲','◆','■','⬡','●'};
        private char[][] _table;

        private Dictionary<char,int> GenerateSymbolValues()
        {
            var dictionary = new Dictionary<char,int>();
            Random rand = new Random();
            foreach (char character in _chars)
            {
                int generatedNumber = rand.Next(0, this._constraints.HighestNumber);
                while (dictionary.ContainsValue(generatedNumber))
                {
                    generatedNumber = rand.Next(1, this._constraints.HighestNumber);
                }
                dictionary[character] = generatedNumber;
            }
            return dictionary;
        }

        private char[][] GenerateRows(char[][] table, int availableChars)
        {
            if (availableChars == _symbols.Count) return table;
            Random rand = new Random();
            for (int i = 0; i < table[0].Length; i++)
            {
                table[availableChars - 1][i] = _chars[rand.Next(0, availableChars)];
            }

            if (table[availableChars - 1].ToList().Distinct().Count() != availableChars)
            {
                GenerateRows(table, availableChars);
            }

            return GenerateRows(table, availableChars + 1);
        }
        
        public RiddleTable(RiddleConstraints constraints)
        {
            this._constraints = constraints;
            _symbols = GenerateSymbolValues();
            var tab = new char[4][] {new char[4],new char[4],new char[4],new char[4]};
            _table = GenerateRows(tab,1);
            PrintTable();
        }

        public void PrintTable()
        {
            for (int i = 0; i < _table.Count(); i++)
            {
                int resX = 0;
                var resY = new int[4];
                for (int j = 0; j < _table[0].Count(); j++)
                {
                    resX += _symbols[_table[i][j]];
                    Console.Write($" {_table[i][j]}");
                }
                Console.WriteLine($"  {resX}");
            }
        }
    }
}