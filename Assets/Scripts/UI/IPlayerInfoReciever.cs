public interface IPlayerInfoReciever
{
    public Player Player { get; }
    public void ReceivePlayerInfo(Player player);
}
