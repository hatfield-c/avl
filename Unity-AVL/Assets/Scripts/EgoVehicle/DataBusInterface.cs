public interface DataBusInterface
{
    void WriteData(byte[] data);
    byte[] ReadData(byte address);
}
