namespace TradeAppEntity
{
    public class ModelPrediction
    {
        public int PredictionId { get; private set; }
        public string Symbol { get; private set; }
        public decimal PredictionValue { get; private set; }
        public DateTime CreatedAt { get; private set; }
    }
}
