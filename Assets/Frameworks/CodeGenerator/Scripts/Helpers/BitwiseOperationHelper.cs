using System;

public static class BitwiseOperationHelper
{
    public static bool IsAndOperation(int input, int target)
    {
        return (input & target) == target;
    }
}