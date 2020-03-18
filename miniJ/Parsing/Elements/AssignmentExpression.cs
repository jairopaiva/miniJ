﻿using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;

namespace miniJ.Parsing.Elements
{
    class AssignmentExpression : Expression
    {
        public ISyntaxNode ParentNode { get; set; }
        public AssignmentExpression(Token origin) : base(origin)
        {
        }
    }
}