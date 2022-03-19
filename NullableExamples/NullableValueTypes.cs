namespace NullableExamples;

// Nullable Value Types in C# 2

public class NullableValueTypes
{
    public static void MagicVsNull()
    {
        int magicNumber = -1;       // -1 represents empty or undefined
        int? nullableNumber = null; // Nullable<int>

        // Check is number valid (provided)

        if (magicNumber == -1) // Magic value of -1
        {
            Console.WriteLine("magicNumber is not provided");
        }

        if (!nullableNumber.HasValue) // Nullable<int> structure has a property HasValue
        {
            Console.WriteLine("nullableNumber is not provided");
        }

        // Nullable number, check value

        nullableNumber = 55;

        if(nullableNumber.HasValue && nullableNumber > 40)
        {
            Console.WriteLine("nullableNumber is greater than 40");
        }

        if(nullableNumber is > 50) 
        {
            Console.WriteLine("nullableNumber is greater than 50");
        }
    }
}


