using System.Runtime.CompilerServices;

namespace GeoWallE;

public class Lexer
{
    private readonly string input;  // codigo
    private int currentIndex = 0;   // indice actual para iterar el codigo
    private int line = 1;   // linea actual
    private int column = 1; // columna actual
    public Errors errors = new();   // para almacenar los posibles errores

    public Lexer(string input)
    {
        this.input = input;
    }

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

            if (char.IsWhiteSpace(currentChar))
            {
                currentIndex++;
                column++;
                continue;
            }

            if (currentChar == '\n') // salto de linea
            {
                column = 0;
                line++;
                currentIndex++;
                continue;
            }

            if (currentChar == '\t') // tabulacion
            {
                column += 4;
                currentIndex++;
                continue;
            }

            if (currentChar == ';')
            {
                tokens.Add(new Token(TokenType.EOL, ";", line, column-1));
                currentIndex++;
                line++;
                column = 0;
                continue;
            }

            if (currentChar == '(')
            {
                tokens.Add(new Token(TokenType.LParen, "(", line, column-1));
                currentIndex++;
                column++;
                continue;
            }

            if (currentChar == ')')
            {
                tokens.Add(new Token(TokenType.RParen, ")", line, column-1));
                currentIndex++;
                column++;
                continue;
            }

            if (currentChar == '{')
            {
                tokens.Add(new Token(TokenType.LBracket, "{", line, column-1));
                currentIndex++;
                column++;
                continue;
            }

            if (currentChar == '}')
            {
                tokens.Add(new Token(TokenType.RBracket, "}", line, column-1));
                currentIndex++;
                column++;
                continue;
            }

            if (currentChar == ',')
            {
                tokens.Add(new Token(TokenType.Comma, ",", line, column-1));
                currentIndex++;
                column++;
                continue;
            }

            if (currentChar == '+')
            {
                tokens.Add(new Token(TokenType.Plus, "+", line, column-1));
                currentIndex++;
                column++;
                continue;
            }

            if (currentChar == '-')
            {
                tokens.Add(new Token(TokenType.Minus, "-", line, column-1));
                currentIndex++;
                column++;
                continue;
            }

            if (currentChar == '*')
            {
                tokens.Add(new Token(TokenType.Mult, "*", line, column-1));
                currentIndex++;
                column++;
                continue;
            }

            if (currentChar == '/')
            {
                tokens.Add(new Token(TokenType.Slash, "/", line, column-1));
                currentIndex++;
                column++;
                continue;
            }

            if (currentChar == '%')
            {
                tokens.Add(new Token(TokenType.Mod, "%", line, column-1));
                currentIndex++;
                column++;
                continue;
            }

            if (currentChar == '^')
            {
                tokens.Add(new Token(TokenType.Pow, "^", line, column-1));
                currentIndex++;
                column++;
                continue;
            }

            if (currentChar == '=')
            {
                tokens.Add(new Token(TokenType.Asignation, "=", line, column-1));
                currentIndex++;
                column++;
                continue;
            }

            if (currentChar == '_')
            {
                tokens.Add(new Token(TokenType.Underline, "_", line, column-1));
                currentIndex++;
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
            return new Token(TokenType.Error, number, line, column-number.Length);            
        }

        return new Token(TokenType.Number, number, line, column-number.Length);
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
            return new Token(TokenType.Error, str, line, column-str.Length-1);
        }

        currentIndex++;
        column++;
        return new Token(TokenType.String, str, line, column-str.Length-1);
    }
    private Token ReadIdentificator()
    {
        string identificator = string.Empty;

        while (currentIndex < input.Length && char.IsLetterOrDigit(input[currentIndex]))
        {
            identificator += input[currentIndex];
            currentIndex++;
            column++;
        }

        if (identificator == "sequence")
        {
            return new Token(TokenType.Sequence, identificator, line, column-identificator.Length);
        }

        if (identificator == "line")
        {
            return new Token(TokenType.Line, identificator, line, column-identificator.Length);
        }

        if (identificator == "segment")
        {
            return new Token(TokenType.Segment, identificator, line, column-identificator.Length);
        }

        if (identificator == "ray")
        {
            return new Token(TokenType.Ray, identificator, line, column-identificator.Length);
        }

        if (identificator == "circle")
        {
            return new Token(TokenType.Circle, identificator, line, column-identificator.Length);
        }

        if (identificator == "color")
        {
            return new Token(TokenType.Color, identificator, line, column-identificator.Length);
        }

        if (identificator == "restore")
        {
            return new Token(TokenType.Restore, identificator, line, column-identificator.Length);
        }

        if (identificator == "import")
        {
            return new Token(TokenType.Import, identificator, line, column-identificator.Length);
        }

        if (identificator == "draw")
        {
            return new Token(TokenType.Draw, identificator, line, column-identificator.Length);
        }

        if (identificator == "arc")
        {
            return new Token(TokenType.Arc, identificator, line, column-identificator.Length);
        }

        if (identificator == "measure")
        {
            return new Token(TokenType.Measure, identificator, line, column-identificator.Length);
        }

        if (identificator == "intersect")
        {
            return new Token(TokenType.Intersect, identificator, line, column-identificator.Length);
        }

        if (identificator == "count")
        {
            return new Token(TokenType.Count, identificator, line, column-identificator.Length);
        }

        if (identificator == "randoms")
        {
            return new Token(TokenType.Randoms, identificator, line, column-identificator.Length);
        }

        if (identificator == "point")
        {
            return new Token(TokenType.Point, identificator, line, column-identificator.Length);
        }

        if (identificator == "points")
        {
            return new Token(TokenType.Points, identificator, line, column-identificator.Length);
        }

        if (identificator == "samples")
        {
            return new Token(TokenType.Samples, identificator, line, column-identificator.Length);
        }

        if (identificator == "let") 
        {
            return new Token(TokenType.Let, identificator, line, column-identificator.Length);
        }

        if (identificator == "in")
        {
            return new Token(TokenType.In, identificator, line, column-identificator.Length);
        }

        return new Token(TokenType.Identifier, identificator, line, column-identificator.Length);
    }
}