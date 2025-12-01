using UnityEngine;

public static class TextGenerator 
{
    public static char GenerateSymbol()
    {
        char symbol;
        symbol = (char)Random.Range(33, 128);
        return symbol;
    }

    public static string GenerateRandomText(int symbplsCount)
    {
        string randomStr = "";
        for (int i = 0; i < symbplsCount; i++)
        {
            randomStr += GenerateSymbol();
        }
        return randomStr;
    }
}
