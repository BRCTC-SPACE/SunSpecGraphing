namespace SunSpecGraphing
{
    internal class SpectrometerHandler
    {
        static public UInt16[] currentData = new UInt16[Global.SAMPLE_SIZE];

        static public void PrintData()
        {
            try
            {
                using (FileStream spectrometerData = new FileStream("C:\\Users\\pokem\\Desktop\\Share\\AllData\\data.spec", FileMode.Open))
                using (BinaryReader buffer = new BinaryReader(spectrometerData))
                {
                    int count = 0;
                    while (buffer.BaseStream.Position < buffer.BaseStream.Length)
                    {
                        
                        ushort value = buffer.ReadUInt16();
                        // Console.WriteLine($"{value}  -  {Convert.ToString(value, 2).PadLeft(16, '0')}");
                        currentData[count] = value;
                        count+=1;
                    }
                }
            }
            catch (Exception error)
            {
                // Error
                Console.WriteLine(error.Message);
            }
        }
    }
}
