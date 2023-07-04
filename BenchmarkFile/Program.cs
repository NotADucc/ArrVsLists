using BenchmarkDotNet.Running;
using System;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using System.IO;
using BenchmarkDotNet.Configs;
using WebSocketSharp;
using Perfolizer.Mathematics.SignificanceTesting;
using VerkoopTruithesBL.Model;
using System.Numerics;
using System.ComponentModel;

namespace BenchmarkFile {
    public class Program {

        static void Main(string[] args) {
            try {
                var summary = BenchmarkRunner.Run<Stock>();
                Console.ReadKey();
            } catch (Exception e) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(e.StackTrace);
                Console.ResetColor();
            }
        }
    }
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [Config(typeof(Config))]
    public class Stock {
        [Params(100, 1_000, 10_000, 100_000, 1_000_000)]
        public int _collectionSize;
        private Bestelling[] _arrayObject;
        private List<Bestelling> _listObject;
        private int[] _arrayPrimitive;
        private List<int> _listPrimitive;
        [GlobalSetup]
        public void Setup() 
        {
            _arrayObject = new Bestelling[_collectionSize];
            _listObject = new List<Bestelling>();
            _arrayPrimitive = new int[_collectionSize];
            _listPrimitive = new List<int>();
            Random rnd = new Random(69_420);
            for (int i = 0; i < _collectionSize; i++)
            {
                _arrayObject[i] = new Bestelling(i + 1, DateTime.Now, 9999999.99, new Klant(i + 1, "Jan Goris van den Boris", "WallibiLaan 9000 Gent Oost-Vlaanderen Belgie"), true);
                _listObject.Add(new Bestelling(i + 1, DateTime.Now, 9999999.99, new Klant(i + 1, "Jan Goris van den Boris", "WallibiLaan 9000 Gent Oost-Vlaanderen Belgie"), true));
                _arrayPrimitive[i] = rnd.Next(10);
                _listPrimitive.Add(rnd.Next(10));
            }
        }

        #region Objects
        [BenchmarkCategory("Array", "Object"), Benchmark(Baseline = true)]
        public Bestelling[] ArrayForeachObject() {
            foreach (var item in _arrayObject) {
                item.VoegTruitjesToe(new Truitje(9999, "2022-2023", new Club("Champs League van europa", "KAA Gent"), new ClubSet(true, 500000), KledingMaat.XL), 90000);
            }
            return _arrayObject;
        }
        [BenchmarkCategory("Array", "Object"), Benchmark]
        public Bestelling[] ArrayForObject()
        {
            for (int i = 0; i < _collectionSize; i++)
            {
                _arrayObject[i].VoegTruitjesToe(new Truitje(9999, "2022-2023", new Club("Champs League van europa", "KAA Gent"), new ClubSet(true, 500000), KledingMaat.XL), 90000);
            }
            return _arrayObject;
        }


        [BenchmarkCategory("List", "Object"), Benchmark(Baseline = true)]
        public List<Bestelling> ListForeachObject()
        {
            foreach (var item in _listObject)
            {
                item.VoegTruitjesToe(new Truitje(9999, "2022-2023", new Club("Champs League van europa", "KAA Gent"), new ClubSet(true, 500000), KledingMaat.XL), 90000);
            }
            return _listObject;
        }
        [BenchmarkCategory("List", "Object"), Benchmark]
        public List<Bestelling> ListForObject()
        {
            for (int i = 0; i < _collectionSize; i++)
            {
                _listObject[i].VoegTruitjesToe(new Truitje(9999, "2022-2023", new Club("Champs League van europa", "KAA Gent"), new ClubSet(true, 500000), KledingMaat.XL), 90000);
            }
            return _listObject;
        }
        #endregion



        #region Primitive
        [BenchmarkCategory("Array", "Primitive"), Benchmark(Baseline = true)]
        public int ArrayForeachPrimitive()
        {
            int x= 0;
            foreach (var item in _arrayPrimitive)
            {
                x += item;    
            }
            return x;
        }
        [BenchmarkCategory("Array", "Primitive"), Benchmark]
        public int ArrayForPrimitive()
        {
            int x = 0;
            for (int i = 0; i < _collectionSize; i++)
            {
                x += _arrayPrimitive[i];
            }
            return x;
        }


        [BenchmarkCategory("List", "Primitive"), Benchmark(Baseline = true)]
        public int ListForeachPrimitive()
        {
            int x = 0;
            foreach (var item in _listPrimitive)
            {
                x += item;
            }
            return x;
        }
        [BenchmarkCategory("List", "Primitive"), Benchmark]
        public int ListForPrimitive()
        {
            int x = 0;
            for (int i = 0; i < _collectionSize; i++)
            {
                x += _listPrimitive[i];
            }
            return x;
        }
        #endregion
    }
}