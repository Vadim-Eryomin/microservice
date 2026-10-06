namespace StarshipGame
{
    public static class AngleExtension
    {
        public static double Cos(this Angle angle)
        {
            return Math.Cos(angle);
        }

        public static double Sin(this Angle angle)
        {
            return Math.Sin(angle);
        }

        public static double Tan(this Angle angle)
        {
            return Math.Tan(angle);
        }

        public static double Sec(this Angle angle)
        {
            return 1 / Math.Cos(angle);
        }

        public static double Csc(this Angle angle)
        {
            return 1 / Math.Sin(angle);
        }

        public static double Cot(this Angle angle)
        {
            return 1 / Math.Tan(angle);
        }
    }
}
