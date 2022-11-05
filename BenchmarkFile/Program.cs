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
    public class Stock {
        private const int collectionSize = 1000;
        private const int rSize = 500;
        //OBJECTS
        [BenchmarkCategory("Object"), Benchmark(Baseline = true)]
        public Bestelling[] ArrayForeachObject() {
            Bestelling[] arr = new Bestelling[collectionSize];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = new Bestelling(i + 1, DateTime.Now, 9999999.99, new Klant(i + 1, "Jan Goris van den Boris", "WallibiLaan 9000 Gent Oost-Vlaanderen Belgie"), true);
            }
            foreach (var item in arr) {
                item.VoegTruitjesToe(new Truitje(9999, "2022-2023", new Club("Champs League van europa", "KAA Gent"), new ClubSet(true, 500000), KledingMaat.XL), 90000);
            }
            return arr;
        }

        [BenchmarkCategory("Object"), Benchmark]
        public Bestelling[] ArrayForObject() {
            Bestelling[] arr = new Bestelling[collectionSize];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = new Bestelling(i + 1, DateTime.Now, 9999999.99, new Klant(i + 1, "Jan Goris van den Boris", "WallibiLaan 9000 Gent Oost-Vlaanderen Belgie"), true);
            }
            for (int i = 0; i < arr.Length; i++) {
                arr[i].VoegTruitjesToe(new Truitje(9999, "2022-2023", new Club("Champs League van europa", "KAA Gent"), new ClubSet(true, 500000), KledingMaat.XL), 90000);
            }
            return arr;
        }
        [BenchmarkCategory("Object"), Benchmark]
        public List<Bestelling> ListsClassicObject() {
            List<Bestelling> lst = new List<Bestelling>();
            for (int i = 0; i < collectionSize; i++) {
                lst.Add(new Bestelling(i + 1, DateTime.Now, 9999999.99, new Klant(i + 1, "Jan Goris van den Boris", "WallibiLaan 9000 Gent Oost-Vlaanderen Belgie"), true));
            }
            foreach (var item in lst) {
                item.VoegTruitjesToe(new Truitje(9999, "2022-2023", new Club("Champs League van europa", "KAA Gent"), new ClubSet(true, 500000), KledingMaat.XL), 90000);
            }
            return lst;
        }
        [BenchmarkCategory("Object"), Benchmark]
        public List<Bestelling> ListsGivenLengthForObject() {
            List<Bestelling> lst = new List<Bestelling>((new Bestelling[collectionSize]));
            for (int i = 0; i < lst.Count; i++) {
                lst[i] = new Bestelling(i+1, DateTime.Now, 9999999.99, new Klant(i + 1, "Jan Goris van den Boris", "WallibiLaan 9000 Gent Oost-Vlaanderen Belgie"), true);
            }
            for (int i = 0; i < lst.Count; i++) {
                lst[i].VoegTruitjesToe(new Truitje(9999, "2022-2023", new Club("Champs League van europa", "KAA Gent"), new ClubSet(true, 500000), KledingMaat.XL), 90000);
            }
            return lst;
        }
        [BenchmarkCategory("Object"), Benchmark]
        public List<Bestelling> ListsGivenLengthForeachObject() {
            List<Bestelling> lst = new List<Bestelling>((new Bestelling[collectionSize]));
            for (int i = 0; i < lst.Count; i++) {
                lst[i] = new Bestelling(i + 1, DateTime.Now, 9999999.99, new Klant(i + 1, "Jan Goris van den Boris", "WallibiLaan 9000 Gent Oost-Vlaanderen Belgie"), true);
            }
            foreach (var item in lst) {
                item.VoegTruitjesToe(new Truitje(9999, "2022-2023", new Club("Champs League van europa", "KAA Gent"), new ClubSet(true, 500000), KledingMaat.XL), 90000);
            }
            return lst;
        }




        private const int nestedSize = 200;

        //int
        [BenchmarkCategory("int"), Benchmark(Baseline = true)]
        public int[] ArrayForeachInt() {
            int[] arr = new int[collectionSize];
            Random rnd = new Random(rSize);
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = rnd.Next(rSize);
            }
            long storage = 0;
            for (int i = 0; i < nestedSize; i++) {
                foreach (var item in arr) {
                    storage += item;
                }
            }
            return arr;
        }
        [BenchmarkCategory("int"), Benchmark]
        public int[] ArrayForInt() {
            int[] arr = new int[collectionSize];
            Random rnd = new Random(rSize);
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = rnd.Next(rSize);
            }
            long storage = 0;
            for (int i = 0; i < nestedSize; i++) {
                for (int j = 0; j < arr.Length; j++) {
                    storage += arr[j];
                }
            }
            return arr;
        }

        [BenchmarkCategory("int"), Benchmark]
        public List<int> ListsGivenLengthForInt() {
            List<int> lst = new List<int>(new int[collectionSize]);
            Random rnd = new Random(rSize);
            for (int i = 0; i < collectionSize; i++) {
                lst[i] = (rnd.Next(rSize));
            }
            long storage = 0;
            for (int i = 0; i < nestedSize; i++) {
                for (int j = 0; j < lst.Count; j++) {
                    storage += lst[j];
                }
            }
            return lst;
        }

        [BenchmarkCategory("int"), Benchmark]
        public List<int> ListsGivenLengthForeachInt() {
            List<int> lst = new List<int>(new int[collectionSize]);
            Random rnd = new Random(rSize);
            for (int i = 0; i < collectionSize; i++) {
                lst[i] = (rnd.Next(rSize));
            }
            long storage = 0;
            for (int i = 0; i < nestedSize; i++) {
                foreach (var item in lst) {
                    storage += item;
                }
            }
            return lst;
        }
    }
}