﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TLS_TextParser;
using System.Collections.Generic;

namespace TLS_Test
{
    [TestClass]
    public class TLS_TextParser_Tests
    {
        [TestMethod]
        public void Parser_WithValidFile_NoErrors()
        {
            try
            {
                TLSParser tlsParser = new TLSParser("C:/Work/Training/TLS_TextParser/TLS_TextParser/text/source_file.txt");
            }
            catch(Exception e)
            {
                Assert.Fail("Expected no exception, but got: " + e.Message);
            }
        }

        [TestMethod]
        public void Parser_WithInvalidFile_ThrowsArgumentException()
        {
            try
            {
                TLSParser tlsParser = new TLSParser("C:/Work/Training/TLS_TextParser/TLS_TextParser/text/invalid_file.txt");
            }
            catch(ArgumentException ae)
            {
                StringAssert.Contains(ae.Message, TLSParser.InvalidFileMessage);
                return;
            }

            Assert.Fail("No Argument Exception was thrown");
        }

        [TestMethod]
        public void Reader_WithSourceFile_FirstLineCorrect()
        {
            TextReader textReader = new TextReader("C:/Work/Training/TLS_TextParser/TLS_TextParser/text/source_file.txt");
            string firstLine = textReader.GetLines()[0];
            StringAssert.Equals(firstLine, "The Project Gutenberg EBook of Romeo and Juliet, by William Shakespeare");
        }

        [TestMethod]
        public void Parser_WithSourceFile_CountsTRA()
        {
            TLSParser tlsParser = new TLSParser("C:/Work/Training/TLS_TextParser/TLS_TextParser/text/source_file.txt");
            int traCount = tlsParser.RegexCount("tra");
            Assert.AreEqual<int>(traCount, 63);
        }

        [TestMethod]
        public void Parser_WithSourceFile_AddsToDictionary()
        {
            TLSParser tlsParser = new TLSParser("C:/Work/Training/TLS_TextParser/TLS_TextParser/text/source_file.txt");
            tlsParser.RunTLSCount();

            int traCount = tlsParser.GetTLSCount("tra");
            int preCount = tlsParser.GetTLSCount("pre");
            int qqqCount = tlsParser.GetTLSCount("qqq");

            Assert.AreEqual<int>(traCount, 63);
            Assert.AreEqual<int>(preCount, 63);
            Assert.AreEqual<int>(qqqCount, 0);
        }

        [TestMethod]
        public void Dictionary_WithInvalidOutput_ReturnsZero()
        {
            TLSDictionary dict = new TLSDictionary();

            int xxxCount = dict.GetCount("xxx");
            int invalidCount = dict.GetCount("Invalid");

            Assert.AreEqual<int>(xxxCount, 0);
            Assert.AreEqual<int>(invalidCount, 0);
        }

        [TestMethod]
        public void Dictionary_WithIncorrectSizeInput_ThrowsException()
        {
            TLSDictionary dict = new TLSDictionary();

            try
            {
                dict.IncrementTLS("Invalid");
            }
            catch(ArgumentException ae)
            {
                StringAssert.Contains(ae.Message, TLSDictionary.IncorrectLengthTLSMessage);
                return;
            }

            Assert.Fail("No Argument Exception was thrown");
        }

        [TestMethod]
        public void Dictionary_IncrementsCounts()
        {
            TLSDictionary dict = new TLSDictionary();

            int emptyAAACount = dict.GetCount("aaa");

            dict.IncrementTLS("aaa");
            int oneAAACount = dict.GetCount("aaa");

            dict.IncrementTLS("aaa");
            int twoAAACount = dict.GetCount("aaa");

            int emptyBBBCount = dict.GetCount("bbb");

            Assert.AreEqual<int>(emptyAAACount, 0);
            Assert.AreEqual<int>(oneAAACount, 1);
            Assert.AreEqual<int>(twoAAACount, 2);
            Assert.AreEqual<int>(emptyBBBCount, 0);
        }

        [TestMethod]
        public void Parser_FindsTLSWithCount()
        {
            TLSParser tlsParser = new TLSParser("C:/Work/Training/TLS_TextParser/TLS_TextParser/text/test_file.txt");
            tlsParser.RunTLSCount();

            List<string> zeroList = tlsParser.GetTLSWithCount(0);
            List<string> oneList = tlsParser.GetTLSWithCount(1);
            List<string> twoList = tlsParser.GetTLSWithCount(2);
            List<string> eightList = tlsParser.GetTLSWithCount(8);

            Assert.AreEqual<int>(zeroList.Count, 0);

            Assert.AreEqual<int>(oneList.Count, 1);
            StringAssert.Equals(oneList[0], "one");

            Assert.AreEqual<int>(twoList.Count, 2);
            StringAssert.Equals(twoList[0], "two");
            StringAssert.Equals(twoList[1], "dos");

            Assert.AreEqual<int>(eightList.Count, 1);
            StringAssert.Equals(eightList[0], "oct");
        }

        [TestMethod]
        public void TopN_WithTestInput_FindsTopN()
        {
            TLSTopN top5 = new TLSTopN(5);

            top5.AddNew(new TLSSortedPair("Ten", 10));
            top5.AddNew(new TLSSortedPair("Non", 9));
            top5.AddNew(new TLSSortedPair("Oct", 8));
            top5.AddNew(new TLSSortedPair("Hep", 7));
            top5.AddNew(new TLSSortedPair("Hex", 6));
            top5.AddNew(new TLSSortedPair("Qin", 5));
            top5.AddNew(new TLSSortedPair("Qad", 4));

            List<string> top5List = top5.GetTopN();

            Assert.AreEqual<int>(top5List.Count, 5);
            StringAssert.Contains(top5List[0], "Ten");
            StringAssert.Contains(top5List[1], "Non");
            StringAssert.Contains(top5List[2], "Oct");
            StringAssert.Contains(top5List[3], "Hep");
            StringAssert.Contains(top5List[4], "Hex");
        }

        [TestMethod]
        public void TopN_WithSameFrequencyInput_FindsAplhabeticalN()
        {
            TLSTopN top5 = new TLSTopN(5);

            top5.AddNew(new TLSSortedPair("zzz", 10));
            top5.AddNew(new TLSSortedPair("sss", 10));
            top5.AddNew(new TLSSortedPair("ccc", 10));
            top5.AddNew(new TLSSortedPair("ddd", 10));
            top5.AddNew(new TLSSortedPair("aaa", 10));
            top5.AddNew(new TLSSortedPair("eee", 10));
            top5.AddNew(new TLSSortedPair("fff", 10));
            top5.AddNew(new TLSSortedPair("bbb", 10));

            List<string> top5List = top5.GetTopN();

            Assert.AreEqual<int>(5, top5List.Count);
            StringAssert.Contains(top5List[0], "aaa");
            StringAssert.Contains(top5List[1], "bbb");
            StringAssert.Contains(top5List[2], "ccc");
            StringAssert.Contains(top5List[3], "ddd");
            StringAssert.Contains(top5List[4], "eee");
        }

        [TestMethod]
        public void TopN_WithInsufficientInput_LimitsSizeOfList()
        {
            TLSTopN top5 = new TLSTopN(5);

            top5.AddNew(new TLSSortedPair("Ten", 10));

            List<string> top5List = top5.GetTopN();

            Assert.AreEqual<int>(1, top5List.Count);
        }
    }
}
