using System;

namespace BankEngine.Models;

public interface IOutputProvider
{
    public void WriteString(string s)
    {}
}

public interface IInputProvider
{
    public string GetString()
    {
        return "";
    }
}

public interface IIOProvider : IInputProvider, IOutputProvider {}

public class ConsoleInputProvider : IInputProvider
{
    public string GetString()
    {
        return Console.ReadLine();
    }
}

public class ConsoleOutputProvider : IOutputProvider
{
    public void WriteString(string s)
    {
        Console.WriteLine(s);
    }
}

public class ConsoleIOProvider : IIOProvider
{
    public string GetString()
    {
        return Console.ReadLine();
    }
    
    public void WriteString(string s)
    {
        Console.WriteLine(s);
    }
}
