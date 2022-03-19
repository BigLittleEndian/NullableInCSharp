using System.Diagnostics.CodeAnalysis;

namespace NullableExamples;

// Nullable Reference Types in C# 8+

public class Student
{
    public string FirstName;      // Will never be null
    public string LastName;       // Will never be null
    public string? MiddleName;    // Can be null

    //----------------------------------------------------------------------------
    // Constructor is making sure that First and Last name are not null.
    // Commenting first two lines will produce warning.
    //----------------------------------------------------------------------------
    public Student(string firstName, string lastName, string? middleName)
    {
        FirstName = firstName;
        LastName = lastName;

        // Commenting line below will not generate warning since MiddleName can be null
        MiddleName = middleName;
    }

    //--------------------------------------------------------------------------
    // In default constructor compiler knows that SetFirstLastNameEmpty()
    // helper method will set First and Last name since method is using
    // MemberNotNull attribute
    //
    // Example using MemberNotNull method and property helper methods attribute
    //--------------------------------------------------------------------------
    public Student()
    {
        // MemberNotNull attribute is marking First and Last name as not null
        SetFirstLastNameEmpty(); 
    }

    //------------------------------------------------------------------
    // In this constructor compiler knows that SetEmptyIfNull() helper
    // will make sure first and last name are not null. 
    // SetEmptyIfNull() is using postcondition NotNull attribute
    //
    // Example using NotNull postcondition attribute 
    //------------------------------------------------------------------
    public Student(string? firstName, string? lastName)
    {
        SetEmptyIfNull(ref firstName);
        SetEmptyIfNull(ref lastName);

        FirstName = firstName;
        LastName = lastName;

        MiddleName = null;
    }

    //---------------------------------------------------------------------------
    // Compiler is firing warning:
    // "'MiddleName' may be null here. Dereference of a possibly null reference"
    // Code will crash if MiddleName is null
    //---------------------------------------------------------------------------
    public int GetTotalLettersInNameWrong()
    {
        return  FirstName.Length + LastName.Length + MiddleName.Length;
    }

    //---------------------------------------------------------------------
    // Code is safe. MiddleName is checked for null before dereferenced.
    //---------------------------------------------------------------------
    public int GetTotalLettersInName()
    {
        int length = FirstName.Length + LastName.Length;

        if(MiddleName is not null)
        {
            length += MiddleName.Length;
        }

        return length;
    }

    //-----------------------------------------------------------------------
    // Code is NOT safe. Once checked MiddleCount will become null.
    //----------------------------------------------------------------------
    public int? MiddleCount => MiddleName?.Length;

    public int GetTotalLettersInNameNoWarning()
    {
        int length = FirstName.Length + LastName.Length;

        if(MiddleCount.HasValue)
        {
            MiddleName = null;      // MiddleCount becomes null after if guard
            length += MiddleCount.Value;
        }

        return length;
    }

    //-----------------------------------------------------------------------------------
    // Code is safe. Helper is checking MiddleName for null before dereferencing.
    // Helper is using conditional postcondition attribute NotNullWhen to help analysis.
    //
    // Example using NotNullWhen conditional postcondition attribute
    //-----------------------------------------------------------------------------------
    public int GetTotalLettersInNameWithConditionalHelper()
    {
        int length = FirstName.Length + LastName.Length;

        // Compiler knows that if StringIsNotNull returns true, MiddleName is not null
        // StringIsNotNull helper uses NotNullWhen conditional postcondition attribute
        if (StringIsNotNull(MiddleName))
        {
            length += MiddleName.Length;
        }

        return length;
    }

    //--------------------------------------------------------------------
    // No warning since FailIf will throw exception.
    // Helper is notifying compiler that code below is unreachable if
    // passed boolean is true (if MiddleName is null).
    //
    // Example using DoesNotReturnIf unreachable code attribute
    //--------------------------------------------------------------------
    public int GetTotalLettersInNameWithUnreachableCodeHelper()
    {
        int length = FirstName.Length + LastName.Length;

        // If true method will throw and code below will be unreachable
        FailIf(MiddleName is null); 

        return length + MiddleName.Length;
    }

    // ---- Helpers made using attributes -----

    //------------------------------------------------------------
    // Postcondition attributes:
    // 1) MaybeNull
    // 2) NotNull
    // 
    // Example using NotNull
    //------------------------------------------------------------
    public static void SetEmptyIfNull([NotNull] ref string? text)
    {
        if(text is null) 
        {
            text = string.Empty;
        }
    }

    //-----------------------------------------------------
    // Conditional postcondition attributes:
    // 1) MaybeNullWhen
    // 2) NotNullWhen
    // 3) NotNullIfNotNull
    //
    // Example using NotNullWhen
    //-----------------------------------------------------
    public static bool StringIsNotNull([NotNullWhen(true)] string? name) => name is not null;

    //------------------------------------------------------------------
    // Method and property helper methods attributes:
    // 1) MemberNotNull
    // 2) MemberNotNullWhen (using bool value that method returns)
    //
    // Example using MemberNotNull
    //------------------------------------------------------------------
    [MemberNotNull(nameof(FirstName), nameof(LastName))]
    public void SetFirstLastNameEmpty()
    {
        FirstName = LastName = string.Empty;
    }

    //-------------------------------------------------------------
    // Unreachable code attributes:
    // 1) DoesNotReturn
    // 2) DoesNotReturnIf
    //
    // Example using DoesNotReturnIf unreachable code attribute
    //-------------------------------------------------------------
    public static void FailIf([DoesNotReturnIf(true)] bool isNull)
    {
        if (isNull)
        {
            throw new InvalidOperationException();
        }
    }

    // Helper used in unit test example
    public void SetFirstName(string firstName)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
    }
}

public class NullableReferenceTypesExamples
{
    public static void Run()
    {
        var studentLuke = new Student("Luke", "Skywalker", null);
        var studentParker = new Student("Peter", "Parker", "Benjamin");

        // GetTotalLettersInName (no warnings, guard is used)
        Console.WriteLine("GetTotalLettersInName:");
        Console.WriteLine($"Letters for Luke: {studentLuke.GetTotalLettersInName()}");
        Console.WriteLine($"Letters for Peter: {studentParker.GetTotalLettersInName()}");

        // GetTotalLettersInNameNoWarning (not safe code)
        Console.WriteLine("\nGetTotalLettersInNameNoWarning:");
        Console.WriteLine($"Letters for Luke: {studentLuke.GetTotalLettersInNameNoWarning()}");
        //Console.WriteLine($"Letters for Peter: {studentParker.GetTotalLettersInNameNoWarning()}"); // Will throw

        // GetTotalLettersInNameWithConditionalHelper (NotNullWhen attribute example)
        Console.WriteLine("\nGetTotalLettersInNameWithConditionalHelper:");
        Console.WriteLine($"Letters for Luke: {studentLuke.GetTotalLettersInNameWithConditionalHelper()}");
        Console.WriteLine($"Letters for Peter: {studentParker.GetTotalLettersInNameWithConditionalHelper()}");

        // GetTotalLettersInNameWithUnreachableCodeHelper (DoesNotReturnIf attribute example)
        Console.WriteLine("\nGetTotalLettersInNameWithUnreachableCodeHelper:");
        // Console.WriteLine($"Letters for Luke: {studentLuke.GetTotalLettersInNameWithUnreachableCodeHelper()}"); // Will throw
        Console.WriteLine($"Letters for Peter: {studentParker.GetTotalLettersInNameWithUnreachableCodeHelper()}");

        // Set First Name (example is in unit test)
        studentParker.SetFirstName("Spider-Man");

        // Using default constructor with helper method (MemberNotNull attribute example)
        Console.WriteLine("\nUsing default constructor:");
        var emptyStudent = new Student();
        Console.WriteLine($"Letters for EmptyStudent: {emptyStudent.GetTotalLettersInName()}");

        // Using constructor with nullable first and last name parameters (NotNull attribute example)
        Console.WriteLine("\nUsing constructor with nullable arguments:");
        var anotherEmptyStudent = new Student(null, null);
        Console.WriteLine($"Letters for AnotherEmptyStudent: {anotherEmptyStudent.GetTotalLettersInName()}");        

    }

}