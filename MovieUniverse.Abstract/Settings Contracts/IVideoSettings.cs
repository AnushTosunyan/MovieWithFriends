namespace MovieUniverse.Abstract.Settings_Contracts
{
    public interface IVideoSettings
    {
        string Format { get; set; }
        string Quaility { get; set; }
        double Duration { get; set; }
        string Language { get; set; }
    }
}
