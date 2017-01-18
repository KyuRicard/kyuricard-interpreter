using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_A
{
    class ExpressionEvaluator
    {
        static int currentIndex;
        static char currentChar;
        static Token currentToken;
        static string text;

        public static void EvaluateExpression(string expression)
        {
            currentChar = expression[0];
            currentIndex = 0;
            text = expression;
        }

        static bool Eat(Token.TokenType tt)
        {
            return currentToken.type != tt;
        }

        static void Advance()
        {
            ++currentIndex;
            currentChar = text[currentIndex];
        }

        static void ConsumeWhiteSpaces()
        {
            while ((currentChar == ' ' || currentChar == '\t') && currentIndex < text.Length - 1)
            {
                Advance();
            }
        }

        static void GetInteger()
        {
            bool isDecimal = false;
            string num = "";
            while((char.IsDigit(currentChar) || currentChar == '.') && currentIndex < text.Length - 1)
            {
                if (currentChar == '.' && !isDecimal)
                {
                    isDecimal = true;
                    num += '.';
                }
                else if (char.IsDigit(currentChar))
                {
                    num += currentChar;
                }
                Advance();
            }
            currentToken = new Token(num, isDecimal ? Token.TokenType.FLOAT : Token.TokenType.INTEGER);
        }

        static void GetNextToken()
        {
            //Really, how?
            if (currentChar == '\0')
                throw new ArgumentNullException("I don't know why you have a NULL character in C# strings. Dude or Chick, we're in 2017 and using" +
                    " C# not in the '80 and using C.");
            else if (char.IsDigit(currentChar))
            {
                GetInteger();
            }
            else if (char.IsWhiteSpace(currentChar) || currentChar == '\t')
            {
                ConsumeWhiteSpaces();
            }
            else if (currentChar == '+')
            {
                currentToken = new Token("+", Token.TokenType.PLUS);
                Advance();
            }
            else if (currentChar == '-')
            {
                currentToken = new Token("-", Token.TokenType.LESS);
                Advance();
            }
            else if (currentChar == '*')
            {
                currentToken = new Token("*", Token.TokenType.MULT);
                Advance();
            }
            else if (currentChar == '/')
            {
                currentToken = new Token("/", Token.TokenType.DIV);
                Advance();
            }
            else if (currentChar == '%')
            {
                currentToken = new Token("%", Token.TokenType.MOD);
                Advance();
            }
            else if (currentChar == '>')
            {
                Advance();
                if (currentChar == '=')
                {

                }
            }
        }
    }

    class Token
    {
        public enum TokenType {
            INTEGER, IDENTIFIER, FLOAT, PLUS, LESS, MULT, DIV, MOD, EQUALS, ISEQUALS, ISLESS, ISMORE, ISLESSEQ, ISMOREEQ, EOF, LEFTP, RIGHTP
        };
        public TokenType type;
        public string value;

        public Token(string value, TokenType type)
        {
            this.type = type;
            this.value = value;
        }
    }
}
