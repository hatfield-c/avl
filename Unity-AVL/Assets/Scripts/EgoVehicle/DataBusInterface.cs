public interface DataBusInterface
{
    void WriteBus(byte[] data);
    byte[] ReadBus();
    int GetSize();
}
