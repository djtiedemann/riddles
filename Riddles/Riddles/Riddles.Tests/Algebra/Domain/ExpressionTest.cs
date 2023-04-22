using NUnit.Framework;
using Riddles.Algebra.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Tests.Algebra.Domain
{
    public class ExpressionTest
    {
        private Dictionary<int, (Expression, string)> _testCaseDictionary 
            = new Dictionary<int, (Expression, string)>
            {
                {1, (new Term("A"), "A") },
                {2, (new Term("B"), "B") },
                {3, (new Sum(
                    new List<Expression> { new Term("A"), new Term("C") }, 
                    new List<Expression> { new Term("D"), new Term("B")}, true), 
                    "((A+C)-(B+D))") },
                {4, (new Sum(
                    new List<Expression> { new Term("A"), new Term("C"), new Term("B") },
                    new List<Expression> { new Term("D")}, true),
                    "((A+B+C)-(D))") },
                {5, (new Sum(
                    new List<Expression> { new Term("D"), new Term("C"), new Term("B") },
                    new List<Expression> { }, true),
                    "(B+C+D)") },
                {6, (new Sum(
                    new List<Expression> {
                        new Sum(new List<Expression>{
                                new Term("X"),
                                new Term("A"), 
                                new Term("C"),
                            }, 
                            new List<Expression> {
                                new Term("M"),
                                new Term("R")
                            }),
                        new Sum(new List<Expression>{
                                new Term("L"),
                            },
                            new List<Expression> {
                                new Term("T"),
                                new Term("V"),
                                new Term("Y"),
                            }),
                        new Sum(new List<Expression>{
                                new Term("P"),
                            },
                            new List<Expression> {
                                new Term("Q"),
                                new Term("Z"),
                                new Term("O"),
                            }),
                    },
                    new List<Expression> { }, false),
                    "(((A+C+X)-(M+R))+((L)-(T+V+Y))+((P)-(O+Q+Z)))") },
                {7, (new Sum(
                    new List<Expression> {
                        new Sum(
                            new List<Expression>
                            {
                                new Term("H"),
                                new Term("F")
                            },
                            new List<Expression>
                            {
                                new Term("G")
                            },
                            false
                        ) },
                    new List<Expression>{
                        new Sum(new List<Expression>{
                                new Term("X"),
                                new Term("A"),
                                new Term("C"),
                            },
                            new List<Expression> {
                                new Term("M"),
                                new Term("R")
                            }),
                        new Sum(new List<Expression>{
                                new Term("L"),
                            },
                            new List<Expression> {
                                new Term("T"),
                                new Term("V"),
                                new Term("Y"),
                            }),
                        new Sum(new List<Expression>{
                                new Term("P"),
                            },
                            new List<Expression> {
                                new Term("Q"),
                                new Term("Z"),
                                new Term("O"),
                            }),
                    }, false),
                    "((((F+H)-(G)))-(((A+C+X)-(M+R))+((L)-(T+V+Y))+((P)-(O+Q+Z))))") },
                {8, (new Sum(
                    new List<Expression> {
                        new Sum(new List<Expression>{
                                new Term("X"),
                                new Term("A"),
                                new Term("C"),
                            },
                            new List<Expression> {
                                new Term("M"),
                                new Term("R")
                            }),
                        new Sum(new List<Expression>{
                                new Term("L"),
                            },
                            new List<Expression> {
                                new Term("T"),
                                new Term("V"),
                                new Term("Y"),
                            }),
                        new Sum(new List<Expression>{
                                new Term("P"),
                            },
                            new List<Expression> {
                                new Term("Q"),
                                new Term("Z"),
                                new Term("O"),
                            }),
                    },
                    new List<Expression> { }, true),
                    "((A+C+L+P+X)-(M+O+Q+R+T+V+Y+Z))") },
                {9, (new Sum(
                    new List<Expression> {
                        new Sum(
                            new List<Expression>
                            {
                                new Term("H"),
                                new Term("F")
                            },
                            new List<Expression>
                            {
                                new Term("G")
                            },
                            true
                        ) },
                    new List<Expression>{
                        new Sum(new List<Expression>{
                                new Term("X"),
                                new Term("A"),
                                new Term("C"),
                            },
                            new List<Expression> {
                                new Term("M"),
                                new Term("R")
                            }),
                        new Sum(new List<Expression>{
                                new Term("L"),
                            },
                            new List<Expression> {
                                new Term("T"),
                                new Term("V"),
                                new Term("Y"),
                            }),
                        new Sum(new List<Expression>{
                                new Term("P"),
                            },
                            new List<Expression> {
                                new Term("Q"),
                                new Term("Z"),
                                new Term("O"),
                            }),
                    }, true),
                    "((F+H+M+O+Q+R+T+V+Y+Z)-(A+C+G+L+P+X))") },
            };

        [TestCase(1)] // Simple term
        [TestCase(2)] // Simple term
        [TestCase(3)] // Addition and Subtraction, commutative property
        [TestCase(4)] // Addition and Subtraction, commutative property lists
        [TestCase(5)] // Addition, commutative property
        [TestCase(6)] // Addition and Subtraction - associative and commutative, full form
        [TestCase(7)] // Addition and Subtraction - associative and commutative, full form
        [TestCase(8)] // Addition and Subtraction - associative and commutative, simplified
        [TestCase(9)] // Addition and Subtraction - associative and commutative, simplified
        // TODO: Add test where the correct ordering is applied for nested sums
        public void TestGetStringRepresentation(int testCaseId)
        {
            var testCase = this._testCaseDictionary[testCaseId];
            var expression = testCase.Item1;
            var exprectedResult = testCase.Item2;
            var actualResult = expression.GetStringRepresentation();
            Assert.AreEqual(exprectedResult, actualResult);
        }
    }
}
