namespace LibSdk3.Models.DestroyModels
{
    public class CosmosDbDestroyResponse
    {
        public double RequestUnits { get; }

        public CosmosDbDestroyResponse(double requestUnits)
        {
            RequestUnits = requestUnits;
        }
    }
}