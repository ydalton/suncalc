/*
 * suncalc: a simple sunset/sunrise calculator for the command line
 *
 * This is the first .NET application I've written in a long time.
 * I have a lot more experience in writing C code, so this might
 * resemble C code in some ways.
 */

namespace SunCalc {
    class Program {
        // program info
        const string programName = "suncalc";
        const string version = "0.01";

        // the obliquity/tilt of the Earth in degrees
        const double obliquity = 23.44;
        const double yearLength = 365.242;
        // get declension of the sun
        // !not a problem
        public static double GetDeclension(int day) {
            return obliquity * Math.Sin(2 * Math.PI * ((double) day/yearLength) - 1.4);
        }

        // get time of sunset
        public static double GetHours(double lat, double decl) {
            return Math.Acos(-Math.Tan(lat) * Math.Tan(decl));
        }
        // get offset in hours from "real noon" and local noon.
        public static double GetOffset(double day) {
            double angle = (2 * Math.PI * day)/yearLength;
            // an approximation of the equation of time.
            double offset = 7.5 * Math.Sin(angle - 3.3) +
                9.8 * Math.Sin(2 * angle - 2.9);
            // offset in hours
            return offset / 60.0;
        }
        static void Usage() {
            Console.WriteLine("usage: " + programName + " [-v | --version] [-h | --help]");
        }
        static void Version() {
            Console.WriteLine(programName + " " + version);
        }
        static void Main(String[] args) {
            for(int i = 0; i < args.Length; i++) {
                switch(args[i]) {
                    case "-v":
                    case "--version":
                        Version();
                        Environment.Exit(0);
                        break;
                    case "-h":
                    case "--help":
                        Usage();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Unknown option '" + args[i] + "'.");
                        Environment.Exit(0);
                        break;
                }
            }
            const string invalid = "Please enter a valid number.";
            // query the latitude
            double latitude;
            Console.WriteLine("Latitude: ");
            while(!Double.TryParse(Console.ReadLine(), out latitude)) {
                Console.WriteLine(invalid);
            }

            // query the year
            int year;
            Console.WriteLine("Year: ");
            while(!Int32.TryParse(Console.ReadLine(), out year)) {
                Console.WriteLine(invalid);
            }

            // query the month
            byte month;
            Console.WriteLine("Month: ");
            while(!Byte.TryParse(Console.ReadLine(), out month)
                  || month > 12 || month < 1) {
                Console.WriteLine(invalid);
            }

            // query the day
            byte day;
            Console.WriteLine("Day: ");
            while(!Byte.TryParse(Console.ReadLine(), out day)
                  || day > 31 || day < 1) {
                Console.WriteLine(invalid);
            }

            byte monthLength = 0;
            // turn days of the month into days of the year
            for(int i = 0; i < month-1; i++) {
                switch(i+1) {
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
                    default:
                        Console.WriteLine("Stop.");
                        Environment.Exit(0);
                        break;
                }
                day += monthLength;
            }
            latitude *= (Math.PI / 180.0);
            double declension = (Math.PI / 180.0) * GetDeclension(day);
            double hours = (12.0/Math.PI) * GetHours(latitude, declension);

            double offset = GetOffset(day);
            double noon = 12 + offset;
            double sunrise = noon - hours;
            double sunset = noon + hours;
            double daylength = hours * 2;
            Console.WriteLine("Results:\nSunrise:\t" + Math.Floor(sunrise) + " "
                              + Math.Floor((sunrise % 1) * 60) + "\nSunset:\t\t"
                              + Math.Floor(sunset) + " " + Math.Floor((sunset % 1) * 60) + "\nDay length:\t"
                              + Math.Floor(daylength) + " " + Math.Floor((daylength % 1) * 60));
            Console.ReadKey();
        }
    }
}
