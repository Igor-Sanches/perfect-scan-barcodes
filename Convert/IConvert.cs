namespace Perfect_Scan.Convert
{
    public interface IConvert
    {
        bool Equals(object obj);
        int GetHashCode();
        string ToString();
    }
}