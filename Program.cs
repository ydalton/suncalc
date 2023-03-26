/*
 * suncalc: a simple sunset/sunrise calculator for the command line
 *
 * This is the first .NET application I've written in a long time.
 * I have a lot more experience in writing C code, so this might
 * resemble C code in some ways.
 */

namespace BesselCli {
    class Program {
        // program info
        const string programName = "suncalc";
        const string version = "0.01";

        // the obliquity/tilt of the Earth in degrees
        const double obliquity = 23.44;
        const double yearLength = 365.242;
        // get time of sunset
        public static double GetHours() {
            return 0;
        }
        // get offset in hours from "real noon" and local noon.
        public static double GetOffset(double day) {
            double angle = (2 * Math.PI * day)/yearLength;
            // an approximation of the equation of time.
            return 7.5 * Math.Sin(angle - 3.3) +
                9.8 * Math.Sin(2 * angle - 2.9);
        }
        static void Usage() {
            Console.WriteLine("usage: " + programName + " [-v | --version] [-h | --help]");
        }
        static void Main(String[] args) {
            for(int i = 0; i < args.Length; i++) {
                switch(args[i]) {
                    case "-v":
                    case "--version":
                        Environment.Exit(0);
                        break;
                    case "-h":
                    case "--help":
                        Environment.Exit(0);
                        break;
                }
            }
            const string invalid = "Please enter a valid number.";
            double latitude;
            Console.WriteLine("Latitude: ");
            while(!Double.TryParse(Console.ReadLine(), out latitude)) {
                Console.WriteLine(invalid);
            }
            int year;
            Console.WriteLine("Year: ");
            while(!Int32.TryParse(Console.ReadLine(), out year)) {
                Console.WriteLine(invalid);
            }
            byte month;
            Console.WriteLine("Month: ");
            while(!Byte.TryParse(Console.ReadLine(), out month)
                  || month > 12 || month < 1) {
                Console.WriteLine(invalid);
            }
            byte monthLength;
            // decide the month length
            switch(month) {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    monthLength = 31;
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    monthLength = 30;
                    break;
                case 2:
                    // assuming a Julian calendar for simplicity
                    monthLength = (byte) (((year % 4) == 0) ? 28 : 29);
                    break;
            }
            byte day;
            Console.WriteLine("Day: ");
            while(!Byte.TryParse(Console.ReadLine(), out day) || day > 31) {
                Console.WriteLine(invalid);
            }
        }
    }
}
