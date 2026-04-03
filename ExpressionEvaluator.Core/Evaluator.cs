namespace ExpressionEvaluator.Core;

public class Evaluator
{
    public static double Evaluate(string infix)
    {
        var postfix = InfixToPostfix(infix);
        //Console.WriteLine("Postfix: " + postfix);
        return EvaluatePostfix(postfix);
    }

    private static string InfixToPostfix(string infix)
    {
        var postFix = "";
        var stack = new Stack<char>();
        var numero = "";

        foreach (var item in infix)
        {
         
            if (char.IsDigit(item) || item == '.')
            {
                numero += item;
            }
            else
            {
               
                if (numero != "")
                {
                    postFix += numero + " ";
                    numero = "";
                }

                if (IsOperator(item))
                {
                    if (item == '(')
                    {
                        stack.Push(item);
                    }
                    else if (item == ')')
                    {
                        while (stack.Peek() != '(')
                        {
                            postFix += stack.Pop() + " ";
                        }
                        stack.Pop();
                    }
                    else
                    {
                        while (stack.Count > 0 &&
                               PriorityInfix(item) <= PriorityStack(stack.Peek()))
                        {
                            postFix += stack.Pop() + " ";
                        }
                        stack.Push(item);
                    }
                }
            }
        }

       
        if (numero != "")
        {
            postFix += numero + " ";
        }

       
        while (stack.Count > 0)
        {
            postFix += stack.Pop() + " ";
        }

        return postFix.Trim();
    }
    private static int PriorityStack(char item) => item switch
    {
        '^' => 3,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 0,
        _ => throw new Exception("Sintax error."),
    };

    private static int PriorityInfix(char item) => item switch
    {
        '^' => 4,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 5,
        _ => throw new Exception("Sintax error."),
    };

    private static double EvaluatePostfix(string postfix)
    {
        var stack = new Stack<double>();
        var culture = System.Globalization.CultureInfo.InvariantCulture;

        foreach (var token in postfix.Split(' '))
        {
            if (double.TryParse(token, System.Globalization.NumberStyles.Any,
                   System.Globalization.CultureInfo.InvariantCulture, out double numero))
            {
                stack.Push(numero);
            }
            else
            {
                var b = stack.Pop();
                var a = stack.Pop();

                stack.Push(token switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "*" => a * b,
                    "/" => a / b,
                    "^" => Math.Pow(a, b),
                    _ => throw new Exception("Sintax error.")
                });
            }
        }

        return stack.Pop();
    }
    private static bool IsOperator(char item) => "+-*/^()".Contains(item);
}