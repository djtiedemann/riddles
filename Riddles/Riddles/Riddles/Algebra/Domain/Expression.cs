using Riddles.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Algebra.Domain
{
    public enum ExpressionType { 
        Term = 0,
        Product = 1,
        Sum = 2
    }

    public abstract class Expression
    {
        protected List<string> _terms;

        public List<string> Terms { get { return this._terms; } }
        public abstract string GetStringRepresentation();
        public abstract double Evaluate(Dictionary<string, int> termValues);

        public override bool Equals(object obj)
        {
            if (!(obj is Expression))
            {
                return false;
            }
            return this.GetStringRepresentation() == ((Expression)obj).GetStringRepresentation();
        }

        public override int GetHashCode()
        {
            return this.GetStringRepresentation().GetHashCode();
        }
    }

    public class Term : Expression
    {
        private string? _stringRepresentation;
        public Term(string term) {
            this._terms = new List<string> { term };
        }

        public override string GetStringRepresentation()
        {
            if(this._stringRepresentation == null)
            {
                this._stringRepresentation = this._terms.Single();
            }
            return this._stringRepresentation;
        }

        public override double Evaluate(Dictionary<string, int> termValues)
        {
            return termValues[this._terms.Single()];
        }
    }

    /// <summary>
    /// Used to build expressions for sums and products
    /// </summary>
    public abstract class CommutativeAndAssociativeExpression : Expression
    {
        protected List<Expression> _standardExpressions;
        protected List<Expression> invertedExpressions;
        private string? _stringRepresentation;
        private string _standardExpressionDelimeter;
        private string _invertedExpressionDelimeter;
        public CommutativeAndAssociativeExpression(
            List<Expression> standardExpressions, 
            List<Expression> invertedExpressions,
            string standardExpressionDelimeter,
            string invertedExpressionDelimeter,
            bool simplifyExpression = true
        )
        {
            this._standardExpressionDelimeter = standardExpressionDelimeter;
            this._invertedExpressionDelimeter = invertedExpressionDelimeter;
            if (simplifyExpression)
            {
                this._standardExpressions = new List<Expression>();
                this.invertedExpressions = new List<Expression>();
                foreach(var expression in standardExpressions)
                {
                    if(this.ShouldApplyAssociativeProperty(expression))
                    {
                        this._standardExpressions.AddRange(((CommutativeAndAssociativeExpression)expression)
                            .GetStandardExpressions());
                        this.invertedExpressions.AddRange(((CommutativeAndAssociativeExpression)expression)
                            .GetInvertedExpressions());
                    }
                    else
                    {
                        this._standardExpressions.Add(expression);
                    }
                }
                foreach(var expression in invertedExpressions)
                {
                    if (this.ShouldApplyAssociativeProperty(expression))
                    {
                        this.invertedExpressions.AddRange(
                            ((CommutativeAndAssociativeExpression)expression)
                                .GetStandardExpressions());
                        this._standardExpressions.AddRange(
                            ((CommutativeAndAssociativeExpression)expression)
                            .GetInvertedExpressions());
                    }
                    else
                    {
                        this.invertedExpressions.Add(expression);
                    }
                }
            }
            else
            {
                this._standardExpressions = standardExpressions;
                this.invertedExpressions = invertedExpressions;
            }
            
            this._terms = 
                standardExpressions.SelectMany(x => x.Terms).ToList().Concat(
                    invertedExpressions.SelectMany(x => x.Terms)
                    ).Distinct().ToList();
        }

        protected abstract bool ShouldApplyAssociativeProperty(Expression e);

        public override string GetStringRepresentation()
        {
            if(this._stringRepresentation == null)
            {
                var addedStringRepresentations =
                    this._standardExpressions.Select(s =>
                        s.GetStringRepresentation()
                    ).OrderBy(x => x)
                    .Aggregate("", (acc, exp) => acc == "" ? exp : $"{acc}{this._standardExpressionDelimeter}{exp}");
                var subtractedStringRepresentations = 
                    this.invertedExpressions.Select(s =>
                        s.GetStringRepresentation()
                    ).OrderBy(x => x)
                    .Aggregate("", (acc, exp) => acc == "" ? exp : $"{acc}{this._standardExpressionDelimeter}{exp}");
                if (subtractedStringRepresentations == "")
                {
                    this._stringRepresentation = $"({addedStringRepresentations})";
                }
                else
                {
                    this._stringRepresentation 
                        = $"(({addedStringRepresentations}){this._invertedExpressionDelimeter}({subtractedStringRepresentations}))";
                }
            }
            return this._stringRepresentation;
        }

        public List<Expression> GetStandardExpressions() { 
            return this._standardExpressions.ToList(); 
        }

        public List<Expression> GetInvertedExpressions()
        {
            return this.invertedExpressions.ToList();
        }
    }

    public class Sum : CommutativeAndAssociativeExpression
    {
        public Sum(List<Expression> standardExpressions,
            List<Expression> invertedExpressions,
            bool simplifyExpression = true) 
            : base(
                  standardExpressions,
                  invertedExpressions,
                  "+",
                  "-",
                  simplifyExpression
                )
        {

        }

        protected override bool ShouldApplyAssociativeProperty(Expression e)
        {
            return e is Sum;
        }
        public override double Evaluate(Dictionary<string, int> termValues)
        {
            var standardExpressions = this._standardExpressions
                .Sum(x => x.Evaluate(termValues));
            var invertedExpressions = this.invertedExpressions
                .Sum(x => x.Evaluate(termValues));
            return standardExpressions - invertedExpressions;
        }
    }

    public class Product : CommutativeAndAssociativeExpression
    {
        public Product(List<Expression> standardExpressions,
            List<Expression> invertedExpressions,
            bool simplifyExpression = true)
            : base(
                  standardExpressions,
                  invertedExpressions,
                  "*",
                  "/",
                  simplifyExpression
                )
        {

        }

        protected override bool ShouldApplyAssociativeProperty(Expression e)
        {
            return e is Product;
        }
        public override double Evaluate(Dictionary<string, int> termValues)
        {
            var standardExpressions = this._standardExpressions
                .Aggregate(1.0, (agg, x) => agg*x.Evaluate(termValues));
            var invertedExpressions = this.invertedExpressions
                .Sum(x => x.Evaluate(termValues));
            return standardExpressions / invertedExpressions;
        }
    }
}
