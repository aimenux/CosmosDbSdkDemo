namespace LibSdk2.Models.CreateModels
{
    public class CosmosDbCreateResponse
    {
        public double RequestUnits { get; }

        public CosmosDbCreateResponse(double requestUnits)
        {
            RequestUnits = requestUnits;
        }
    }
}