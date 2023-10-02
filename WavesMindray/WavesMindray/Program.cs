namespace WavesMindray
{
    internal class Program
    {   
        static Connection server = new Connection();

        public static void Main(string[] args)
        {
            //MindrayTranslate.TranslatorMindray();
            MindrayParameters mid = new();
            mid.TranslatorParameters();

        }
    }
}