using NUnit.Framework;
using Riddles.Algebra.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Tests.Algebra.Domain
{
    public class ExpressionTest
    {
        private Dictionary<int, (Expression, string, string, double)> _testCaseDictionary 
            = new Dictionary<int, (Expression, string, string, double)>
            {
                {1, (new Term("A"), "A", "A", 1) },
                {2, (new Term("B"), "B", "B", 2) },
                {3, (new Sum(
                    new List<Expression> { new Term("A"), new Term("C") },
                    new List<Expression> { new Term("D"), new Term("B")}, true),
                    "((A+C)-(B+D))", "ABCD", -2) },
                {4, (new Sum(
                    new List<Expression> { new Term("A"), new Term("C"), new Term("B") },
                    new List<Expression> { new Term("D")}, true),
                    "((A+B+C)-(D))",
                    "ABCD",
                    2) },
                {5, (new Sum(
                    new List<Expression> { new Term("D"), new Term("C"), new Term("B") },
                    new List<Expression> { }, true),
                    "(B+C+D)",
                    "BCD",
                    9) },
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
                    "(((A+C+X)-(M+R))+((L)-(T+V+Y))+((P)-(O+Q+Z)))",
                    "ACLMOPQRTVXYZ",
                    -100 // 8.10049679e-7
                    ) },
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
                    "((((F+H)-(G)))-(((A+C+X)-(M+R))+((L)-(T+V+Y))+((P)-(O+Q+Z))))",
                    "ACFGHLMOPQRTVXYZ",
                    107 // 955674623232
                 ) },
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
                    "((A+C+L+P+X)-(M+O+Q+R+T+V+Y+Z))",
                    "ACLMOPQRTVXYZ",
                    -100 // 8.10049679e-7
                    ) },
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
                    "((F+H+M+O+Q+R+T+V+Y+Z)-(A+C+G+L+P+X))",
                    "ACFGHLMOPQRTVXYZ",
                    107 // 955674623232
                    ) },
                {10, (new Product(
                    new List<Expression> { new Term("A"), new Term("C") },
                    new List<Expression> { new Term("D"), new Term("B")}, true),
                    "((A*C)/(B*D))", "ABCD", 3.0/8.0) },
                {11, (new Product(
                    new List<Expression> { new Term("A"), new Term("C"), new Term("B") },
                    new List<Expression> { new Term("D")}, true),
                    "((A*B*C)/(D))",
                    "ABCD",
                    3.0/2.0) },
                {12, (new Product(
                    new List<Expression> { new Term("D"), new Term("C"), new Term("B") },
                    new List<Expression> { }, true),
                    "(B*C*D)",
                    "BCD",
                    24) },
                {13, (new Product(
                    new List<Expression> {
                        new Product(new List<Expression>{
                                new Term("X"),
                                new Term("A"),
                                new Term("C"),
                            },
                            new List<Expression> {
                                new Term("M"),
                                new Term("R")
                            }),
                        new Product(new List<Expression>{
                                new Term("L"),
                            },
                            new List<Expression> {
                                new Term("T"),
                                new Term("V"),
                                new Term("Y"),
                            }),
                        new Product(new List<Expression>{
                                new Term("P"),
                            },
                            new List<Expression> {
                                new Term("Q"),
                                new Term("Z"),
                                new Term("O"),
                            }),
                    },
                    new List<Expression> { }, false),
                    "(((A*C*X)/(M*R))*((L)/(T*V*Y))*((P)/(O*Q*Z)))",
                    "ACLMOPQRTVXYZ",
                    8.10049679e-7
                    ) },
                {14, (new Product(
                    new List<Expression> {
                        new Product(
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
                        new Product(new List<Expression>{
                                new Term("X"),
                                new Term("A"),
                                new Term("C"),
                            },
                            new List<Expression> {
                                new Term("M"),
                                new Term("R")
                            }),
                        new Product(new List<Expression>{
                                new Term("L"),
                            },
                            new List<Expression> {
                                new Term("T"),
                                new Term("V"),
                                new Term("Y"),
                            }),
                        new Product(new List<Expression>{
                                new Term("P"),
                            },
                            new List<Expression> {
                                new Term("Q"),
                                new Term("Z"),
                                new Term("O"),
                            }),
                    }, false),
                    "((((F*H)/(G)))/(((A*C*X)/(M*R))*((L)/(T*V*Y))*((P)/(O*Q*Z))))",
                    "ACFGHLMOPQRTVXYZ",
                    8465089.28571
                 ) },
                {15, (new Product(
                    new List<Expression> {
                        new Product(new List<Expression>{
                                new Term("X"),
                                new Term("A"),
                                new Term("C"),
                            },
                            new List<Expression> {
                                new Term("M"),
                                new Term("R")
                            }),
                        new Product(new List<Expression>{
                                new Term("L"),
                            },
                            new List<Expression> {
                                new Term("T"),
                                new Term("V"),
                                new Term("Y"),
                            }),
                        new Product(new List<Expression>{
                                new Term("P"),
                            },
                            new List<Expression> {
                                new Term("Q"),
                                new Term("Z"),
                                new Term("O"),
                            }),
                    },
                    new List<Expression> { }, true),
                    "((A*C*L*P*X)/(M*O*Q*R*T*V*Y*Z))",
                    "ACLMOPQRTVXYZ",
                    8.10049679e-7
                    ) },
                {16, (new Product(
                    new List<Expression> {
                        new Product(
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
                        new Product(new List<Expression>{
                                new Term("X"),
                                new Term("A"),
                                new Term("C"),
                            },
                            new List<Expression> {
                                new Term("M"),
                                new Term("R")
                            }),
                        new Product(new List<Expression>{
                                new Term("L"),
                            },
                            new List<Expression> {
                                new Term("T"),
                                new Term("V"),
                                new Term("Y"),
                            }),
                        new Product(new List<Expression>{
                                new Term("P"),
                            },
                            new List<Expression> {
                                new Term("Q"),
                                new Term("Z"),
                                new Term("O"),
                            }),
                    }, true),
                    "((F*H*M*O*Q*R*T*V*Y*Z)/(A*C*G*L*P*X))",
                    "ACFGHLMOPQRTVXYZ",
                    8465089.28571
                    ) },
                { 17, (new Product(
                    new List<Expression>{
                        new Term("F"),
                        new Sum(
                            new List<Expression>{ new Term("C"), new Term("B") },
                            new List<Expression>{ },
                            simplifyExpression: true
                        ),
                        new Term("G"),
                        new Sum( 
                            new List<Expression> { new Term("D") },
                            new List<Expression> { new Term("A")},
                            true
                        ),
                        new Term("E")
                    },
                    new List<Expression>{ 
                        new Sum(
                            new List<Expression>{ new Term("N") },
                            new List<Expression>{ new Term("L")},
                            simplifyExpression: true
                        ) 
                    },
                    true
                ), "((((D)-(A))*(B+C)*E*F*G)/(((N)-(L))))", 
                    "ABCDEFGLN", 
                    1575) },
                { 18, (new Sum(
                    new List<Expression>{
                        new Term("F"),
                        new Product(
                            new List<Expression>{ new Term("C"), new Term("B") },
                            new List<Expression>{ },
                            simplifyExpression: true
                        ),
                        new Term("G"),
                        new Product(
                            new List<Expression> { new Term("D") },
                            new List<Expression> { new Term("A")},
                            true
                        ),
                        new Term("E")
                    },
                    new List<Expression>{
                        new Product(
                            new List<Expression>{ new Term("N") },
                            new List<Expression>{ new Term("L")},
                            simplifyExpression: true
                        )
                    },
                    true
                ), "((((D)/(A))+(B*C)+E+F+G)-(((N)/(L))))",
                    "ABCDEFGLN",
                    161.0/6.0) },
                { 19, (
                    new Sum(
                        new List<Expression>{
                            new Term("B"),
                            new Term("A"),
                            new Product(
                                new List<Expression>{ 
                                    new Term("E"),
                                    new Sum(
                                        new List<Expression>{
                                            new Term("D"),
                                            new Term("C")
                                        },
                                        new List<Expression>{ },
                                        simplifyExpression: true
                                    )},
                                new List<Expression>{ 
                                    
                                },
                                simplifyExpression: true
                            )
                        },
                        new List<Expression>{ },
                        simplifyExpression: true
                    ),
                    "(((C+D)*E)+A+B)",
                    "ABCDE",
                    38
                    )},
                { 20, (
                    new Product(
                        new List<Expression>{
                            new Term("B"),
                            new Term("A"),
                            new Sum(
                                new List<Expression>{
                                    new Term("E"),
                                    new Product(
                                        new List<Expression>{
                                            new Term("D"),
                                            new Term("C")
                                        },
                                        new List<Expression>{ },
                                        simplifyExpression: true
                                    )},
                                new List<Expression>{

                                },
                                simplifyExpression: true
                            )
                        },
                        new List<Expression>{ },
                        simplifyExpression: true
                    ),
                    "(((C*D)+E)*A*B)",
                    "ABCDE",
                    34
                    )},
            };

        private double Epsilon = 0.00000001;

        [TestCase(1)] // Simple term
        [TestCase(2)] // Simple term
        [TestCase(3)] // Addition and Subtraction, commutative property
        [TestCase(4)] // Addition and Subtraction, commutative property lists
        [TestCase(5)] // Addition, commutative property
        [TestCase(6)] // Addition and Subtraction - associative and commutative, full form
        [TestCase(7)] // Addition and Subtraction - associative and commutative, full form
        [TestCase(8)] // Addition and Subtraction - associative and commutative, simplified
        [TestCase(9)] // Addition and Subtraction - associative and commutative, simplified
        [TestCase(10)] // Multiplication and Division, commutative property
        [TestCase(11)] // Multiplication and Division, commutative property lists
        [TestCase(12)] // Multiplication, commutative property
        [TestCase(13)] // Multiplication and Division - associative and commutative, full form
        [TestCase(14)] // Multiplication and Division - associative and commutative, full form
        [TestCase(15)] // Multiplication and Division - associative and commutative, simplified
        [TestCase(16)] // Multiplication and Division - associative and commutative, simplified
        // TODO: Add test where the correct ordering is applied for nested sums
        [TestCase(17)] // Combination of Addition, Subtraction, Multiplication, Division
        [TestCase(18)] // Test Case 17 inverted
        [TestCase(19)] // Nested addition within multiplication
        [TestCase(20)] // Test case 19 inverted
        public void TestGetStringRepresentation(int testCaseId)
        {
            var testCase = this._testCaseDictionary[testCaseId];
            var expression = testCase.Item1;
            var exprectedResult = testCase.Item2;
            var actualResult = expression.GetStringRepresentation();
            Assert.AreEqual(exprectedResult, actualResult);
            var actualTermString = expression.Terms
                .Aggregate("", (agg, v) => $"{agg}{v}");
            var expectedTermString = testCase.Item3;
            Assert.AreEqual(expectedTermString, actualTermString);
            var termDictionary =
                Enumerable.Range(0, 26).Select(x => (char)('A' + x))
                .ToDictionary(
                    x => x.ToString(),
                    x => (x - 'A') + 1
                );
            var expectedEvaluation = testCase.Item4;
            var actualEvaluation = expression.Evaluate(termDictionary);
            Assert.LessOrEqual(
                Math.Abs(expectedEvaluation - actualEvaluation) / actualEvaluation,
                Epsilon
            );
        }
    }
}
