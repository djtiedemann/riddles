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
        public abstract string GetStringRepresentation(bool useCache = true);
        public abstract bool ExpressionHasNegativeSignParity();
        public abstract double Evaluate(Dictionary<string, double> termValues);

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

        public abstract Expression Copy();
    }

    public class Term : Expression
    {
        private string? _stringRepresentation;
        public Term(string term) {
            this._terms = new List<string> { term };
        }

        public override string GetStringRepresentation(bool useCache = true)
        {
            if(this._stringRepresentation == null)
            {
                this._stringRepresentation = this._terms.Single();
            }
            return this._stringRepresentation;
        }

        public override double Evaluate(Dictionary<string, double> termValues)
        {
            return termValues[this._terms.Single()];
        }

        public override bool ExpressionHasNegativeSignParity()
        {
            return false;
        }

        public override Expression Copy()
        {
            return new Term(this._terms.Single());
        }
    }

    /// <summary>
    /// Used to build expressions for sums and products
    /// </summary>
    public abstract class CommutativeAndAssociativeExpression : Expression
    {
        protected List<Expression> _standardExpressions;
        protected List<Expression> _invertedExpressions;
        protected string? _stringRepresentation;
        private string _standardExpressionDelimeter;
        private string _invertedExpressionDelimeter;
        protected bool _simplifyExpression;
        public CommutativeAndAssociativeExpression(
            List<Expression> standardExpressions, 
            List<Expression> invertedExpressions,
            string standardExpressionDelimeter,
            string invertedExpressionDelimeter,
            bool simplifyExpression
        )
        {
            this._standardExpressionDelimeter = standardExpressionDelimeter;
            this._invertedExpressionDelimeter = invertedExpressionDelimeter;
            this._simplifyExpression = simplifyExpression;
            if (simplifyExpression)
            {
                this._standardExpressions = new List<Expression>();
                this._invertedExpressions = new List<Expression>();
                foreach(var expression in standardExpressions)
                {
                    if(this.ShouldApplyAssociativeProperty(expression))
                    {
                        this._standardExpressions.AddRange(((CommutativeAndAssociativeExpression)expression)
                            .GetStandardExpressions());
                        this._invertedExpressions.AddRange(((CommutativeAndAssociativeExpression)expression)
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
                        this._invertedExpressions.AddRange(
                            ((CommutativeAndAssociativeExpression)expression)
                                .GetStandardExpressions());
                        this._standardExpressions.AddRange(
                            ((CommutativeAndAssociativeExpression)expression)
                            .GetInvertedExpressions());
                    }
                    else
                    {
                        this._invertedExpressions.Add(expression);
                    }
                }
                this._standardExpressions = this._standardExpressions
                    .OrderBy(x => x.GetStringRepresentation())
                    .ToList();
                this._invertedExpressions = this._invertedExpressions
                    .OrderBy(x => x.GetStringRepresentation())
                    .ToList();
                this.SimplifySigns();
            }
            else
            {
                this._standardExpressions = standardExpressions;
                this._invertedExpressions = invertedExpressions;
            }
            
            this._terms = 
                standardExpressions.SelectMany(x => x.Terms).ToList().Concat(
                    invertedExpressions.SelectMany(x => x.Terms)
                    ).Distinct().OrderBy(x => x).ToList();
        }

        protected abstract bool ShouldApplyAssociativeProperty(Expression e);

        public abstract bool CanInvertSignParity();
        public abstract void InvertSignParity();
        public abstract void SimplifySigns();
        protected abstract List<CommutativeAndAssociativeExpression> GetInvertibleExpressions();

        public override string GetStringRepresentation(bool useCache = true)
        {
            if(!useCache || this._stringRepresentation == null)
            {
                var addedStringRepresentations =
                    this._standardExpressions.Select(s =>
                        s.GetStringRepresentation(useCache)
                    ).OrderBy(x => x)
                    .Aggregate("", (acc, exp) => acc == "" ? exp : $"{acc}{this._standardExpressionDelimeter}{exp}");
                var subtractedStringRepresentations = 
                    this._invertedExpressions.Select(s =>
                        s.GetStringRepresentation(useCache)
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
            return this._invertedExpressions.ToList();
        }
    }

    public class Sum : CommutativeAndAssociativeExpression
    {
        public Sum(List<Expression> standardExpressions,
            List<Expression> invertedExpressions,
            bool simplifyExpression) 
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
        public override bool CanInvertSignParity()
        {
            return this._invertedExpressions.Count() != 0;
        }
        public override void InvertSignParity()
        {
            var temp = this._standardExpressions;
            this._standardExpressions = this._invertedExpressions;
            this._invertedExpressions = temp;
            this._stringRepresentation = null;
        }
        protected override List<CommutativeAndAssociativeExpression> GetInvertibleExpressions()
        {
            return new List<CommutativeAndAssociativeExpression>();
        }
        public override void SimplifySigns()
        {
        }
        public override double Evaluate(Dictionary<string, double> termValues)
        {
            var standardExpressions = this._standardExpressions
                .Sum(x => x.Evaluate(termValues));
            var invertedExpressions = this._invertedExpressions
                .Sum(x => x.Evaluate(termValues));
            return standardExpressions - invertedExpressions;
        }

        public override bool ExpressionHasNegativeSignParity()
        {
            if (this._invertedExpressions.Any())
            {
                if (this._standardExpressions.Any())
                {
                    return
                        this._standardExpressions[0].GetStringRepresentation()
                        .CompareTo(this._invertedExpressions[0].GetStringRepresentation())
                        > 0;
                }
                return true;
            }
            return false;
        }

        public override Expression Copy()
        {
            var standardExpressionsCopy = new List<Expression>();
            foreach (var expression in this._standardExpressions)
            {
                standardExpressionsCopy.Add(expression.Copy());
            }
            var invertedExpressionsCopy = new List<Expression>();
            foreach (var expression in this._invertedExpressions)
            {
                invertedExpressionsCopy.Add(expression.Copy());
            }
            return new Sum(
                standardExpressionsCopy,
                invertedExpressionsCopy,
                this._simplifyExpression
            );
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
        public override bool CanInvertSignParity()
        {
            throw new NotImplementedException();
        }
        public override void InvertSignParity()
        {
            var invertibleExpressions =
                this.GetInvertibleExpressions();
            if (invertibleExpressions.Any())
            {
                invertibleExpressions.Last().InvertSignParity();
                this._stringRepresentation = null;
            }
        }
        public override void SimplifySigns()
        {
            var hasNegativeSignParity = this.ExpressionHasNegativeSignParity();
            var invertibleExpressions =
                this.GetInvertibleExpressions();
            foreach (var expression in invertibleExpressions)
            {
                if (expression.ExpressionHasNegativeSignParity())
                {
                    expression.InvertSignParity();
                }
            }
            if (hasNegativeSignParity)
            {
                this.InvertSignParity();
            }
        }
        protected override List<CommutativeAndAssociativeExpression> GetInvertibleExpressions()
        {
            return this._standardExpressions
                    .Where(c => c is CommutativeAndAssociativeExpression)
                    .Select(c => (CommutativeAndAssociativeExpression)c).Concat(
                    this._invertedExpressions
                    .Where(c => c is CommutativeAndAssociativeExpression)
                    .Select(c => (CommutativeAndAssociativeExpression)c)
                ).Where(x => x.CanInvertSignParity())
                    .ToList();
        }
        public override double Evaluate(Dictionary<string, double> termValues)
        {
            var standardExpressions = this._standardExpressions
                .Aggregate(1.0, (agg, x) => agg*(x.Evaluate(termValues)));
            var invertedExpressions = this._invertedExpressions
                .Aggregate(1.0, (agg, x) => agg * (x.Evaluate(termValues)));
            return standardExpressions / invertedExpressions;
        }

        public override bool ExpressionHasNegativeSignParity()
        {
            return 
                this.GetInvertibleExpressions()
                .Where(x => x.ExpressionHasNegativeSignParity())
                .Count() % 2 == 1;
           
        }

        public override Expression Copy()
        {
            var standardExpressionsCopy = new List<Expression>();
            foreach (var expression in this._standardExpressions)
            {
                standardExpressionsCopy.Add(expression.Copy());
            }
            var invertedExpressionsCopy = new List<Expression>();
            foreach (var expression in this._invertedExpressions)
            {
                invertedExpressionsCopy.Add(expression.Copy());
            }
            return new Product(
                standardExpressionsCopy,
                invertedExpressionsCopy,
                this._simplifyExpression
            );
        }
    }
}
