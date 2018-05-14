namespace CardCollectiveSession
{
    /// <summary>
    /// Карта
    /// </summary>
    public class Card : Container
    {
        public Card(int cardCost)
        {
            try
            { AddComponent(new Cost(cardCost)); }
            catch { throw; }
        }
    }
}