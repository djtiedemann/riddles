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
    /// A Sum class contains a list of expressions that should be summed
    /// and a list of expressions that should be subracted.
    /// It orders the expressions and evaluates them
    /// </summary>
    public class Sum : Expression
    {
        private List<Expression> _addedExpressions;
        private List<Expression> _subtractedExpressions;
        private string? _stringRepresentation;
        public Sum(
            List<Expression> addedExpressions, 
            List<Expression> subtractedExpressions,
            bool simplifyExpression = true
        )
        {
            if (simplifyExpression)
            {
                this._addedExpressions = new List<Expression>();
                this._subtractedExpressions = new List<Expression>();
                foreach(var expression in addedExpressions)
                {
                    if(expression is Sum)
                    {
                        this._addedExpressions.AddRange(((Sum)expression).GetAddedExpressions());
                        this._subtractedExpressions.AddRange(((Sum)expression).GetSubtractedExpressions());
                    }
                    else
                    {
                        this._addedExpressions.Add(expression);
                    }
                }
                foreach(var expression in subtractedExpressions)
                {
                    if (expression is Sum)
                    {
                        this._subtractedExpressions.AddRange(((Sum)expression).GetAddedExpressions());
                        this._addedExpressions.AddRange(((Sum)expression).GetSubtractedExpressions());
                    }
                    else
                    {
                        this._subtractedExpressions.Add(expression);
                    }
                }
            }
            else
            {
                this._addedExpressions = addedExpressions;
                this._subtractedExpressions = subtractedExpressions;
            }
            
            this._terms = 
                addedExpressions.SelectMany(x => x.Terms).ToList().Concat(
                    subtractedExpressions.SelectMany(x => x.Terms)
                    ).ToList();
        }

        public override double Evaluate(Dictionary<string, int> termValues)
        {
            var addedExpressions = this._addedExpressions
                .Sum(x => x.Evaluate(termValues));
            var subtractedExpressions = this._subtractedExpressions
                .Sum(x => x.Evaluate(termValues));
            return addedExpressions - subtractedExpressions;
        }

        public override string GetStringRepresentation()
        {
            if(this._stringRepresentation == null)
            {
                var addedStringRepresentations =
                    this._addedExpressions.Select(s =>
                        s.GetStringRepresentation()
                    ).OrderBy(x => x)
                    .Aggregate("", (acc, exp) => acc == "" ? exp : $"{acc}+{exp}");
                var subtractedStringRepresentations = 
                    this._subtractedExpressions.Select(s =>
                        s.GetStringRepresentation()
                    ).OrderBy(x => x)
                    .Aggregate("", (acc, exp) => acc == "" ? exp : $"{acc}+{exp}");
                if (subtractedStringRepresentations == "")
                {
                    this._stringRepresentation = $"({addedStringRepresentations})";
                }
                else
                {
                    this._stringRepresentation 
                        = $"(({addedStringRepresentations})-({subtractedStringRepresentations}))";

                }
            }
            return this._stringRepresentation;
        }

        public List<Expression> GetAddedExpressions() { 
            return this._addedExpressions.ToList(); 
        }

        public List<Expression> GetSubtractedExpressions()
        {
            return this._subtractedExpressions.ToList();
        }
    }
}
