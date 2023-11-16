namespace GeoWall_E;

// Aqui va cada token posible
public enum TokenType
{
    Point, // point <id>
    Line, // line <id> | line(<id>, <id>)
    Segment, // segment <id> | segment(<id>, <id>)
    Ray, // ray <id> | ray(<id>, <id>)
    Circle, // circle <id> | circle(<id>, <id>)
    Sequence,
    Color, // color <black> | <white> | <red> | <green> | <blue> | <yellow>
    Restore,
    Import, // import <string>
    Draw, // draw <exp> <string> (optional)
    Arc, // arc(<id>, <id>, <id>, <measure>)
    Measure, // measure(<id>, <id>)
    Intersect, // intersect(<id>, <id>)
    Count, // count(<sequence>)
    Randoms, // randoms()
    Points, // points(<figure>)
    Samples, // samples()
    Number,
    String,
    Variable,
    Underline,
    Plus,
    Minus,
    Mult,
    Slash,
    Mod,
    Pow,
    Or,
    And,
    Equal,
    GreaterOrEqual,
    LessOrEqual,
    Greater,
    Less,
    Asignation,
    Identifier,
    LParen,
    RParen,
    LBracket,
    RBracket,
    Comma,
    WhiteSpace,
    Let,
    In,
    If,
    Then,
    Else,
    Error,
    Empty,
    BinaryExpression,
    UnaryExpression,
    ParenExpression,
    LetInExpression,
    FunctionCallExpression,
    FunctionDeclaration,
    AsignationStatement,
    EOL,
    EOF
}

public class Token
{
    public TokenType Type { get; set; }
    public string Text { get; set; }
    public int Line { get; set; }
    public int Column { get; set; }

    public Token(TokenType type, string text, int line, int column)
    {
        Type = type;
        Text = text;
        Line = line;
        Column = column;
    }
}