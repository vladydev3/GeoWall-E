using System.Runtime.CompilerServices;

namespace GeoWall_E
{
    public class Lexer
    {
        private readonly string input;  // codigo
        private int currentIndex = 0;   // indice actual para iterar el codigo
        private int line = 1;   // linea actual
        private int column = 1; // columna actual
        private readonly Error errors;   // para almacenar los posibles errores

        public Lexer(string input)
        {
            this.input = input;
            errors = new Error();
        }

        public Error Errors => errors;

        public void SetError(string error)
        {
            errors.AddError(error);
        }

        private static readonly Dictionary<char, TokenType> singleCharTokens = new()
        {
            { ';', TokenType.EOL },
            { '(', TokenType.LParen },
            { ')', TokenType.RParen },
            { '{', TokenType.LBracket },
            { '}', TokenType.RBracket },
            { ',', TokenType.Comma },
            { '+', TokenType.Plus },
            { '-', TokenType.Minus },
            { '*', TokenType.Mult },
            { '%', TokenType.Mod },
            { '^', TokenType.Pow },
            { '!', TokenType.Not },
            { '_', TokenType.Underline },
        };

        public List<Token> Tokenize()
        {
            List<Token> tokens = new();

            while (currentIndex < input.Length) // iteramos por el codigo
            {
                char currentChar = input[currentIndex];

                if (char.IsLetter(currentChar))
                {
                    tokens.Add(ReadIdentificator());
                    continue;
                }

                if (currentChar == '"')
                {
                    tokens.Add(ReadString());
                    continue;
                }

                if (char.IsDigit(currentChar))
                {
                    tokens.Add(ReadNumber());
                    continue;
                }

                if (currentChar == '\n') // salto de linea
                {
                    column = 0;
                    line++;
                    currentIndex++;
                    continue;
                }

                if (char.IsWhiteSpace(currentChar))
                {
                    currentIndex++;
                    column++;
                    continue;
                }

                if (singleCharTokens.TryGetValue(currentChar, out TokenType tokenType))
                {
                    tokens.Add(new Token(tokenType, currentChar.ToString(), line, column - 1));
                    currentIndex++;
                    column++;
                    continue;
                }

                if (currentChar == '<')
                {
                    currentIndex++;
                    if (input[currentIndex] == '=')
                    {
                        tokens.Add(new Token(TokenType.LessOrEqual, "<=", line, column - 1));
                        currentIndex++;
                        column++;
                    }
                    else tokens.Add(new Token(TokenType.Less, "<", line, column - 1));
                    column++;
                    continue;
                }

                if (currentChar == '>')
                {
                    currentIndex++;
                    if (input[currentIndex] == '=')
                    {
                        tokens.Add(new Token(TokenType.GreaterOrEqual, ">=", line, column - 1));
                        currentIndex++;
                        column++;
                    }
                    else tokens.Add(new Token(TokenType.Greater, ">", line, column - 1));
                    column++;
                    continue;
                }

                if (currentChar == '/')
                {
                    currentIndex++;
                    if (input[currentIndex] == '/')
                    {
                        while (currentIndex < input.Length && input[currentIndex] != '\n')
                        {
                            currentIndex++;
                        }
                        continue;
                    }
                    else if (input[currentIndex] == '*')
                    {
                        currentIndex++;
                        while (currentIndex < input.Length && !(input[currentIndex] == '*' && input[currentIndex + 1] == '/'))
                        {
                            currentIndex++;
                        }
                        currentIndex += 2;
                        continue;
                    }
                    tokens.Add(new Token(TokenType.Slash, "/", line, column - 1));
                    currentIndex++;
                    column++;
                    continue;
                }

                if (currentChar == '=')
                {
                    currentIndex++;
                    if (input[currentIndex] == '=') tokens.Add(new Token(TokenType.Equal, "==", line, column - 1));
                    else tokens.Add(new Token(TokenType.Asignation, "=", line, column - 1));
                    column++;
                    continue;
                }

                errors.AddError($"Unexpected character '{currentChar}'");
                tokens.Add(new Token(TokenType.Error, currentChar.ToString(), line, column));
            }
            tokens.Add(new Token(TokenType.EOF, "", line, column));
            return tokens;
        }
        private Token ReadNumber()
        {
            string number = string.Empty;

            while (currentIndex < input.Length && char.IsLetterOrDigit(input[currentIndex]))
            {
                number += input[currentIndex];
                currentIndex++;
                column++;
            }

            if (currentIndex < input.Length && input[currentIndex] == '.')
            {
                number += input[currentIndex];
                currentIndex++;
                column++;
            }
            while (currentIndex < input.Length && char.IsLetterOrDigit(input[currentIndex]))
            {
                number += input[currentIndex];
                currentIndex++;
                column++;
            }

            if (!double.TryParse(number, out double result))
            {
                errors.AddError($"Invalid number '{number}'");
                return new Token(TokenType.Error, number, line, column - number.Length);
            }

            return new Token(TokenType.Number, number, line, column - number.Length);
        }
        private Token ReadString()
        {
            string str = string.Empty;
            currentIndex++;
            column++;

            while (currentIndex < input.Length && input[currentIndex] != '"')
            {
                str += input[currentIndex];
                currentIndex++;
                column++;
            }

            if (currentIndex >= input.Length)
            {
                errors.AddError("Unterminated string");
                return new Token(TokenType.Error, str, line, column - str.Length - 1);
            }

            currentIndex++;
            column++;
            return new Token(TokenType.String, str, line, column - str.Length - 1);
        }

        private static readonly Dictionary<string, TokenType> keywords = new()
        {
            { "undefined", TokenType.Undefined },
            { "rest", TokenType.Rest },
            { "sequence", TokenType.Sequence },
            { "line", TokenType.Line },
            { "segment", TokenType.Segment },
            { "ray", TokenType.Ray },
            { "circle", TokenType.Circle },
            { "color", TokenType.Color },
            { "restore", TokenType.Restore },
            { "import", TokenType.Import },
            { "draw", TokenType.Draw },
            { "arc", TokenType.Arc },
            { "measure", TokenType.Measure },
            { "intersect", TokenType.Intersect },
            { "count", TokenType.Count },
            { "randoms", TokenType.Randoms },
            { "point", TokenType.Point },
            { "points", TokenType.Points },
            { "samples", TokenType.Samples },
            { "let", TokenType.Let },
            { "in", TokenType.In },
            { "if", TokenType.If },
            { "then", TokenType.Then },
            { "else", TokenType.Else },
        };

        private Token ReadIdentificator()
        {
            string identificator = string.Empty;

            while (currentIndex < input.Length && char.IsLetterOrDigit(input[currentIndex]))
            {
                identificator += input[currentIndex];
                currentIndex++;
                column++;
            }

            if (keywords.TryGetValue(identificator, out TokenType tokenType))
            {
                return new Token(tokenType, identificator, line, column - identificator.Length);
            }

            return new Token(TokenType.Identifier, identificator, line, column - identificator.Length);
        }
    }
}