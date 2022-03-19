namespace NullableExamples;

public class NullableReferencesPitfalls
{
    public struct Person 
    {
        public string Name; // For class you would get warning here
        public int Age;
    }

	public static void Run()
    {
        // 1) Arrays - no warnings and will crash at runtime
        string[] array = new string[10];
        // Console.WriteLine(array[5].Length);

        // 2) Default struct will create person with Name = null, no warning
        //    Code will crash at runtime on Name.ToUpper()
        // PrintPerson(default);
    }

    public static void PrintPerson(Person person)
    {
        Console.WriteLine($"{person.Name.ToUpper()} - {person.Age}");
    }
}


