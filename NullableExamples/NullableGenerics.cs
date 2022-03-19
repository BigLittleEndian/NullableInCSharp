namespace NullableExamples;

/* Constraints on type parameters

    where T : class	    - Reference type. T must be a non-nullable reference type.
    where T : class?	- Reference type, either nullable or non-nullable. 
    where T : notnull	- Non-nullable type. The argument can be a non-nullable
                          reference type or a non-nullable value type.
 */

//------------------------------------------------------------------------------
// notnull constraint
//
// The type argument must be a non-nullable type. The argument can be:
// 1) non-nullable reference type in C# 8.0+
// 2) non-nullable value type
//------------------------------------------------------------------------------
public class NotNullableGeneric<T> where T: notnull
{
    public T Value { get; set; }

    // Since T is not nullable we can apply ? and create nullable:
    public T? NullableValue { get; set; }

    public NotNullableGeneric(T value, T? nullableValue)
    {
        Value = value;
        NullableValue = nullableValue;
    }

    public string? Text
    {
        get
        {
            // We need null-conditional operator ?. only for NullableValue
            // There is no need to guard Value due to constraint notnull
            return Value.ToString() +           // Not guarded
                   NullableValue?.ToString();   // Guarded with ?.
        }
    }
}


public class NullableGenericExamples
{
    public static void Run()
    {
        //
        // notnull constraint example
        //
        Console.WriteLine("NotNullableGeneric:");

        // Second parameter is string? so we can pass null
        var stringExample = new NotNullableGeneric<string>("Luke", null);
        Console.WriteLine("NullableGeneric<string>:");
        Console.WriteLine($"Text: {stringExample.Text}");

        // Both parameters are "int".
        // Strange, for reference type above we got "string, string?" types.
        var intExample = new NotNullableGeneric<int>(55, 44);
        Console.WriteLine("NullableGeneric<int>:");
        Console.WriteLine($"Text: {intExample.Text}");

        // Line below will generate warning since T = stirng? is violating notnull constraint
        var nullStringExample = new NotNullableGeneric<string?>(null, null);
        //Console.WriteLine($"Text: {nullStringExample.Text}"); // Code is crashing on null.ToString()

    }

}
