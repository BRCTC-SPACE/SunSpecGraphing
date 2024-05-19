namespace SunSpecGraphing
{
    internal class Conversions
    {

        /* Greatly appriciated work here. Found from:
           https://stackoverflow.com/questions/1472514/convert-light-frequency-to-rgb
        
        ______________________________________________________________________________________________
           Taken from Earl F. Glynn's web page:
           <a href="http://www.efg2.com/Lab/ScienceAndEngineering/Spectra.htm">Spectra Lab Report</a>
        */
        public static Color wavelengthToARGB(double Wavelength)
        {
            double Gamma = 0.9;
            double IntensityMax = 255;
            double factor;
            double Red, Green, Blue;

            if ((Wavelength >= 380) && (Wavelength < 440))
            {
                Red = -(Wavelength - 440) / (440 - 380);
                Green = 0.0;
                Blue = 1.0;
            }
            else if ((Wavelength >= 440) && (Wavelength < 490))
            {
                Red = 0.0;
                Green = (Wavelength - 440) / (490 - 440);
                Blue = 1.0;
            }
            else if ((Wavelength >= 490) && (Wavelength < 510))
            {
                Red = 0.0;
                Green = 1.0;
                Blue = -(Wavelength - 510) / (510 - 490);
            }
            else if ((Wavelength >= 510) && (Wavelength < 580))
            {
                Red = (Wavelength - 510) / (580 - 510);
                Green = 1.0;
                Blue = 0.0;
            }
            else if ((Wavelength >= 580) && (Wavelength < 645))
            {
                Red = 1.0;
                Green = -(Wavelength - 645) / (645 - 580);
                Blue = 0.0;
            }
            else if ((Wavelength >= 645) && (Wavelength < 781))
            {
                Red = 1.0;
                Green = 0.0;
                Blue = 0.0;
            }
            else
            {
                Red = 0.0;
                Green = 0.0;
                Blue = 0.0;
            }

            // Let the intensity fall off near the vision limits

            if ((Wavelength >= 380) && (Wavelength < 420))
            {
                factor = 0.3 + 0.7 * (Wavelength - 380) / (420 - 380);
            }
            else if ((Wavelength >= 420) && (Wavelength < 701))
            {
                factor = 1.0;
            }
            else if ((Wavelength >= 701) && (Wavelength < 781))
            {
                factor = 0.3 + 0.7 * (780 - Wavelength) / (780 - 700);
            }
            else
            {
                factor = 0.0;
            }

            int ARGB_VALUE = unchecked((int)0xFF000000); // FF 00 00 00 : First is Alpha, Second is Red, so on
            ARGB_VALUE |= Red == 0.0 ? 0 : ((int)Math.Round(IntensityMax * Math.Pow(Red * factor, Gamma)) << 16);
            ARGB_VALUE |= Green == 0.0 ? 0 : ((int)Math.Round(IntensityMax * Math.Pow(Green * factor, Gamma)) << 8);
            ARGB_VALUE |= Blue == 0.0 ? 0 : (int)Math.Round(IntensityMax * Math.Pow(Blue * factor, Gamma));

            Color color = Color.FromArgb(ARGB_VALUE);

            return color;
        }
        

        public static Color IntensityToColor(UInt16 Intensity, Color color, int MaxValue)
        {
            // First convert the intensity to an alpha value using a similar method used in "CalculateAllPositionalWavelengthValues"
            int newAlpha = (int)((double)((double)Intensity / (double)MaxValue) * (double)255); // This gets the ratio of intensity to alpha value
            // Console.WriteLine($"Old: {Intensity}  -  New: {newAlpha}");
            int newARGB = (color.ToArgb() & 0x00FFFFFF) | (newAlpha << 24); // Shift the alpha to the proper place with masking
            Color newColor = Color.FromArgb(newARGB);
            return newColor;
        }
    }
}
