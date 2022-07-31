public interface DataBusInterface
{
    byte[] WriteBus(byte[] data);
    byte[] ReadBus();
    int GetSize();
}
