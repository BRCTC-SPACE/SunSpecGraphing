namespace SunSpecGraphing
{
    internal class SpectrometerDataHandler
    {
        // TODO: Rework this so that there is just an object that has all the data and different variables
        public UInt16[] Intensities = new UInt16[Global.SAMPLE_SIZE];

        private string filePath;
        private int MAX_VALUE;

        public SpectrometerDataHandler(string filePath)
        {
            this.filePath = filePath;
            LoadData();
        }

        public void LoadData()
        {
            // Load the data into currentData
            try
            {
                using (FileStream spectrometerData = new FileStream(filePath, FileMode.Open))
                using (BinaryReader buffer = new BinaryReader(spectrometerData))
                {
                    int count = 0;
                    while (buffer.BaseStream.Position < buffer.BaseStream.Length)
                    {
                        
                        ushort value = buffer.ReadUInt16();
                        Intensities[count] = value;
                        count+=1;
                    }
                    if (count != Global.SAMPLE_SIZE)
                    {
                        // TODO: Throw warning or something I geuss
                    }
                }

            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }

            // Set Variables
            MAX_VALUE = Intensities.Max();

            // Console.WriteLine(MAX_VALUE);
        }

        public int GetMaxIntensity()
        {
            return MAX_VALUE;
        }
    }
}
