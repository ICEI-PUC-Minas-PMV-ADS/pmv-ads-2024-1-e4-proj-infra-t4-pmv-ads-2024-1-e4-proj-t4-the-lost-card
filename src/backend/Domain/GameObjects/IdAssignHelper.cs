namespace Domain.GameObjects;

public class IdAssignHelper
{
    public static long CalculateIdHash(string resourceStaticIndentifier)
    {
        ulong hashedValue = 3074457345618258791ul;
        for (int i = 0; i < resourceStaticIndentifier.Length; i++)
        {
            hashedValue += resourceStaticIndentifier[i];
            hashedValue *= 3074457345618258799ul;
        }
        return long.Parse(hashedValue.ToString()[..14]);
    }
}
